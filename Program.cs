using System.Text.RegularExpressions;
using Bandwidth.Standard.Api;
using Bandwidth.Standard.Client;
using Bandwidth.Standard.Model;

//Setting up environment variables
string? BW_ACCOUNT_ID = Environment.GetEnvironmentVariable("BW_ACCOUNT_ID");
string? BW_CLIENT_ID = Environment.GetEnvironmentVariable("BW_CLIENT_ID");
string? BW_CLIENT_SECRET = Environment.GetEnvironmentVariable("BW_CLIENT_SECRET");
string? BW_NUMBER = Environment.GetEnvironmentVariable("BW_NUMBER");
string? BW_VOICE_APPLICATION_ID = Environment.GetEnvironmentVariable("BW_VOICE_APPLICATION_ID");
string? BW_MESSAGING_APPLICATION_ID = Environment.GetEnvironmentVariable("BW_MESSAGING_APPLICATION_ID");

if (BW_ACCOUNT_ID == null || BW_CLIENT_ID == null || BW_CLIENT_SECRET == null || BW_NUMBER == null
    || BW_VOICE_APPLICATION_ID == null || BW_MESSAGING_APPLICATION_ID == null)
{
    Console.WriteLine("Please set the environmental variables defined in the README");
    Environment.Exit(1);
    return;
}

Configuration configuration = new Configuration();
configuration.OAuthClientId = BW_CLIENT_ID;
configuration.OAuthClientSecret = BW_CLIENT_SECRET;

const string SCOPE = "sample";

string Prompt(string text)
{
    Console.Write(text);
    string? line = Console.ReadLine();
    if (line == null)
    {
        Environment.Exit(1);
    }
    return line.Trim();
}

string phoneNumber = Prompt("Enter the phone number to send the code to (E.164, e.g. +15554443333): ");
if (!Regex.IsMatch(phoneNumber, @"^\+[1-9]\d{1,14}$"))
{
    Console.Error.WriteLine("Phone number must be in E.164 format, e.g. +15554443333");
    Environment.Exit(1);
}

string method = Prompt("Send the code by messaging or voice? [messaging]: ").ToLower();
if (method == "")
{
    method = "messaging";
}
if (method != "messaging" && method != "voice")
{
    Console.Error.WriteLine("Method must be messaging or voice");
    Environment.Exit(1);
}

MFAApi mfaApi = new MFAApi(configuration);

CodeRequest codeRequest = new CodeRequest(
    to: phoneNumber,
    from: BW_NUMBER,
    applicationId: method == "voice" ? BW_VOICE_APPLICATION_ID : BW_MESSAGING_APPLICATION_ID,
    scope: SCOPE,
    message: "Your temporary {NAME} {SCOPE} code is {CODE}",
    digits: 6
);

Console.WriteLine($"Sending a code by {method} to {phoneNumber}");
try
{
    if (method == "voice")
    {
        await mfaApi.GenerateVoiceCodeAsync(BW_ACCOUNT_ID, codeRequest);
    }
    else
    {
        await mfaApi.GenerateMessagingCodeAsync(BW_ACCOUNT_ID, codeRequest);
    }
}
catch (Exception e)
{
    Console.Error.WriteLine($"Failed to send code: {e.Message}");
    Environment.Exit(1);
}

string code = Prompt("Enter the code you received: ");

bool valid = false;
try
{
    VerifyCodeRequest verifyRequest = new VerifyCodeRequest(
        to: phoneNumber,
        scope: SCOPE,
        expirationTimeInMinutes: 3,
        code: code
    );
    valid = (await mfaApi.VerifyCodeAsync(BW_ACCOUNT_ID, verifyRequest)).Valid;
}
catch (Exception e)
{
    Console.Error.WriteLine($"Failed to verify code: {e.Message}");
    Environment.Exit(1);
}

Console.WriteLine(valid ? "Code verified" : "Code not verified");
Environment.Exit(valid ? 0 : 1);

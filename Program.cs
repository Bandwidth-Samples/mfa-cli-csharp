using Bandwidth.Standard.Api;
using Bandwidth.Standard.Client;
using Bandwidth.Standard.Model;

string BW_USERNAME;
string BW_PASSWORD;
string BW_MESSAGING_APPLICATION_ID;
string BW_ACCOUNT_ID;
string BW_VOICE_APPLICATION_ID;
string BW_NUMBER;
string USER_NUMBER;

//Setting up environment variables
try
{
    BW_USERNAME = Environment.GetEnvironmentVariable("BW_USERNAME");
    BW_PASSWORD = Environment.GetEnvironmentVariable("BW_PASSWORD");
    BW_MESSAGING_APPLICATION_ID = Environment.GetEnvironmentVariable("BW_MESSAGING_APPLICATION_ID");
    BW_ACCOUNT_ID = Environment.GetEnvironmentVariable("BW_ACCOUNT_ID");
    BW_VOICE_APPLICATION_ID = System.Environment.GetEnvironmentVariable("BW_VOICE_APPLICATION_ID");
    BW_NUMBER = System.Environment.GetEnvironmentVariable("BW_NUMBER");
    USER_NUMBER = System.Environment.GetEnvironmentVariable("USER_NUMBER");
}
catch (Exception)
{
    Console.WriteLine("Please set the environmental variables defined in the README");
    throw;
}


Configuration configuration = new Configuration();
configuration.Username = BW_USERNAME;
configuration.Password = BW_PASSWORD;

MFAApi mfaApiInstance = new MFAApi(configuration);

Console.WriteLine("Enter phone number in E164 format (+15554443333): ");
string phoneNumber = Console.ReadLine();

Console.WriteLine("Enter MFA request method (voice/messaging). Default is messaging:");
string method = Console.ReadLine();

CodeRequest codeRequest = new CodeRequest(
    to: phoneNumber,
    from: BW_NUMBER,
    applicationId: method.ToLower() == "voice" ? BW_VOICE_APPLICATION_ID : BW_MESSAGING_APPLICATION_ID,
    scope: "scope",
    digits: 6,
    message: "Your temporary {NAME} {SCOPE} code is {CODE}"
);

if (method.ToLower() == "voice")
{
    mfaApiInstance.GenerateVoiceCode(BW_ACCOUNT_ID, codeRequest);
}
else
{
    mfaApiInstance.GenerateMessagingCode(BW_ACCOUNT_ID, codeRequest);
}

Console.WriteLine("Enter the code you received: ");
string code = Console.ReadLine();

VerifyCodeRequest verifyCodeRequest = new VerifyCodeRequest(
    to: phoneNumber,
    scope: "scope",
    expirationTimeInMinutes: 3,
    code: code
);

if (mfaApiInstance.VerifyCode(BW_ACCOUNT_ID, verifyCodeRequest).Valid)
{
    Console.WriteLine("Code is valid");
}
else
{
    Console.WriteLine("Code is invalid");
}

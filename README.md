# Multi-Factor Auth CLI

<a href="https://dev.bandwidth.com/docs/mfa/">
  <img src="./icon-mfa.svg" title="Multi-Factor Auth About Page" alt="Multi-Factor Auth About Page"/>
</a>

 # Table of Contents

- [Multi-Factor Auth CLI](#multi-factor-auth-cli)
- [Table of Contents](#table-of-contents)
- [Description](#description)
- [Pre-Requisites](#pre-requisites)
- [Environmental Variables](#environmental-variables)
- [Running the Application](#running-the-application)

# Description

This app creates a simple CLI tool used to create and verify multi-factor auth codes using Bandwidth's Multi-Factor Auth API. The app will prompt the user for their phone number, followed by their preferred method of code delivery; either messaging or voice. The app will then text or call the phone number provided with a 6 digit MFA code that the user can enter back into the CLI to verify.

# Pre-Requisites

In order to use the Bandwidth API users need to set up the appropriate application at the [Bandwidth Dashboard](https://dashboard.bandwidth.com/) and create API tokens.

To create an application log into the [Bandwidth Dashboard](https://dashboard.bandwidth.com/) and navigate to the `Applications` tab.  Fill out the **New Application** form selecting the service that the application will be used for (this sample app uses a messaging application).

For more information about API credentials see our [Account Credentials](https://dev.bandwidth.com/docs/account/credentials) page.

# Environmental Variables

The sample app uses the below environmental variables.

```sh
BW_ACCOUNT_ID                        # Your Bandwidth Account Id
BW_USERNAME                          # Your Bandwidth API Username
BW_PASSWORD                          # Your Bandwidth API Password
BW_NUMBER                            # The Bandwidth phone number involved with this application
BW_VOICE_APPLICATION_ID              # Your Voice Application Id created in the dashboard
BW_MESSAGING_APPLICATION_ID          # Your Messaging Application Id created in the dashboard
```

# Running the Application

Use the following command to run the application:

```sh
dotnet run
```

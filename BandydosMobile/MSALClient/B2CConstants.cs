namespace BandydosMobile.MSALClient
{
    public static class B2CConstants
    {
        // Azure AD B2C Coordinates
        public const string Tenant = "Bandydos.onmicrosoft.com";
        public const string AzureADB2CHostname = "Bandydos.b2clogin.com";
        public const string ClientID = "3f779296-d530-4af4-97d2-4debfbef0733";
        public static readonly string RedirectUri = $"msal3f779296-d530-4af4-97d2-4debfbef0733://auth";
        public const string PolicySignUpSignIn = "B2C_1_signupsignin";

        public static readonly string[] Scopes = { "https://fabrikamb2c.onmicrosoft.com/helloapi/demo.read" };

        public static readonly string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
        public static readonly string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";

        public const string IOSKeyChainGroup = "com.microsoft.adalcache";
    }
}

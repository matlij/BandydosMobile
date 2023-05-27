using BandydosMobile.Models;
using BandydosMobile.MSALClient;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace BandydosMobile.Services;

public class Authenticator
{
    public async Task<User?> GetLoggedInUserAsync()
    {
        try
        {
            var result = await PCAWrapperB2C.Instance.AcquireTokenSilentAsync(B2CConstants.Scopes);
            return ConvertToUser(result);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Misslyckades att hämta användare: " + e.Message);
            return null;
        }
    }

    public async Task<User?> SingInASync(bool useEmbedded = false)
    {
        AuthenticationResult result;
        try
        {
            //PCAWrapperB2C.Instance.UseEmbedded = useEmbedded;
            // First attempt silent login, which checks the cache for an existing valid token.
            // If this is very first time or user has signed out, it will throw MsalUiRequiredException
            result = await PCAWrapperB2C.Instance.AcquireTokenSilentAsync(B2CConstants.Scopes);
        }
        catch (MsalUiRequiredException)
        {
            // This executes UI interaction to obtain token
            result = await PCAWrapperB2C.Instance.AcquireTokenInteractiveAsync(B2CConstants.Scopes);
        }

        if (result is null)
        {
            throw new InvalidOperationException("Failed to Sing in user");
        }

        return ConvertToUser(result);
    }

    private static User? ConvertToUser(AuthenticationResult? result)
    {
        if (result is null)
        {
            return null;
        }

        var profile = result.Account.GetTenantProfiles().FirstOrDefault();
        var id = profile?.Oid;
        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidOperationException("Failed to find user ID for user + " + result.Account.Username);
        }

        return new User()
        {
            Id = id ,
            Name = result.ClaimsPrincipal.FindFirst("name")?.Value ?? result.Account.Username,
        };
    }

    public Task SignOutAsync()
    {
        return PCAWrapperB2C.Instance.SignOutAsync();
    }

    // Call the web api. The code is left in the Ux file for easy to see.
    public async Task<string> CallWebAPIWithToken(AuthenticationResult authResult)
    {
        try
        {
            //get data from API
            var client = new HttpClient();
            // create the request
            var message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");

            // ** Add Authorization Header **
            message.Headers.Add("Authorization", authResult.CreateAuthorizationHeader());

            // send the request and return the response
            HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseString;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}


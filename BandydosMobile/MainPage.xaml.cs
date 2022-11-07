using BandydosMobile.MSALClient;
using BandydosMobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Identity.Client;
using System.Windows.Input;

namespace BandydosMobile;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    //protected override void OnAppearing()
    //{
    //    _viewModel.SingInCommand.Execute(null);
    //}
}

public class Authenticator
{
    public async Task<AuthenticationResult> SingInASync(bool useEmbedded = false)
    {
        try
        {
            PCAWrapper.Instance.UseEmbedded = useEmbedded;
            // First attempt silent login, which checks the cache for an existing valid token.
            // If this is very first time or user has signed out, it will throw MsalUiRequiredException
            var result = await PCAWrapper.Instance.AcquireTokenSilentAsync(AppConstants.Scopes);

            return result;
        }
        catch (MsalUiRequiredException)
        {
            // This executes UI interaction to obtain token
            var result = await PCAWrapper.Instance.AcquireTokenInteractiveAsync(AppConstants.Scopes);
            return result;
        }
    }

    public Task SignOutAsync()
    {
        return PCAWrapper.Instance.SignOutAsync();
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


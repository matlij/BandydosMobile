using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using BandydosMobile.MSALClient;
using Microsoft.Identity.Client;

namespace BandydosMobile.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const string AndroidRedirectURI = $"msal{AppConstants.ClientId}://auth";

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        // configure platform specific params
        PlatformConfig.Instance.RedirectUri = AndroidRedirectURI;
        PlatformConfig.Instance.ParentWindow = this;
    }

    /// <summary>
    /// This is a callback to continue with the authentication
    /// Info about redirect URI: https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-client-application-configuration#redirect-uri
    /// </summary>
    /// <param name="requestCode">request code </param>
    /// <param name="resultCode">result code</param>
    /// <param name="data">intent of the actvity</param>
    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}

[Activity(Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
    DataHost = "auth",
    DataScheme = "msal4b706872-7c33-43f0-9325-55bf81d39b93")]
public class MsalActivity : BrowserTabActivity
{
}

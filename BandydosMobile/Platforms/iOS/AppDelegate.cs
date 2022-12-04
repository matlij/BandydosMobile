using BandydosMobile.MSALClient;
using Foundation;
using UIKit;

namespace BandydosMobile.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    private const string iOSRedirectURI = "msauth.com.companyname.mauiappbasic://auth"; // TODO - Replace with your redirectURI
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // configure platform specific params
        PlatformConfig.Instance.RedirectUri = iOSRedirectURI;

        return base.FinishedLaunching(application, launchOptions);
    }
}

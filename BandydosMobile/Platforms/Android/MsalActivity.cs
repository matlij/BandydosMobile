using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace BandydosMobile.Platforms.Android;

[Activity(Exported = true)]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
    DataHost = "auth",
    DataScheme = "msal3f779296-d530-4af4-97d2-4debfbef0733")]
public class MsalActivity : BrowserTabActivity
{
}

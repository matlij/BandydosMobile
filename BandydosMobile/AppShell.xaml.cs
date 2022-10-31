namespace BandydosMobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(EventsPage), typeof(EventsPage));
	}
}

using BandydosMobile.Repository;

namespace BandydosMobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		//Routing.RegisterRoute(nameof(EventsPage), typeof(EventsPage));
        Routing.RegisterRoute(nameof(EventsPage), typeof(EventsPage));
        Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));

    }
}

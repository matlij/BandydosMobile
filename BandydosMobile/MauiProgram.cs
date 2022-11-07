using BandydosMobile.Models;
using BandydosMobile.Models.Translation;
using BandydosMobile.Repository;
using BandydosMobile.Services;
using BandydosMobile.ViewModels;

namespace BandydosMobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddTransient<Authenticator>();
        builder.Services.AddTransient<IEventDataStore, EventDataStore>();
		builder.Services.AddTransient<IDataStore<User>, UserDataStore>();
        builder.Services.AddTransient<IGenericRepository, GenericRepository>();

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<EventsViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<EventsPage>();

        builder.Services.AddAutoMapper(typeof(BandydosProfile));

        return builder.Build();
	}
}

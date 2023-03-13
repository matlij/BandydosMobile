using BandydosMobile.Models;
using BandydosMobile.Models.Translation;
using BandydosMobile.Repository;
using BandydosMobile.Services;
using BandydosMobile.ViewModels;
using CommunityToolkit.Maui;

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

        builder.UseMauiApp<App>().UseMauiCommunityToolkit();

        builder.Services.AddTransient<Authenticator>();
        builder.Services.AddTransient<IEventDataStore, EventDataStore>();
		builder.Services.AddTransient<IDataStore<User>, UserDataStore>();
        builder.Services.AddTransient<IDataStore<EventUser>, EventUserDataStore>();
        builder.Services.AddTransient<IGenericRepository, GenericRepository>();

        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddTransient<EventsViewModel>();
        builder.Services.AddTransient<EventDetailViewModel>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<EventDetailPage>();

        builder.Services.AddAutoMapper(typeof(BandydosProfile));

        return builder.Build();
	}
}

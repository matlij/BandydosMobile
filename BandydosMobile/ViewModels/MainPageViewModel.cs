using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BandydosMobile.ViewModels;

public partial class MainPageViewModel : ObservableObject
{

    public MainPageViewModel(Authenticator authenticator)
    {
        Authenticator = authenticator;
    }

    private Authenticator Authenticator { get; }

    [RelayCommand]
    public async Task LoginAndGoToEventsAsync()
    {
        try
        {
            var result = await Authenticator.SingInASync();
            await Shell.Current.GoToAsync(nameof(EventsPage));
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Inloggning misslyckades", e.Message, "Stäng");
        }
    }
}


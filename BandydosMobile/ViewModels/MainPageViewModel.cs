using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BandydosMobile.ViewModels;

public partial class MainPageViewModel : ObservableObject
{

    public MainPageViewModel(Authenticator authenticator)
    {
        Authenticator = authenticator;
        //SingInCommand = new Command(async () =>
        //{
        //});
    }

    private Authenticator Authenticator { get; }
    //public ICommand SingInCommand { get; }

    [RelayCommand]
    async Task Navigate()
    {
        try
        {
            //var result = await Authenticator.SingInASync();
            await Shell.Current.GoToAsync(nameof(EventsPage));
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Inloggning misslyckades", e.Message, "Stäng");
        }
    }
}


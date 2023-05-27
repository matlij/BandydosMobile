using BandydosMobile.Models;
using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BandydosMobile.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private User? _user;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateUserCommand))]
    private string _userName;
    [ObservableProperty]
    private string _loginStatus;
    [ObservableProperty]
    private bool _isLoggedIn;
    public LoginPageViewModel(Authenticator authenticator, IDataStore<User> userDataStore)
    {
        Authenticator = authenticator;
        UserDataStore = userDataStore;
        Title = "Min sida";
    }

    private Authenticator Authenticator { get; }
    private IDataStore<User> UserDataStore { get; }

    public async Task Init()
    {
        var user = await Authenticator.GetLoggedInUserAsync();
        if (user != null)
        {
            LoginStatus = $"Du är inloggad som:{Environment.NewLine}{user.Name}";
            UserName = user.Name;
            User = user;
            IsLoggedIn = true;
        }
        else
        {
            LoginStatus = $"Du är inte inloggad";
            IsLoggedIn = false;
        }
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        try
        {
            var result = await Authenticator.SingInASync();
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Nånting blev fel", e.Message, "Stäng");
        }
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        try
        {
            await Authenticator.SignOutAsync();
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Nånting blev fel", e.Message, "Stäng");
        }
    }

    [RelayCommand(CanExecute = nameof(CanUpdateUser))]
    public async Task UpdateUserAsync()
    {
        try
        {
            if (_user is null)
            {
                await Application.Current.MainPage.DisplayAlert("Nånting blev fel", "Du är inte inloggad eller ingen användare kunde hittas", "Stäng");
                return;
            }

            _user.Name = _userName;

            var result = await UserDataStore.UpdateAsync(_user.Id, _user);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Uppdatering lyckades!", $"Ditt användarnamn har ändrats till {_userName}.{Environment.NewLine}Du kommer loggas ut för att ändringen ska gå igenom.", "OK");
                await LogoutAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Nånting blev fel", "Namnet uppdaterades inte, prova igen senare", "Stäng");
            }

        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Nånting blev fel", e.Message, "Stäng");
        }
    }

    private bool CanUpdateUser()
    {
        return _user != null && _userName != _user.Name;
    }
}


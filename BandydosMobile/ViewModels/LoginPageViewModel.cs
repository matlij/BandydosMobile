using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BandydosMobile.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _userName;
    [ObservableProperty]
    private string _loginStatus;
    [ObservableProperty]
    private bool _isLoggedIn;
    public LoginPageViewModel(Authenticator authenticator)
    {
        Authenticator = authenticator;
    }

    private Authenticator Authenticator { get; }

    public async Task Init()
    {
        var userName = await Authenticator.GetLoggedInUserNameAsync();
        if (userName != null)
        {
            LoginStatus = $"Du är inloggad som:{Environment.NewLine}{userName}";
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
            await Init();
            //await Shell.Current.GoToAsync("..");
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
            await Init();
            //await Shell.Current.GoToAsync("..");
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Nånting blev fel", e.Message, "Stäng");
        }
    }
}


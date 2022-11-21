using BandydosMobile.MSALClient;
using BandydosMobile.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Identity.Client;
using System.Windows.Input;

namespace BandydosMobile;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        await _viewModel.LoginAndGoToEventsAsync();
    }
}


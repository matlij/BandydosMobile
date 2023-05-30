using BandydosMobile.ViewModels;

namespace BandydosMobile;

public partial class MainPage : ContentPage
{
    private readonly EventsViewModel _viewModel;

    public MainPage(EventsViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        _viewModel.OnAppearing();
    }

    private async void MyPage_Clicked(object sender, EventArgs e)
    {
        await _viewModel.GoToUserProfile();

    }
}

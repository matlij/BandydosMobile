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

    protected override async void OnAppearing()
    {
        await _viewModel.LoadItems();
    }
}

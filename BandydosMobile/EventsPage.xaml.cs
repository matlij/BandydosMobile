using BandydosMobile.ViewModels;

namespace BandydosMobile;

public partial class EventsPage : ContentPage
{
    private readonly EventsViewModel _viewModel;

    public EventsPage(EventsViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        await _viewModel.LoadItems();
    }
}

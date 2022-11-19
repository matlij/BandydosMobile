using BandydosMobile.ViewModels;

namespace BandydosMobile;

public partial class EventDetailPage : ContentPage
{
    private readonly EventDetailViewModel _vm;

    public EventDetailPage(EventDetailViewModel vm)
    {
        InitializeComponent();

        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        await _vm.LoadItem();
    }
}
using CommunityToolkit.Mvvm.ComponentModel;

namespace BandydosMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    protected bool isBusy;

    [ObservableProperty]
    protected string title = "Bandydsos IK";
}

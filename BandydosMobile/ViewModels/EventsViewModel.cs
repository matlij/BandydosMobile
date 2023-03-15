using BandydosMobile.Models;
using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BandydosMobile.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    [ObservableProperty]
    protected bool isRefreshing;

    [ObservableProperty]
    private Event? selectedItem;

    [ObservableProperty]
    private ObservableCollection<Event> items;

    private readonly IEventDataStore _dataStore;
    private readonly Authenticator _authenticator;

    public EventsViewModel(IEventDataStore dataStore, Authenticator authenticator)
    {
        Title = "Aktiviteter";
        Items = new();
        _dataStore = dataStore;
        _authenticator = authenticator;
    }

    [RelayCommand]
    public async Task LoadItems()
    {
        IsRefreshing = true;

        try
        {
            var user = await _authenticator.GetLoggedInUserAsync();
            if (user == null)
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
                return;
            }

            Items.Clear();
            var items = await _dataStore.GetAsync(DateTime.Now);
            foreach (var @event in items.OrderBy(i => i.Date))
            {
                @event.LoggedInUser = @event.Users.FirstOrDefault(u => u.UserId == user.Id);
                Items.Add(@event);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hämta events misslycakdes", ex.Message);
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task ItemTapped(Event @event)
    {
        await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?ItemId={@event.Id}");
    }

    [RelayCommand]
    public async Task GoToUserProfile()
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private static async Task DisplayAlert(string title, string message)
    {
        var page = Application.Current?.MainPage;
        if (page != null)
        {
            await page.DisplayAlert(title, message, "Stäng");
        }
    }
}

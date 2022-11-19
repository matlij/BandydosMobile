using BandydosMobile.Models;
using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BandydosMobile.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    [ObservableProperty]
    private Event? selectedItem;

    [ObservableProperty]
    private ObservableCollection<Event> items;

    private readonly IEventDataStore _dataStore;

    public EventsViewModel(IEventDataStore dataStore)
    {
        Title = "Aktiviteter";
        Items = new ObservableCollection<Event>();
        _dataStore = dataStore;
    }

    public async Task LoadItems()
    {
        IsBusy = true;

        try
        {
            Items.Clear();
            var items = await _dataStore.GetAsync();
            foreach (var @event in items.OrderBy(i => i.Date))
            {
                @event.CurrentUserIsAttending = UserIsAttendingEvent(@event, Guid.NewGuid()); //TODO
                Items.Add(@event);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hämta events misslycakdes", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void OnAppearing()
    {
        IsBusy = true;
        SelectedItem = null;
    }


    [RelayCommand]
    async Task ItemTapped(Event @event)
    {
        await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?ItemId={@event.Id}");
    }

    private static bool UserIsAttendingEvent(Event @event, Guid userId)
    {
        var user = @event.Users.FirstOrDefault(u => u.UserId == userId);

        return user != null && user.IsAttending;
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

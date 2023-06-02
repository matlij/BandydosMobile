using BandydosMobile.Models;
using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System.Collections.ObjectModel;

namespace BandydosMobile.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    [ObservableProperty]
    protected bool isRefreshing;

    [ObservableProperty]
    private Event? selectedItem;

    [ObservableProperty]
    private ObservableCollection<Event> items = new();

    private readonly IEventDataStore _dataStore;
    private readonly Authenticator _authenticator;
    private readonly INotificationService _notificationService;

    public EventsViewModel(IEventDataStore dataStore, Authenticator authenticator, INotificationService notificationService)
    {
        Title = "Aktiviteter";
        Items = new();
        _dataStore = dataStore;
        _authenticator = authenticator;
        _notificationService = notificationService;
        _notificationService.NotificationActionTapped += OnNotificationActionTapped;

    }

    public void OnAppearing()
    {
        // From docs: Manually setting the IsRefreshing property to true will trigger the refresh visualization, and will execute the ICommand defined by the Command property.
        IsRefreshing = true;
    }

    [RelayCommand]
    public async Task LoadItems()
    {
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

            await ScheduleNotifications(items, user);
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

    private async void OnNotificationActionTapped(NotificationActionEventArgs e)
    {
        if (e.IsTapped)
        {
            await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?ItemId={e.Request.ReturningData}");
        }
    }

    private async Task ScheduleNotifications(IEnumerable<Event> events, User user)
    {
        var eventsWhereUserIsNotAttending = events.Where(e => 
            e?.Users.SingleOrDefault(u => u.UserId == user.Id)?.IsAttending ?? false);

        foreach (var item in eventsWhereUserIsNotAttending)
        {
            await ScheduleNotification(item);
        }

        // ScheduleNotificationForNextEvent
        //var nextEvent = events.OrderBy(e => e.Date).FirstOrDefault();
        //var eventUser = nextEvent?.Users.SingleOrDefault(u => u.UserId == user.Id);
        //if (nextEvent != null && eventUser != null && !eventUser.IsAttending)
        //{
        //    await ScheduleNotification(nextEvent);
        //}
    }

    private async Task ScheduleNotification(Event @event)
    {
        if (await _notificationService.AreNotificationsEnabled() == false)
        {
            await _notificationService.RequestNotificationPermission();
        }

        var notification = new NotificationRequest
        {
            NotificationId = BitConverter.ToInt32(@event.Id.ToByteArray()),
            Title = "Snart dags för innebandy",
            Description = "Kom ihåg att svara på nästa träning!",
            ReturningData = @event.Id.ToString(), // Returning data when tapped on notification.
            Schedule = new NotificationRequestSchedule()
            {
                NotifyTime = @event.DateLocalTime.AddHours(-5).DateTime // Send notification 5 hours before event
            }
        };
        await _notificationService.Show(notification);
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

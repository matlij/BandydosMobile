using System.Collections.ObjectModel;

namespace BandydosMobile.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DateLocalTime => Date.ToLocalTime();
        public string Time => $"{DateLocalTime.TimeOfDay:hh\\:mm}";
        public string DateString => $"{DateLocalTime.Date.ToLongDateString()}";
        public string DaysLeft { get => $"Dagar kvar: {(DateLocalTime - DateTime.Now).Days}"; }
        public EventType? EventType { get; set; }
        public Address? Address { get; set; }
        public ObservableCollection<EventUser> Users { get; set; } = new();
        public string UsersAttending { get => $"{Users.Count(u => u.IsAttending)} / {Users.Count}"; }

        public EventUser? LoggedInUser { get; set; }
    }

    public class Address
    {
        public Guid Id { get; set; }
        public string FullAddress { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}

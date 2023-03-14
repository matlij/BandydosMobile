using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BandydosMobile.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Time => $"{Date.TimeOfDay:hh\\:mm}";
        public string DateString => $"{Date.Date.ToLongDateString()}";
        public string DaysLeft { get => $"Dagar kvar: {(Date - DateTime.Now).Days}"; }
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

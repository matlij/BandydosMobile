using System;

namespace BandydosMobile.Models
{
    public class EventUser : IEquatable<EventUser>
    {
        public EventUser(string userId)
        {
            UserId = userId;
        }

        public EventUser(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserId = user.Id;
            Name = user.Name;
        }

        public string UserId { get; }
        public string Name { get; set; } = string.Empty;

        public bool IsAttending => UserReply == EventReply.Attending;

        public bool IsOwner { get; set; }
        public EventReply UserReply { get; set; }

        public string UserReplyString 
        { 
            get
            {
                return UserReply switch
                {
                    EventReply.NotReplied => "Ej svarat",
                    EventReply.Attending => "På",
                    EventReply.NotAttending => "Av",
                    _ => "Oklart",
                };
            }
        }


        public bool Equals(EventUser other)
        {
            if (other == null)
                return false;

            return other.UserId == UserId;
        }
    }
}

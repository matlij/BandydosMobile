using System;

namespace BandydosMobile.Models
{
    public class TaskAddEventUser
    {
        public TaskAddEventUser(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public bool Invited { get; set; }
    }
}

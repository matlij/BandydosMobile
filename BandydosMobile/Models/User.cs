namespace BandydosMobile.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;

        public string GetName()
        {
            return string.IsNullOrEmpty(FriendlyName)
                ? Name
                : FriendlyName;
        }
    }
}

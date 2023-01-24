namespace RunGroupWebApp.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}

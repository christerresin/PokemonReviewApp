using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public Address? Address { get; set; }
        //public ICollection<Club>? Clubs { get; set; }
        //public ICollection<Club>? Races { get; set; }
        public string? ProfileImageUrl { get; set; } = string.Empty;
    }
}

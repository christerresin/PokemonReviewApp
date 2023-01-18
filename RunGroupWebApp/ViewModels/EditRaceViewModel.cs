using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.ViewModels
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string? URL { get; set; }

    }
}

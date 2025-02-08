using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class RaceViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public IFormFile Image { get; set; }
    }
}

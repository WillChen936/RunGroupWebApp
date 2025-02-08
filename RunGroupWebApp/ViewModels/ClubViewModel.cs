using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class ClubViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public IFormFile Image { get; set; }
    }
}

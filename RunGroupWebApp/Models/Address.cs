using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int Street { get; set; }
        public int City { get; set; }
        public int State { get; set; }
    }
}

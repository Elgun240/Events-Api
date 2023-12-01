using System.ComponentModel.DataAnnotations;

namespace Project_5.ViewModel
{
    public class CreateTicketVM
    {
        [Required]
        public int? MeasureId { get; set; }
        [Required]
        public double Price { get; set; }
    }
}

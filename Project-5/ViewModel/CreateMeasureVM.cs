using System.Reflection.Metadata.Ecma335;

namespace Project_5.ViewModel
{
    public class CreateMeasureVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public DateTime DateTime { get; set; }
        public int? CategoryId { get; set; }
        public int TicketCount { get; set; }
        public int? CompanyId { get; set; }
        public string Accessibility { get; set; }
    }
}

namespace Project_5.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Measure? Measure { get; set; }
        public int? MeasureId { get; set; }
        public double Price { get; set; }
        public AppUser AppUser { get; set; }
        public string? AppUserId { get; set; }
    }
}

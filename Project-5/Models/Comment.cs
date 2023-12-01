namespace Project_5.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public Measure Measure { get; set; }
        public int MeasureId { get; set; }
    }
}

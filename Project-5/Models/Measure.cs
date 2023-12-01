namespace Project_5.Models
{
    public class Measure
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public DateTime DateTime { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<AppUser> Users { get; set; }
        public int TicketCount { get; set; }
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
        public List<Comment> Comments { get; set; }
        public string Accessibility { get; set; }
        public string Status { get; set; }
        public string URL { get; set; }
        public bool IsDeactive { get; set; }


        public static string GenerateUniqueUrl()
        {
            Guid uniqueId = Guid.NewGuid();
            string uniqueUrl = uniqueId.ToString("N"); 

           
            return uniqueUrl;
        }
    }
    
}

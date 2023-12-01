namespace Project_5.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool  IsDeactive { get; set; }
        public List<Measure> Measures { get; set; } 

    }
}

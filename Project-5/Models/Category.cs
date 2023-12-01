namespace Project_5.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<Measure> Measures { get; set; }
       
    }
}

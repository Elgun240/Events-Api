using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project_5.Models
{
    public class AppUser:IdentityUser
    {
      
        public string Firstname { get; set; }
        public string Lastname { get; set; }        
        public double Balance { get; set; }   
        [Required, DataType(DataType.Password)]
        public string Phone { get; set; }
        public string Adress { get; set; }
        public bool  IsDeactive { get; set; }
       
        public List<Ticket> Tickets { get; set; }
        public int? CompanyId { get; set; }
        public List<Measure> Measures { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

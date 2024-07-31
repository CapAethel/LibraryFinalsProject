using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryFinalsProject.Models
{
    public class User 
    {
        [Key]
        public int UserId {  get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        

    }
}

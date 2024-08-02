using LibraryFinalsProject.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryFinalsProject.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int UserId {  get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public Role Role { get; set; }  
        public int RoleId { get; set; }

    }
}

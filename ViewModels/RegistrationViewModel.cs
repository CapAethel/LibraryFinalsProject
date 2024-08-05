using System.ComponentModel.DataAnnotations;

namespace LibraryFinalsProject.ViewModels
{
    public class RegistrationViewModel
    {
        public int UserId { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public int RoleId { get; set; }
    }
}

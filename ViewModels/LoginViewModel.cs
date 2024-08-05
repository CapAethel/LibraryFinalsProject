using System.ComponentModel.DataAnnotations;

namespace LibraryFinalsProject.ViewModels
{
    public class LoginViewModel
    {
        public String Name { get; set; }
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

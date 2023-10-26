using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Authentication_CRUD.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [DisplayName("User Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [DisplayName("User Age")]
        public string Age { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
    }
}

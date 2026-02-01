using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Models
{
    public class UserModel
    {
        public int? UserID {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

    }
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
    public class UserDropDown
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}

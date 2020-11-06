using System.ComponentModel.DataAnnotations;

namespace UltraManufacturing.Models.ViewModels
{
    public class UserManagementCreate : UserManagementUpdatePassword
    {
        [Required, StringLength(50, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }
    }

    public class UserManagementUpdatePassword
    {

        // receives password
        [Required, StringLength(15, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // receives password confirmation
        [Required, StringLength(15, MinimumLength = 6)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

}



using System.ComponentModel.DataAnnotations;

public class chang_p_model
{

    [Required(ErrorMessage = "current password is required")]
    
    public string oldpass { get; set; }

    [Required(ErrorMessage = "Field is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
       ErrorMessage = "Password must be 8-15 characters long, include at least one uppercase letter, one number, and one special character.")]
    public string newpass { get; set; }
    // [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    [Required(ErrorMessage = "Field is required.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
       ErrorMessage = "Password must be 8-15 characters long, include at least one uppercase letter, one number, and one special character.")]
    public string confirmpass { get; set; }
}
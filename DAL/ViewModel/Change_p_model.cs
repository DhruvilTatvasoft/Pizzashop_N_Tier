

using System.ComponentModel.DataAnnotations;

public class chang_p_model
{

    [Required(ErrorMessage = "current password is required")]
    public string oldpass { get; set; }

    [Required(ErrorMessage = "Field is required.")]
    public string newpass { get; set; }
    // [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
    [Required(ErrorMessage = "Field is required.")]
    public string confirmpass { get; set; }
}
using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress(ErrorMessage ="Invalid email address")]
    public string username{
        get;
        set;
    }
    [Required(ErrorMessage ="password is required")]
    public string password{
        get;
        set;
    }   
    public bool IsChecked{
        get;
        set;
    }
}
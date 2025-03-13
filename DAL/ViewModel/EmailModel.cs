using System.ComponentModel.DataAnnotations;


public class EmailModel
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress(ErrorMessage ="Invalid email address")]
    public string Email{
        set;
        get;
    }
}

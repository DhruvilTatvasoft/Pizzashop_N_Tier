

using System.ComponentModel.DataAnnotations;
using System.Dynamic;

public class PasswordModel
{
   [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
       ErrorMessage = "Password must be 8-15 characters long, include at least one uppercase letter, one number, and one special character.")]
   public string newpass
   {
      get;
      set;
   }
   [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
       ErrorMessage = "Password must be 8-15 characters long, include at least one uppercase letter, one number, and one special character.")]
   [Required(ErrorMessage = "password is required")]
   public string confirmpass
   {
      get;
      set;
   }
   public string email
   {
      set;
      get;
   }

}



using System.ComponentModel.DataAnnotations;
using System.Dynamic;

public class PasswordModel
{

   [Required(ErrorMessage ="password is required")]
   public string newpass{
    get;
    set;
   }
   [Required(ErrorMessage ="password is required")]
   public string confirmpass{
    get;
    set;
   }
   public string email{
    set;
    get;
   }

}

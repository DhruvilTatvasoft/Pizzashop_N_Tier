using System.ComponentModel.DataAnnotations;

public class MenuModel{
    public List<Menu>? menuList{
        get;
        set;
    }

   [Required] 
   public Menu? m{
    get;
    set;
   }

}
public class Menu{
   public int categoryId{
        get;
        set;
    }
    [Required(ErrorMessage = "Category name is required.")]
       public string? categoryName{
        get;
        set;
    }
    [Required(ErrorMessage = "Category Description is required.")]
   public string? description{
        get;
        set;
    }
}
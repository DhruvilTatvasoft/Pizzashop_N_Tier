public class MenuModel{
    public List<Menu> menuList{
        get;
        set;
    }
   public Menu m{
    get;
    set;
   }
}
public class Menu{
   public int categoryId{
        get;
        set;
    }
   public string categoryName{
        get;
        set;
    }
   public string description{
        get;
        set;
    }
}
public class userpagingdetailmodel{
    public List<users> users{
        get;
        set;
    }
    public int CurrentPageIndex  { get; set; }
    
    public int PageCount { get; set; }

    public searcheduser searcheduser{
        get;
        set;
    }
    public int totalusers{
        get;
        set;
    }
}

public class searcheduser{
    public String fname{
        get;
        set;
    }
    public String lname{
        get;
        set;
    }
    public String email{
        get;
        set;
    }
    public String phone{
        get;
        set;
    }
    public String role{
        get;
        set;
    }
}
public class users{
    public int userid{
        get;
        set;
    }
    public String Firstname{
        get;
        set;
    }
    public String Lastname{
        get;
        set;
    }
    public String Email{
        get;
        set;
    }
    public String phone{
        get;
        set;
    }
    public String role{
        get;
        set;
    }
}

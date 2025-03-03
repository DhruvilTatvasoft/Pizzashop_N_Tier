public class userpagingdetailmodel{
    public List<users> users{
        get;
        set;
    }
    public int CurrentPageIndex  { get; set; }
    
    public int PageCount { get; set; }

    public int totalusers{
        get;
        set;
    }

    public void loadUsers()
    {
        throw new NotImplementedException();
    }
}


public class users{
    public string name{
        get;
        set;
    }
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
    public bool IsActive{
        get;
        set;
    }
}

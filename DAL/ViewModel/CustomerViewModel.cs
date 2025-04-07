using DAL.Data;

public class CustomerViewModel
{
    public string customerName{get;set;}
    public string customerEmail{get;set;}
    public string phoneNumber{get;set;}

    public decimal max_order{get;set;}

    public decimal avg_order{get;set;}

    public DateTime? comingAt{get;set;}

    public int totalVisits{get;set;}

    public List<OrderDetailModel>? orderDetails{get;set;}
    public DateTime orderPlacedDate{get;set;}
    public int totalOrders{get;set;}

    public int pageSize{get;set;}
    public int pageNumber{get;set;}
    public int totalCustomers{get;set;}
    public string sortBy{get;set;} = "asc";
    public string sortOrder{get;set;} = "name";

    public string filterBy{get;set;} = "All Time";

    public List<Customer>? customers{get;set;}
}

public class OrderDetailModel{
    public DateTime orderDate{get;set;}
    public string orderType{get;set;}
    public string paymentStatus{get;set;}
    public int noOfItems{get;set;}
    public decimal amount{get;set;}
    
}
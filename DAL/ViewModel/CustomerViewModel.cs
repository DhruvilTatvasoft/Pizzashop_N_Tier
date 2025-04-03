using DAL.Data;

public class CustomerViewModel
{
    public string customerName{get;set;}
    public string customerEmail{get;set;}
    public string phoneNumber{get;set;}
    public DateTime orderPlacedDate{get;set;}
    public int totalOrders{get;set;}

    public int pageSize{get;set;}
    public int pageNumber{get;set;}
    public int totalCustomers{get;set;}
    public string sortBy{get;set;} = "asc";
    public string sortOrder{get;set;} = "name";

    public List<Customer>? customers{get;set;}
}
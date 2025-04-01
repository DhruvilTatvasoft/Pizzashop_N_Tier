using DAL.Data;

public class OrderViewModel
{
    public List<Order>? orders{get;set;}
    public Order? order{get;set;}

    public List<Orderstatus>? status{get;set;}

    public string? statusname{get;set;}

    public orderItemModifierViewModel orderedItemModifiers{get;set;}

      public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalOrders { get; set; }

    public string sortOrder{get;set;} = "asc";
    public string sortBy{get;set;} = "orderid";



}
using DAL.Data;

public class OrderViewModel
{
    public List<Order>? orders{get;set;}
    public Order? order{get;set;}

    public List<Orderstatus>? status{get;set;}

    public string? statusname{get;set;}

    public orderItemModifierViewModel orderedItemModifiers{get;set;}

}
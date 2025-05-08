using DAL.Data;

public class DashboardViewModel{
    public float totalsales{get;set;}
    public int totalorders{get;set;}
    public float averageOrderValue{get;set;}
    public List<sellingItemDetail> topSellingItem{get;set;}

    public List<sellingItemDetail> leastSellingItem{get;set;}
    public int waitingListCount{get;set;}
    public Dictionary<string,double> dailySales{get;set;}
    public Dictionary<string,int> totalCustomers{get;set;}
    public int noOfCustomers{get;set;}

    public int timeid {get;set;}
}
public class sellingItemDetail{
    public Item item{get;set;}
    public int quantity{get;set;}
    public int OrderCount{get;set;}
    public int TotalQuantity{get;set;}
}
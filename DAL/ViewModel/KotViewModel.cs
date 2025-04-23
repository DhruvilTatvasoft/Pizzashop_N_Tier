using DAL.Data;

public class KotViewModel
{
    public List<Category> categories { get; set; } = new List<Category>();
    // public orderItemModifierViewModel orderedItemModifiers { get; set; } = new orderItemModifierViewModel();

    public Dictionary<Order, List<Dictionary<Item, List<Modifier>>>> orderDetails{get;set;}

    public Dictionary<int,tableAndsection> orderTableSectionDetail{get;set;}

    public string categoryName{get;set;}

    public int? categoryid{get;set;}

    public SingleOrderDetailModel singleOrderDetail{get;set;}
    public int pageSize{get;set;}
    public int pageNumber{get;set;}

    public int totalOrders{get;set;}


}
public class tableAndsection{
    public string sectionName{get;set;}
    public string tableName{get;set;}
}

public class SingleOrderDetailModel{
    public int orderid{get;set;}
    public Dictionary<Item,List<Modifier>> itemAndModifiers{get;set;}

    public Dictionary<int,int> readyItemCount{get;set;}
}
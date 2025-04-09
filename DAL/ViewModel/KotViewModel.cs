using DAL.Data;

public class KotViewModel
{
    public List<Category> categories { get; set; } = new List<Category>();
    // public orderItemModifierViewModel orderedItemModifiers { get; set; } = new orderItemModifierViewModel();

    public Dictionary<int,orderItemModifierViewModel> orderDetails{get;set;}

    public Dictionary<int,tableAndsection> orderTableSectionDetail{get;set;}

    public string categoryName{get;set;}

    public int? categoryid{get;set;}

}
public class tableAndsection{
    public string sectionName{get;set;}
    public string tableName{get;set;}
}

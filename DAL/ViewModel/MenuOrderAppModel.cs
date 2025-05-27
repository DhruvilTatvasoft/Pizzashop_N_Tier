using System.Net;
using DAL.Data;

public class MenuOrderAppModel
{
    public List<Category> categories { get; set; }  = new List<Category>();
    public List<Item> items { get; set; }   = new List<Item>();

    public CustomerModel customer { get; set; } = new CustomerModel();
    public int orderid { get; set; } = 0;

    public Order order
    {
        get; set;
    } = new Order();
    public int readyQuantity { get; set; }
    public string orderComment { get; set; } = string.Empty;

    public string? itemtype { get; set; } = "";

    public int itemQuantity { get; set; }

    public string itemcomment { get; set; } = string.Empty;
    public List<Taxesandfee> taxesandfees { get; set; } = new List<Taxesandfee>();
    public bool isTableAssigned { get; set; } = false;

    public int tokenid { get; set; }
    public int tableid { get; set; }
    public List<ModifierModel> modifiersForItem { get; set; } = new List<ModifierModel>();

    public List<Modifier> modifiers { get; set; } = new List<Modifier>();
    public Item item { get; set; }  = new Item();

    public List<string> uniqueids { get; set; } = new List<string>();

    public string uniqueId { get; set; }  = string.Empty;

    public List<Table> tables { get; set; } = new List<Table>();

    public int categoryId { set; get; } = 0;

    public OrderDetailsViewModel orderDetailModel { get; set; } = new OrderDetailsViewModel();

    public List<appliedTaxDetails>? appliedTax { get; set; }
}

public class orderDetailsForAssignedTable
{
    public string? itemType { get; set; }
    public List<int>? TableIds { get; set; }
    public int? CategoryId { get; set; }
    public string SearchedItem { get; set; } = string.Empty;
    public int? orderid { get; set; }
    public int? customerid { get; set; }
    public bool? favorites { get; set; }
}

public class customerOrderDetailsViewModel
{
    public string customername { get; set; } = string.Empty;
    public int sectionid { get; set; }
    public string sectionname { get; set; } = string.Empty;
    public int customerid { get; set; }
    public string email { get; set; } = string.Empty;
    public string Phonenumber { get; set; } = string.Empty;
    public int personcount { get; set; }
    public string paymentmethod { get; set; } = string.Empty;
}



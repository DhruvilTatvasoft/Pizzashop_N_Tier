using DAL.Data;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TextTemplating;

public class OrderDetailsViewModel
{
    public List<int> tableids { get; set; }
    public List<Table> tables{get;set;}
    public int customerid { get; set; }
    public CustomerModel customerModel{get;set;}
    public List<ItemDetail> itemDetails { get; set; }  
    public float totalamount { get; set; }
    public string ordercomment{get;set;}
    public int totalPersons{get;set;}

    public string PaymentMethod{get;set;}
    public int sectionid{get;set;}
    public List<appliedTaxDetails> appliedTaxes{get;set;}
    public int? orderid{get;set;} = 0; 

    public List<string>? uniqueids {get;set;}
}

public class ItemDetail
{
    public string? itemId { get; set; }
    public Item? item{get;set;}
    public List<string>? modifierIds { get; set; }
    public List<Modifier>? modifiers{get;set;}
    public string? quantity { get; set; }
    public string? itemcomment{get;set;}

    public string? uniqueid{get;set;}
    public int? orderid{get;set;} 

    public List<string>? uniqueids{get;set;}
}

public class appliedTaxDetails{
    public string taxname{get;set;}
    public string taxtype{get;set;}
    public float taxPercentage{get;set;}
}

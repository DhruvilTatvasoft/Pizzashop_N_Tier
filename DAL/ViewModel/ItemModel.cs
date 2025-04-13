using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DAL.Data;
using Microsoft.AspNetCore.Http;


public class ItemModel
{
    public List<Item>? items
    {
        get;
        set;
    }
    public Item i
    {
        get;
        set;
    }

    public int modifierGroupId{
        get;
        set;
    }
    public List<int>? ids{get;set;}

    public List<ModifierModel>? ModifierModels
    {
        get;
        set;
    }

    public string? payload{
        get;
        set;
    }
    public List<string>? modifierGroupIds
    {
        set;
        get;
    }

    public Modifiergroup mg
    {
        get;
        set;
    }
    public int categoryId
    {
        get;
        set;
    }
    public List<Category>? categories
    {
        get;
        set;
    }
    public List<Unit>? units
    {
        get;

        set;
    }

    public List<Modifier>? modifiers
    {
        get;
        set;
    }
    public List<Modifiergroup>? modifiergroups
    {
        get;
        set;
    }

    public int pageSize{get;set;}
    public int pageNumber{get;set;}

    public int? totalrecords{get;set;}

    public string searchItemName
    {
        get;
        set;
    }
    public int itemId
    {
        get;
        set;
    }
    public List<int> ModifierIds
    {
        get;
        set;
    }
    public Modifier modifier{
        get;
        set;
    }

    public ItemViewModel IModel{
        get;set;
    }
}
public class ItemViewModel{

    public int? itemid{get;set;}

    [Required(ErrorMessage = "Item Name is required")]
    [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "please Use only letters")]
    public string Itemname{get;set;}

    [Required(ErrorMessage = "Item rate is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Item rate cannot be less than 0")]
    public int Itemrate{get;set;}

    public bool Itemtype{get;set;}

    [Required(ErrorMessage = "Item Quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Item Quantity cannot be less than 0")]
    public int Itemquantity{get;set;}

    public int Unitid{get;set;}

    public int Categoryid{get;set;}
    public bool Isavailable{get;set;}

    public bool Isdefaulttax{get;set;}
    [Range(0, 100, ErrorMessage = "Tax percentage cannot be less than 0 or greater than 100")]
    public int Taxpercentage{get;set;}

    public string? Shortcode{get;set;}

    public string? Description{get;set;}

    public IFormFile? ImagePath{get;set;}

    public string? ItemImagePathString{get;set;}

    public List<Category>? categories{get;set;}

    public List<Unit>? units{get;set;}

    public List<Modifiergroup> modifiergroups{get;set;}

     public List<ModifierModel>? ModifierModels
    {
        get;
        set;
    }

        public int? pageSize{get;set;}
    public int? pageNumber{get;set;}

    public int? totalrecords{get;set;}

    public string? payload{
        get;
        set;
    }
    public Modifiergroup? modifiergroup{
        get;
        set;
    }
    public List<Modifier> modifiers{
        get;set;
    }

}

public class ModifierModel{

    public List<Modifiergroup> modifiergroups{get;set;}
    public int Modifiergroupid{get;set;}

    public string Modifiername{get;set;}

    public Modifiergroup modifiergroup{get;set;}
    public int Modifierquantity{get;set;}
    public int Modifierrate{get;set;}
    public List<Unit> units{get;set;}
    public string Description{get;set;}
    public string payload{get;set;}

    public List<int> ModifierIds{get;set;}
    public Modifier modifier{get;set;}

}
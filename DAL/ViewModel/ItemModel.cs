using DAL.Data;

public class ItemModel
{
    public List<Item>? items
    {
        get;
        set;
    }
    public Item? i
    {
        get;
        set;
    }

    public List<int> ids{get;set;}

    public List<ModifierModel> ModifierModels
    {
        get;
        set;
    }
    public List<string> modifierGroupIds
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
    public string? searchItemName
    {
        get;
        set;
    }
    public int? itemId
    {
        get;
        set;
    }
}
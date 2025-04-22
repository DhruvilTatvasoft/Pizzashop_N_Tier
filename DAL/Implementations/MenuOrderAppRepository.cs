using DAL.Data;
using Microsoft.IdentityModel.Tokens;

public class MenuOrderAppRepository : IMenuOrderAppRepository
{
    private readonly PizzashopCContext _context;

    public MenuOrderAppRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public Item getItem(int itemid)
    {
       return _context.Items.FirstOrDefault(item => item.Itemid == itemid)!;
    }

    public List<Item> getItemsForcategory(int categoryid,string searchedItem)
    {
        if(searchedItem != "" ){
            if(categoryid != 0){
            return _context.Items.Where(item=>item.Itemname.ToLower().Trim().Contains(searchedItem.ToLower().Trim()) && item.Isdeleted == false && item.Categoryid == categoryid).ToList();
            }
            else{
            return _context.Items.Where(item=>item.Itemname.ToLower().Trim().Contains(searchedItem.ToLower().Trim()) && item.Isdeleted == false).ToList();
            }
        }
        if (categoryid == 0)
        {
            List<Category> categories = _context.Categories.Where(category=>category.Isdeleted == false).ToList();
            List<Item>  items = new List<Item>();
            foreach (var category in categories)
            {
                var itemsInCategory = _context.Items.Where(item => item.Categoryid == category.Categoryid && item.Isdeleted == false).ToList();
                items.AddRange(itemsInCategory);
            }
            return items;
        }
        else
        {
            return _context.Items.Where(item => item.Categoryid == categoryid && item.Isdeleted == false).ToList();
        }
    }

    public List<ModifierModel> getModifiersForItem(int itemid)
    {
        List<ModifierModel> model = new List<ModifierModel>();
        List<Itemsandmodifier> itemsandmodifiers = _context.Itemsandmodifiers.Where(x => x.Itemid == itemid && x.Isdeleted == false).ToList();
        foreach(var itemmodifier in itemsandmodifiers){
            ModifierModel modifierModel = new ModifierModel();
            modifierModel.modifiergroup = _context.Modifiergroups.FirstOrDefault(mg=>mg.Modifiergroupid == itemmodifier.Modifiergroupid && mg.Isdeleted == false);
            if(modifierModel.modifiergroup  != null){
            List<Modifier> modifiers = _context.Modifiers.Where(modifier=>modifier.Modifiergroupid == itemmodifier.Modifiergroupid && modifier.Isdeleted == false).ToList();
            if(modifiers.Count == 0){
                modifierModel.modifiers = new List<Modifier>();
            }
            else{
            modifierModel.modifiers = modifiers;
            }
            modifierModel.min_value = itemmodifier.Requiredminselection;
            modifierModel.max_value = itemmodifier.Allowedmaxselection;
            model.Add(modifierModel);
            }
        }
        return model;
    }
}
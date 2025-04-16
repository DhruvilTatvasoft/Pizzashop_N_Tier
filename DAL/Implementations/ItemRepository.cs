using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;

public class ItemRepository : IItemRepository
{
    public PizzashopCContext _context;

    public ItemRepository(PizzashopCContext context)
    {
        _context = context;
    }
    public List<Item> getItemsForCategory(int categoryId,int pageSize,int pageNumber)
    {
        var query = _context.Items.Where(i => i.Categoryid == categoryId && i.Isdeleted == false)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();
         List<Item> itemList = query.ToList();
        return itemList;
    }
    public bool addNewCategory(string categoryName, string categoryDescription, string createdBy)
    {
        Category? cexist = _context.Categories.Where(c => c.Categoryname.ToLower().Trim() == categoryName.ToLower().Trim()).FirstOrDefault();
        if (cexist == null || cexist.Isdeleted == true)
        {
            Category category = new Category();
            category.Categoryname = categoryName;
            category.Categoryid = _context.Categories.Count() + 1;
            category.Createdat = DateTime.Now;
            category.Description = categoryDescription;
            category.Modifiedat = DateTime.Now;
            category.Createdby = (int)(_context.Logins.FirstOrDefault(lg => lg.Email == createdBy)?.Id ?? 1);
            _context.Categories.Add(category);
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Category> getAllCategories()
    {
        List<Category> categories = _context.Categories.Where(c => c.Isdeleted == false).ToList();
        return categories;
    }

    public void EditCategory(Menu m, string email)
    {
        Category c = _context.Categories.FirstOrDefault(c => c.Categoryid == m.categoryId) ?? new Category();
        c.Categoryname = m.categoryName!;
        c.Description = m.description;
        _context.Update(c);
        _context.SaveChanges();
    }

    public void deleteCategory(int categoryId)
    {
        Category c = _context.Categories.FirstOrDefault(c => c.Categoryid == categoryId)!;
        c.Isdeleted = true;
        _context.Update(c);
        _context.SaveChanges();
    }

    public List<Unit> getAllUnits()
    {
        List<Unit> units = _context.Units.Where(u => u.Isdeleted == false).ToList();
        return units;
    }

    public bool addItemInDb(ItemViewModel itemViewModel , string email,string? imagePath)
    {   
        if(_context.Items.FirstOrDefault(item=>item.Itemname == itemViewModel.Itemname && item.Categoryid == itemViewModel.Categoryid) != null){
            return false;
        }
        else{
        Item i = new Item();
        i.Itemname = itemViewModel.Itemname;
        i.Categoryid = itemViewModel.Categoryid;
        i.Itemtype = itemViewModel.Itemtype;
        i.Itemrate = itemViewModel.Itemrate;
        i.Itemquantity = itemViewModel.Itemquantity;
        i.Isdefaulttax = itemViewModel.Isdefaulttax;
        i.Createdby = (int)(_context.Logins.FirstOrDefault(lg => lg.Email == email)?.Id ?? 1);
        i.Taxpercentage = itemViewModel.Taxpercentage;
        i.Shortcode = itemViewModel.Shortcode;
        i.Unitid = itemViewModel.Unitid;
        if(imagePath!=null){
        i.Itemimage = imagePath;
        }
        else{
            i.Itemimage = "/uploads/dining-menu.png";
        }
        i.Createdat = DateTime.Now;
        i.Modifiedat = DateTime.Now;
        i.Modifiedby = (int)(_context.Logins.FirstOrDefault(lg => lg.Email == email)?.Id ?? 1);
        _context.Items.Add(i);
        _context.SaveChanges();

        return true;
        }

    }

    public void deleteItemFromDb(int itemId)
    {
        Item i = _context.Items.FirstOrDefault(i => i.Itemid == itemId)!;
        i.Isdeleted = true;
        _context.Update(i);
        _context.SaveChanges();
    }

    public List<Item> getSearchedItemFromDb(string searchedItem, int categoryId)
    {
        List<Item> items = _context.Items.Where(i => i.Categoryid == categoryId && i.Itemname.ToLower().Contains(searchedItem.ToLower().Trim()) && i.Isdeleted == false).ToList();
        return items;
    }

    bool IItemRepository.deleteItemFromDb(int itemId)
    {
        Item? i = _context.Items.Find(itemId);
        if (i != null)
        {
            i.Isdeleted = true;
            _context.Update(i);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public List<Modifiergroup> getAllModifierGroups()
    {
        List<Modifiergroup> modifiergroups = _context.Modifiergroups.Where(mg=>mg.Isdeleted == false).ToList();
        return modifiergroups;
    }

    public Item getItem(int itemid)
    {
        return _context.Items.FirstOrDefault(i => i.Itemid == itemid && i.Isdeleted == false) ?? new Item();
    }

    public int getItemFromItemName(string itemname)
    {
       return _context.Items.FirstOrDefault(i => i.Itemname == itemname && i.Isdeleted == false).Itemid;
    }

    public int getAllItemsForCategory(int categoryId)
    {
        return _context.Items.Where(item=>item.Categoryid == categoryId && item.Isdeleted == false).Count();
    }

    public Item getItemFromItemId(int itemId)
    {
        Item item = _context.Items.FirstOrDefault(item=>item.Itemid == itemId && item.Isdeleted == false)! ;
        return item;
    }

    public void updateItemdetails(ItemViewModel model,string imagepath)
    {
        // Item isItemExist = _context.Items.FirstOrDefault(item=>item.Itemname == model.Itemname && item.Itemid == model.itemid && item.Categoryid != model.Categoryid && item.Isdeleted == false);

        Item item = _context.Items.FirstOrDefault(item=>item.Itemid == model.itemid)!;
        item.Itemname = model.Itemname;
        item.Categoryid = model.Categoryid;
        item.Itemtype = model.Itemtype;
        item.Itemrate = model.Itemrate;
        item.Itemquantity = model.Itemquantity;
        item.Isdefaulttax = model.Isdefaulttax;
        item.Isavailable = model.Isavailable;
        item.Taxpercentage = model.Taxpercentage;
        item.Shortcode = model.Shortcode;
        item.Description = model.Description;
        item.Unitid = model.Unitid;
        if(imagepath != null || imagepath != ""){
        item.Itemimage = imagepath;
        }
        item.Isdeleted = false;
        item.Createdat = DateTime.Now;
        item.Modifiedat = DateTime.Now;
        item.Modifiedby = 1;
        item.Createdby = 1;
        _context.Items.Update(item);
        _context.SaveChanges();

    }

    public Category getCategoryById(int categoryId)
    {
        return _context.Categories.FirstOrDefault(category=>category.Categoryid == categoryId)!;
    }
}
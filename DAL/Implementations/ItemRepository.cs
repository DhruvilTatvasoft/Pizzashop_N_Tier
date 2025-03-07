
using DAL.Data;
using Microsoft.EntityFrameworkCore;

public class ItemRepository : IItemRepository
{
     public PizzashopCContext _context;
    public ItemRepository(PizzashopCContext context){
        _context = context;
    }

    public List<Item> getItemsForCategory(int categoryId)
    {
       List<Item> itemList = _context.Items.Where(i=>i.Categoryid == categoryId && i.Isdeleted == false).ToList();
       return itemList;
    }

      public void addNewCategory(string categoryName, string categoryDescription,string createdBy)
    {
        Category cexist = _context.Categories.Where(c => c.Categoryname == categoryName).FirstOrDefault();
        if(cexist == null || cexist.Isdeleted == true){
        Category category = new Category();
        category.Categoryname = categoryName;
        category.Categoryid = _context.Categories.Count()+1;
        category.Createdat = DateTime.Now;
        category.Description = categoryDescription;
        category.Modifiedat = DateTime.Now;
        category.Createdby = _context.Logins.FirstOrDefault(lg=>lg.Email == createdBy).Id;
        _context.Categories.Add(category);
        _context.SaveChanges();
        }
        else{
            throw new Exception("Category already exists.");
        }
    }

    public List<Category> getAllCategories()
    {
        List<Category> categories = _context.Categories.Where(c=>c.Isdeleted == false).ToList();
        return categories;
    }

    public void EditCategory(Menu m, string email)
    {
        Category c = _context.Categories.FirstOrDefault(c=>c.Categoryid == m.categoryId);
        c.Categoryname = m.categoryName;
        c.Description = m.description;
        _context.Update(c);
        _context.SaveChanges();
    }

    public void deleteCategory(int categoryId)
    {
        Category c = _context.Categories.FirstOrDefault(c=>c.Categoryid == categoryId);
        c.Isdeleted = true;
        _context.Update(c);
        _context.SaveChanges();
    }

    public List<Unit> getAllUnits()
    {
       List<Unit> units = _context.Units.Where(u=>u.Isdeleted == false).ToList();
       return units;
    }

    public void addItemInDb(Item i,string email)
    {
        i.Itemid = _context.Items.Count()+1;
        _context.Items.Add(i);
        _context.SaveChanges();
    }

    public void deleteItemFromDb(int itemId)
    {
        Item i = _context.Items.FirstOrDefault(i=>i.Itemid == itemId);
        i.Isdeleted = true;
        _context.Update(i);
        _context.SaveChanges();
    }

    public List<Item> getSearchedItemFromDb(string searchedItem,int categoryId)
    {
        // x.name.ToLower().Contains(search.ToLower())
        List<Item> items = _context.Items.Where(i =>i.Categoryid == categoryId && i.Itemname.ToLower().Contains(searchedItem)).ToList();
        return items;
    }

    bool IItemRepository.deleteItemFromDb(int itemId)
    {
        Item? i = _context.Items.Find(itemId);
        if(i != null){
        i.Isdeleted = true;
        _context.Update(i);
        _context.SaveChanges();
        return true;
        }
        return false;
    }

    public List<Modifiergroup> getAllModifierGroups()
    {
        List<Modifiergroup> modifiergroups = _context.Modifiergroups.ToList();
        return modifiergroups;
    }
}
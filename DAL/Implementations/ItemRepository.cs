
using DAL.Data;

public class ItemRepository : IItemRepository
{
     public PizzashopCContext _context;
    public ItemRepository(PizzashopCContext context){
        _context = context;
    }

    public List<Item> getItemsForCategory(int categoryId)
    {
       List<Item> itemList = _context.Items.Where(i=>i.Categoryid == categoryId).ToList();
       return itemList;
    }

      public void addNewCategory(string categoryName, string categoryDescription,string createdBy)
    {
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

    public List<Category> getAllCategories()
    {
        List<Category> categories = _context.Categories.ToList();
        return categories;
    }
}
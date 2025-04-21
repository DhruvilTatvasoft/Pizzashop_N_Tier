using DAL.Data;

public class MenuOrderAppRepository : IMenuOrderAppRepository
{
    private readonly PizzashopCContext _context;

    public MenuOrderAppRepository(PizzashopCContext context)
    {
        _context = context;
    }
    public List<Item> getItemsForcategory(int categoryid)
    {
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
}
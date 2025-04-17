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
            return _context.Items.Where(item => item.Isdeleted == false).ToList();
        }
        else
        {
            return _context.Items.Where(item => item.Categoryid == categoryid && item.Isdeleted == false).ToList();
        }
    }
}
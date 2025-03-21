using DAL.Data;

public class TableRepository : ITableRepository
{
    public PizzashopCContext _context;

    public TableRepository(PizzashopCContext context){
        _context = context;
    }
    public List<Table> getTablesForSection(int sectionid)
    {
       return _context.Tables.Where(t => t.Sectionid == sectionid && t.Isdeleted == false).ToList();
    }
}
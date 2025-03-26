using DAL.Data;

public class TableRepository : ITableRepository
{
    public PizzashopCContext _context;

    public TableRepository(PizzashopCContext context){
        _context = context;
    }

    public bool addNewTable(Table table)
    {
        Table newTable = new Table();
        newTable.Tablename = table.Tablename;
        newTable.Sectionid = table.Sectionid;
        newTable.Capacity = table.Capacity;
        newTable.Status = table.Status;
        newTable.Createdat = DateTime.Now;
        newTable.Modifiedat = DateTime.Now;
        newTable.Createdby = 1;
        newTable.Modifiedby = 1;
        newTable.Isdeleted = false;
        try{
        _context.Tables.Add(newTable);
        _context.SaveChanges();
        return true;
        }
        catch (Exception ex){
            Console.WriteLine(ex.Message);
            return false;
        }

    }

    public void deleteTable(int tableId)
    {
        Table table = _context.Tables.Find(tableId)??new Table();
        if(table != null){
            table.Isdeleted = true;
            _context.Tables.Update(table);
            _context.SaveChanges();
        }
    }
    public Table gettablebyid(int tableid)
    {
       return _context.Tables.FirstOrDefault(table=>table.Tableid == tableid && table.Isdeleted == false)!;
    }

    public List<Table> getTablesForSection(int sectionid)
    {
       return _context.Tables.Where(t => t.Sectionid == sectionid && t.Isdeleted == false).ToList();
    }

    public bool updateTable(Table table)
    {
        Table tableToUpdate = _context.Tables.FirstOrDefault(t=>t.Tableid == table.Tableid && t.Isdeleted == false)!;
        tableToUpdate.Tablename = table.Tablename;
        tableToUpdate.Sectionid = table.Sectionid;
        tableToUpdate.Capacity = table.Capacity;
        tableToUpdate.Modifiedat = DateTime.Now;
        tableToUpdate.Modifiedby = 1;
        tableToUpdate.Status = table.Status;
        _context.Tables.Update(tableToUpdate);
        _context.SaveChanges();
        return true;
    }
}
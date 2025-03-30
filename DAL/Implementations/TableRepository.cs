using DAL.Data;

public class TableRepository : ITableRepository
{
    public PizzashopCContext _context;

    public TableRepository(PizzashopCContext context){
        _context = context;
    }

    public bool addNewTable(Table table)
    {
        if(_context.Tables.FirstOrDefault(t=>t.Tablename.ToLower().Trim() == table.Tablename.ToLower().Trim() && t.Sectionid == table.Sectionid && t.Isdeleted == false) != null){
            return false;
        }   
        
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

    public int getAllTables()
    {
        return _context.Tables.Where(table=>table.Isdeleted == false).ToList().Count;
    }

    public Table gettablebyid(int tableid)
    {
       return _context.Tables.FirstOrDefault(table=>table.Tableid == tableid && table.Isdeleted == false)!;
    }

    public List<Table> getTablesForSection(int sectionId, int pageNumber, int pageSize)
{
    if (pageSize <= 0)
    {
        pageSize = 1; // Prevent division by zero
    }

    int totalTables = _context.Tables.Count(t => t.Sectionid == sectionId && t.Isdeleted == false);

    if (totalTables == 0)
    {
        return new List<Table>(); // Return empty if no tables found
    }

    int totalPages = (int)Math.Ceiling((double)totalTables / pageSize);

    if (pageNumber > totalPages)
    {
        pageNumber = totalPages; // Ensure pageNumber is within bounds
    }

    return _context.Tables
                   .Where(t => t.Sectionid == sectionId && t.Isdeleted == false)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
}

    public bool isOccupied(int tableid)
    {
    Table table =    _context.Tables.FirstOrDefault(table=>table.Tableid == tableid);
       if(!_context.Tables.FirstOrDefault(table=>table.Tableid == tableid)!.Status){
        return true;
       }
       else{
        return false;
       }
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
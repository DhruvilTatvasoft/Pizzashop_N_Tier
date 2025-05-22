

using DAL.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class TableRepository : ITableRepository
{
    public PizzashopCContext _context;
    private readonly IConfiguration _configuration;

    public TableRepository(PizzashopCContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
    }
    public bool addNewTable(Table table)
    {
        if (_context.Tables.FirstOrDefault(t => t.Tablename.ToLower().Trim() == table.Tablename.ToLower().Trim() && t.Sectionid == table.Sectionid && t.Isdeleted == false) != null)
        {
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
        newTable.Statusname = "Available";
        newTable.Isdeleted = false;
        try
        {
            _context.Tables.Add(newTable);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }

    public void deleteTable(int tableId)
    {
        Table table = _context.Tables.Find(tableId) ?? new Table();
        if (table != null)
        {
            table.Isdeleted = true;
            _context.Tables.Update(table);
            _context.SaveChanges();
        }
    }

    public int getAllTables()
    {
        return _context.Tables.Where(table => table.Isdeleted == false).ToList().Count;
    }



    public Table gettablebyid(int tableid)
    {
        return _context.Tables.FirstOrDefault(table => table.Tableid == tableid && table.Isdeleted == false)!;
    }

    public TableAndSectionViewModel getTablesForSection(int sectionId, int pageNumber, int pageSize, string? searchedTable)
    {
        IQueryable<Table> query;
        if (!string.IsNullOrWhiteSpace(searchedTable))
        {
            query = _context.Tables
                .Where(t => t.Sectionid == sectionId
                            && t.Isdeleted == false
                            && t.Tablename.ToLower().Trim().Contains(searchedTable.ToLower().Trim()));
        }
        else
        {
            query = _context.Tables
                          .Where(t => t.Sectionid == sectionId && t.Isdeleted == false);
        }
        TableAndSectionViewModel model = new TableAndSectionViewModel();
        model.sectionId = sectionId;
        model.TotalTables = query.ToList().Count();
        model.PageSize = pageSize;
        model.PageNumber = pageNumber;
        model.tables = query.Skip((pageNumber - 1) * pageSize)
                       .Take(pageSize)
                       .ToList();

        return model;
    }

    public bool isOccupied(int tableid)
    {
        Table table = _context.Tables.FirstOrDefault(table => table.Tableid == tableid);
        if (!_context.Tables.FirstOrDefault(table => table.Tableid == tableid)!.Status)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool updateTable(Table table)
    {
        Table tableToUpdate = _context.Tables.FirstOrDefault(t => t.Tableid == table.Tableid && t.Isdeleted == false)!;
        tableToUpdate.Tablename = table.Tablename;
        tableToUpdate.Sectionid = table.Sectionid;
        tableToUpdate.Capacity = table.Capacity;
        tableToUpdate.Modifiedat = DateTime.Now;
        tableToUpdate.Modifiedby = 1;
        tableToUpdate.Status = table.Status;
        if (tableToUpdate.Status)
        {
            tableToUpdate.Statusname = "Available";
        }
        else
        {
            tableToUpdate.Statusname = "Assigned";
        }

        _context.Tables.Update(tableToUpdate);
        _context.SaveChanges();
        return true;
    }

    public TableViewModel GetAllTablesAndSections()
    {
        Dictionary<SectionViewModel, List<Table>> sectionWiseTables = new Dictionary<SectionViewModel, List<Table>>();
        const string query1 = "select * from get_section_table_counts()";
        const string query2 = "select * from get_sectionwise_tables(@sectionid)";
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            var result = connection.Query<dynamic>(query1).ToList();
            foreach (var row in result)
            {
                SectionViewModel section = new SectionViewModel();
                Section s = new Section();
                s.Sectionname = row.sectionname;
                s.Sectionid = row.sectionid;
                section.section = s;
                section.AssignedTablesCount = (int)row.assigned_tables_count;
                section.AvailableTablesCount = (int)row.available_tables_count;
                section.RunningTablesCount = (int)row.running_tables_count;
                var tables = connection.Query<dynamic>(query2, new { sectionid = s.Sectionid }).ToList();
                List<Table> tablesinsection = new List<Table>();
                foreach (var table in tables)
                {
                    Table table1 = new Table();
                    table1.Tableid = table.tableid;
                    table1.Tablename = table.tablename;
                    table1.Capacity = table.capacity;
                    table1.Status = table.status;
                    table1.Statusname = table.statusname;
                    table1.Sectionid = table.sectionid;
                    table1.Createdat = table.createddate;
                    tablesinsection.Add(table1);
                }
                sectionWiseTables.Add(section, tablesinsection);
            }
        }



        // List<Section> sections = _context.Sections.Where(section => section.Isdeleted == false).OrderBy(section => section.Sectionid).ToList();
        // foreach (var section in sections)
        // {
        //     List<Table> tablesForSection = _context.Tables.Where(t => t.Sectionid == section.Sectionid && t.Isdeleted == false).ToList();
        //     SectionViewModel sectionModel = new SectionViewModel();
        //     sectionModel.section = section;
        //     sectionModel.AssignedTablesCount = _context.Tables.Where(table => table.Statusname != null && table.Statusname.ToLower().Trim() == "assigned" && table.Isdeleted == false && table.Sectionid == section.Sectionid).ToList().Count();
        //     sectionModel.AvailableTablesCount = _context.Tables.Where(table => table.Statusname != null && table.Statusname.ToLower().Trim() == "available" && table.Isdeleted == false && table.Sectionid == section.Sectionid).ToList().Count();
        //     sectionModel.RunningTablesCount = _context.Tables.Where(table => table.Statusname != null && table.Statusname.ToLower().Trim() == "running" && table.Isdeleted == false && table.Sectionid == section.Sectionid).ToList().Count();
        //     sectionWiseTables.Add(sectionModel, tablesForSection);
        // }
        TableViewModel model = new TableViewModel();
        model.tablesPerSection = sectionWiseTables;
        return model;
    }

}
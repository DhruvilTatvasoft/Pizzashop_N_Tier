using DAL.Data;

public class TableService : ITableService
{
     private readonly ITableRepository _tableRepository;

    public TableService(ITableRepository tableRepository){
        _tableRepository = tableRepository;
    }

    public bool addNewTable(Table table)
    {
        return _tableRepository.addNewTable(table);
    }


    public void deleteTables(List<int> selectedTables)
    {
        foreach(int tableId in selectedTables){
            _tableRepository.deleteTable(tableId);
        }
    }

    public int getAllTables()
    {
        return _tableRepository.getAllTables();
    }

    public TableViewModel GetAllTablesAndSections()
    {
        return _tableRepository.GetAllTablesAndSections();
    }


    public Table gettablebyid(int tableid)
    {
        return _tableRepository.gettablebyid(tableid);
    }

    public TableAndSectionViewModel getTablesForsection(int sectionid,int pageNumber, int pageSize,string? searchedTable)
    {
        return _tableRepository.getTablesForSection(sectionid,pageNumber,pageSize,searchedTable);
    }

    public bool isOccupied(int tableid)
    {
        return _tableRepository.isOccupied(tableid);
    }

    public bool updateTable(Table table)
    {
        return _tableRepository.updateTable(table);
    }
}
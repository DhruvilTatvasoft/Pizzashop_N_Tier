using DAL.Data;

public class TableImpl : ITableService
{
     private readonly ITableRepository _tableRepository;

    public TableImpl(ITableRepository tableRepository){
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

    public Table gettablebyid(int tableid)
    {
        return _tableRepository.gettablebyid(tableid);
    }

    public List<Table> getTablesForsection(int sectionid)
    {
        return _tableRepository.getTablesForSection(sectionid);
    }

    public bool updateTable(Table table)
    {
        return _tableRepository.updateTable(table);
    }
}
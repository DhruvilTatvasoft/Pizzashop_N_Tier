using DAL.Data;

public class TableImpl : ITableService
{
     private readonly ITableRepository _tableRepository;

    public TableImpl(ITableRepository tableRepository){
        _tableRepository = tableRepository;
    }

    public List<Table> getTablesForsection(int sectionid)
    {
        return _tableRepository.getTablesForSection(sectionid);
    }
   
}
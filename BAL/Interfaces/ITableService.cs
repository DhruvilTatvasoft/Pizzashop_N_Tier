using DAL.Data;

public interface ITableService{
    bool addNewTable(Table table);
    void deleteTables(List<int> selectedTables);
    int getAllTables();
    TableViewModel GetAllTablesAndSections();
    Table gettablebyid(int tableid);
    TableAndSectionViewModel getTablesForsection(int sectionId,int pageNumber, int pageSize,string? searchedTable);
    bool isOccupied(int tableid);
    bool updateTable(Table table);
}
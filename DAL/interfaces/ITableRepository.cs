using DAL.Data;

public interface ITableRepository{
    bool addNewTable(Table table);
    void assignTable(List<int> tableids,int customerid);
    void deleteTable(int tableId);
    int getAllTables();
    TableViewModel GetAllTablesAndSections();
    Table gettablebyid(int tableid);
    TableAndSectionViewModel getTablesForSection(int sectionid,int pageNumber, int pageSize,string? searchedTable);
    bool isOccupied(int tableid);
    bool updateTable(Table table);
}
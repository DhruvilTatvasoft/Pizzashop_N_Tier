using DAL.Data;

public interface ITableRepository{
    bool addNewTable(Table table);
    void deleteTable(int tableId);
    int getAllTables();
    TableViewModel GetAllTablesAndSections();
    Table gettablebyid(int tableid);
    TableAndSectionViewModel getTablesForSection(int sectionid,int pageNumber, int pageSize);
    bool isOccupied(int tableid);
    bool updateTable(Table table);
}
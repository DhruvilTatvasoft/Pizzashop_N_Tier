using DAL.Data;

public interface ITableRepository{
    bool addNewTable(Table table);
    void deleteTable(int tableId);
    int getAllTables();
    Table gettablebyid(int tableid);
    List<Table> getTablesForSection(int sectionid,int pageNumber, int pageSize);
    bool isOccupied(int tableid);
    bool updateTable(Table table);
}
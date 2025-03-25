using DAL.Data;

public interface ITableRepository{
    bool addNewTable(Table table);
    void deleteTable(int tableId);
    Table gettablebyid(int tableid);
    List<Table> getTablesForSection(int sectionid);
    bool updateTable(Table table);
}
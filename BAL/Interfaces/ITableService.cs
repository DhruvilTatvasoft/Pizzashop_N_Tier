using DAL.Data;

public interface ITableService{
    bool addNewTable(Table table);
    void deleteTables(List<int> selectedTables);
    Table gettablebyid(int tableid);
    List<Table> getTablesForsection(int sectionId);
    bool updateTable(Table table);
}
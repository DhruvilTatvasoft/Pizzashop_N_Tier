using DAL.Data;

public interface ITableService{
    bool addNewTable(Table table);
    void deleteTables(List<int> selectedTables);
    int getAllTables();
    Table gettablebyid(int tableid);
    List<Table> getTablesForsection(int sectionId,int pageNumber, int pageSize);
    bool isOccupied(int tableid);
    bool updateTable(Table table);
}
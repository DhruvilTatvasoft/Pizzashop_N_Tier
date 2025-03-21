using DAL.Data;

public interface ITableService{
   List<Table> getTablesForsection(int sectionId);
}
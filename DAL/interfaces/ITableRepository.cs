using DAL.Data;

public interface ITableRepository{
    List<Table> getTablesForSection(int sectionid);
}
using Azure.Core.Pipeline;
using DAL.Data;

public class TableViewModel
{
    public Dictionary<SectionViewModel,List<Table>>? tablesPerSection{get;set;}
}
public class SectionViewModel{
    public Section section{get;set;}
    public int AssignedTablesCount{get;set;}
    public int RunningTablesCount{get;set;}
    public int AvailableTablesCount{get;set;}
}
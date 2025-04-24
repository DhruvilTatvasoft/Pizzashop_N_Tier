using Azure.Core.Pipeline;
using DAL.Data;

public class TableViewModel
{
    public Dictionary<SectionViewModel,List<Table>>? tablesPerSection{get;set;}
    public List<CustomerModel> customers{get;set;}

    public WaitingTokenModel WaitingToken{get;set;}

    public List<int> tables{get;set;}
}
public class SectionViewModel{
    public Section section{get;set;}
    public int AssignedTablesCount{get;set;}
    public int RunningTablesCount{get;set;}
    public int AvailableTablesCount{get;set;}
}

public class assignTableDetails{
    public int tokenid{get;set;}
    public List<int> tableids{get;set;}
}
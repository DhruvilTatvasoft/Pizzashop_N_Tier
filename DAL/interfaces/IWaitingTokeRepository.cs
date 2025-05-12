using DAL.Data;

public interface IWaitingTokenRepository
{
    bool addNewWaitingToken(WaitingTokenModel model);
    bool assignTable(List<int> tableid, int tokenid);
    bool deleteWaitingToken(int tokenid);
    WaitingTokenModel getAllWaingTokens(int sectionid);
    CustomerModel getCustomerForWaitingToken(int tokenid, List<int> tableid);
    List<CustomerModel> getCustomerTokensForSection(int sectionid,List<int> tableid);
    int? getMaxPersonCountForSection(List<int> tableids);
    int getSectionIdOfToken(int tokenid);
    Dictionary<Section,int> getSectionsWithWaitingTokens();
    List<CustomerModel> getSuggestedCustomerList(string name);
    List<Table> getTablesForToken(int tokenid);
    WaitingTokenModel getTokenDetails(int waitingTokenId);
    int getTokenidFromCustomerEmail(string email);
    int getTotalWaitingTokens();
    bool updateWaitingToken(WaitingTokenModel model);
}
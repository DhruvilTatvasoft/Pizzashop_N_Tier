using DAL.Data;

public interface IWaitingTokenRepository
{
    bool addNewWaitingToken(WaitingTokenModel model);
    bool assignTable(int tableid, int tokenid);
    bool deleteWaitingToken(int tokenid);
    WaitingTokenModel getAllWaingTokens(int sectionid);
    List<CustomerModel> getCustomerTokensForSection(int sectionid);
    int getSectionIdOfToken(int tokenid);
    Dictionary<Section,int> getSectionsWithWaitingTokens();
    List<CustomerModel> getSuggestedCustomerList(string name);
    List<Table> getTablesForToken(int tokenid);
    WaitingTokenModel getTokenDetails(int waitingTokenId);
    int getTotalWaitingTokens();
    bool updateWaitingToken(WaitingTokenModel model);
}
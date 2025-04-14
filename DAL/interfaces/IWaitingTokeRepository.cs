using DAL.Data;

public interface IWaitingTokenRepository
{
    bool addNewWaitingToken(WaitingTokenModel model);
    WaitingTokenModel getAllWaingTokens(int sectionid);
    Dictionary<Section,int> getSectionsWithWaitingTokens();
    WaitingTokenModel getTokenDetails(int waitingTokenId);
    int getTotalWaitingTokens();
    bool updateWaitingToken(WaitingTokenModel model);
}
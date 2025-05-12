using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface IWaitingTokenService
    {
        bool AddNewWaitingToken(WaitingTokenModel model);
        bool AssignTable(int tableid, int tokenid);
        bool deleteWaitingToken(int tokenid);
        WaitingTokenModel getAllWaitingTokens(int sectionid);
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
        bool UpdateWaitingToken(WaitingTokenModel model);
    }
}
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
        WaitingTokenModel getAllWaitingTokens(int sectionid);
        Dictionary<Section,int> getSectionsWithWaitingTokens();
        WaitingTokenModel getTokenDetails(int waitingTokenId);
        int getTotalWaitingTokens();
        bool UpdateWaitingToken(WaitingTokenModel model);
    }
}
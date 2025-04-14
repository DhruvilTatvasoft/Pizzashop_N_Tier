using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Data;

namespace BAL.Implementations
{
    public class WaitingTokenImpl : IWaitingTokenService
    {
        IWaitingTokenRepository _waitingTokenRepository;
        public WaitingTokenImpl(IWaitingTokenRepository waitingTokenRepository){
            _waitingTokenRepository = waitingTokenRepository;
        }

        public bool AddNewWaitingToken(WaitingTokenModel model)
        {
            return _waitingTokenRepository.addNewWaitingToken(model);
        }

        public WaitingTokenModel getAllWaitingTokens(int sectionid)
        {
            return _waitingTokenRepository.getAllWaingTokens(sectionid);
        }

        public Dictionary<Section,int> getSectionsWithWaitingTokens()
        {
            return _waitingTokenRepository.getSectionsWithWaitingTokens();
        }

        public WaitingTokenModel getTokenDetails(int waitingTokenId)
        {
            return _waitingTokenRepository.getTokenDetails(waitingTokenId);
        }

        public int getTotalWaitingTokens()
        {
            return _waitingTokenRepository.getTotalWaitingTokens();
        }

        public bool UpdateWaitingToken(WaitingTokenModel model)
        {
            return _waitingTokenRepository.updateWaitingToken(model);
        }
    }
}
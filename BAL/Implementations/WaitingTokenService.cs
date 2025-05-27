using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Data;

namespace BAL.Implementations
{
    public class WaitingTokenService : IWaitingTokenService
    {
        IWaitingTokenRepository _waitingTokenRepository;
        public WaitingTokenService(IWaitingTokenRepository waitingTokenRepository){
            _waitingTokenRepository = waitingTokenRepository;
        }

        public bool AddNewWaitingToken(WaitingTokenModel model)
        {
            return _waitingTokenRepository.addNewWaitingToken(model);
        }

        public bool AssignTable(List<int> tableid, int tokenid)
        {
            return _waitingTokenRepository.assignTable(tableid, tokenid);
        }

        public Customer createNewCustomer(CustomerModel customerModal)
        {
            return _waitingTokenRepository.createCustomer(customerModal);
        }

        public bool deleteWaitingToken(int tokenid)
        {
           return _waitingTokenRepository.deleteWaitingToken(tokenid);
        }

        public WaitingTokenModel getAllWaitingTokens(int sectionid)
        {
            return _waitingTokenRepository.getAllWaingTokens(sectionid);
        }

        public CustomerModel getCustomerForWaitingToken(int tokenid, List<int> tableid)
        {
            return _waitingTokenRepository.getCustomerForWaitingToken(tokenid, tableid);
        }

        public List<CustomerModel> getCustomerTokensForSection(int sectionid,List<int> tableid)
        {
            return _waitingTokenRepository.getCustomerTokensForSection(sectionid,tableid);
        }

        public int? getMaxPersonCountForSection(List<int> tableids)
        {
           return _waitingTokenRepository.getMaxPersonCountForSection(tableids);
        }

        public int getSectionIdOfToken(int tokenid)
        {
            return _waitingTokenRepository.getSectionIdOfToken(tokenid);
        }

        public Dictionary<Section,int> getSectionsWithWaitingTokens()
        {
            return _waitingTokenRepository.getSectionsWithWaitingTokens();
        }

        public List<CustomerModel> getSuggestedCustomerList(string name)
        {
            return _waitingTokenRepository.getSuggestedCustomerList(name);
        }

        public List<Table> getTablesForToken(int tokenid)
        {
            return _waitingTokenRepository.getTablesForToken(tokenid);
        }

        public WaitingTokenModel getTokenDetails(int waitingTokenId)
        {
            return _waitingTokenRepository.getTokenDetails(waitingTokenId);
        }

        public int getTokenidFromCustomerEmail(string email)
        {
           return _waitingTokenRepository.getTokenidFromCustomerEmail(email);
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
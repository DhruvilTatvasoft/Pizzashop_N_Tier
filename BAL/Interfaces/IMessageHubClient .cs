using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IMessageHubClient 
    {
         Task SendOffersToUser(List < string > message);
    }
}
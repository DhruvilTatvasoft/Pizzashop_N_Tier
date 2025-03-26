using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface IOrderService
    {
        List<Order> getAllOrders();
        List<Order>? getAllOrdersBySearch(string v);

        // List<Order> getAllOrdersByStatus(int statusid);
        List<Order> getAllOrdersByStatus(int status);

        List<Orderstatus> getAllStatus();
    }
}
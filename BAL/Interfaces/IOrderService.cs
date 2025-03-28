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
        List<Order> getAllOrdersBySearch(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Order> getAllOrdersByStatus(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Orderstatus> getAllStatus();
        List<Order> getOrdersByFilters(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Order> getAllOrderByOptionFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Order> getAllOrderByDateFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        void createExcelSheet(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        Order? getOrderDetails(int orderid);
        Dictionary<Item, List<Modifier>> getItemsAndModifiers(int orderid);
    }
}
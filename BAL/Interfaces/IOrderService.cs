using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface IOrderService
    {
        List<Order> getAllOrders(int pageNumber,int pageSize);
        List<Order> getAllOrdersBySearch(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Order> getAllOrdersByStatus(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Orderstatus> getAllStatus();
        OrderViewModel getOrdersByFilters(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate,int pageNumber,int pageSize,string sortOrder,string sortBy);
        List<Order> getAllOrderByOptionFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        List<Order> getAllOrderByDateFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        void createExcelSheet(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        Order? getOrderDetails(int orderid);
        Dictionary<Item, List<Modifier>> getItemsAndModifiers(int orderid);
        int getTotalOrderCount();
        Dictionary<int, orderItemModifierViewModel> getAllOrderByOptionFilterForKot();
        Dictionary<int, tableAndsection> getOrderSectionAndTableDetails(int orderId);
        // Dictionary<int, orderItemModifierViewModel> getOrderDetailsByCategory(int categoryid,bool? IsReady);

        Dictionary<int, List<Dictionary<Item, List<Modifier>>>> GetOrderDetailsByCategory(int categoryid, bool? IsReady);
        SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid);
        void changeReadyQuantity(Dictionary<int, int> readyItemCount);
    }
}
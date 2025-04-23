using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface IOrderService
    {
        List<Orderstatus> getAllStatus();
        OrderViewModel getOrdersByFilters(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate,int pageNumber,int pageSize,string sortOrder,string sortBy);
        void createExcelSheet(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
        Order? getOrderDetails(int orderid);
      
        int getTotalOrderCount();
        Dictionary<int, orderItemModifierViewModel> getAllOrderByOptionFilterForKot();
        tableAndsection getOrderSectionAndTableDetails(int orderId);
        // Dictionary<int, orderItemModifierViewModel> getOrderDetailsByCategory(int categoryid,bool? IsReady);

        Dictionary<Order, List<Dictionary<Item, List<Modifier>>>> GetOrderDetailsByCategory(int categoryid, bool? IsReady);
        SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid);
        void changeReadyQuantity(Dictionary<int, int> readyItemCount);
        Dictionary<int, Dictionary<Item, List<Modifier>>> getModifiersForItems(int orderid);
    }
}
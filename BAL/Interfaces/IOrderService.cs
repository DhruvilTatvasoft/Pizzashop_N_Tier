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

        void GetOrderDetailsByCategory(int categoryid, bool? IsReady,int pageSize,int pageNumber,KotViewModel kotModel);
        SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid,string status);
        void changeReadyQuantity(Dictionary<int, int> readyItemCount);
        Dictionary<int, Dictionary<Item, List<Modifier>>> getModifiersForItems(int orderid);
        int CreateOrder(int tokenid, int tableid);
        void addItemInOrder(int itemid, List<int> modifiers);
        int CreateOrderForCustomer(int tokenid, List<int> tableids);
    }
}
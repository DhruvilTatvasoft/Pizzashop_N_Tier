using DAL.Data;

public interface IOrderRepository{
    List<Orderstatus> getAllStatus();
    OrderViewModel GetAllOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate,int pageNumber,int pageSize,string sortOrder,string sortBy,bool fromExport);
    Order? GetOrderDetails(int orderid);
    int GetTotalOrderCount();
    Dictionary<int, orderItemModifierViewModel> GetOrderByOptionFilterForKot();
    tableAndsection getOrderSectionAndTableDetails(int orderId);
    Dictionary<Order, List<Dictionary<Item, List<Modifier>>>> GetOrderDetailsByCategory(int categoryid, bool? IsReady);
    SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid);
    void changeReadyQuantity(Dictionary<int, int> readyItemCount);

    Dictionary<int,Dictionary<Item,List<Modifier>>> GetItemsAndModifiersForOrder2(int orderid);
}
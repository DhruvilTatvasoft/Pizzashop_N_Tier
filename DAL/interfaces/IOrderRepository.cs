using DAL.Data;

public interface IOrderRepository{
    List<Orderstatus> getAllStatus();
    OrderViewModel GetAllOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate,int pageNumber,int pageSize,string sortOrder,string sortBy,bool fromExport);
    Order? GetOrderDetails(int orderid);
    int GetTotalOrderCount();
    Dictionary<int, orderItemModifierViewModel> GetOrderByOptionFilterForKot();
    tableAndsection getOrderSectionAndTableDetails(int orderId);
    void GetOrderDetailsByCategory(int categoryid, bool? IsReady,int pageSize,int pageNumber,KotViewModel kotModel);
    SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid,string status);
    void changeReadyQuantity(Dictionary<int, int> readyItemCount);

    Dictionary<int,Dictionary<Item,List<Modifier>>> GetItemsAndModifiersForOrder2(int orderid);
    int CreateOrder(int tokenid, int tableid);
    void addItemInOrder(int itemid, List<int> modifiers);
    int CreateOrderForCustomer(int tokenid, List<int> tableids);
}
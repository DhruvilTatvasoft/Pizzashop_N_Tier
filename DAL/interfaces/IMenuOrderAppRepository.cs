using DAL.Data;

public interface IMenuOrderAppRepository
{
    bool cancelTheOrder(ItemDetail itemDetails);
    bool completeTheOrder(ItemDetail itemdetails);
    int createOrder(OrderDetailsViewModel orderDetails);
    MenuOrderAppModel getAssignedTableDetails(int tableid);
    CustomerModel getcustomerDetails(int customerid,List<int>? tableid);

    void getDashBoardDetails( DashboardViewModel model,int timeId,string startdate,string fromDate);
    Item getItem(int itemid);
    List<Item> getItemsForcategory(int categoryid,string searchedItem,string ItemType);
    List<ModifierModel> getModifiersForItem(int itemid);
    void getOrderdItemQuantity(int? orderid, int itemid,MenuOrderAppModel model,List<int> modifiers);
    Order getOrderfromOrderid(int orderId);
    MenuOrderAppModel getRunningTableOrder(int tableid);
    void loadOrderedItemsData(int? orderid, MenuOrderAppModel responseModel);
    void saveCustomerDetails(CustomerModel customer);
    void saveOrderWiseComment(MenuOrderAppModel model);
}
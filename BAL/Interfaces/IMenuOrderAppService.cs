using DAL.Data;

public interface IMenuOrderAppService
{
    bool cancelTheOrder(ItemDetail itemDetails);
    bool completeTheOrder(ItemDetail itemdetails);
    int createOrder(OrderDetailsViewModel orderDetails);
    MenuOrderAppModel getAssignedTableDetails(int tableid);
    CustomerModel getcustomerDetails(int customerid,List<int>? tableid);
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
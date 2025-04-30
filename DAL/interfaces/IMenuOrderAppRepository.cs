using DAL.Data;

public interface IMenuOrderAppRepository
{
    bool createOrder(OrderDetailsViewModel orderDetails);
    CustomerModel getcustomerDetails(int customerid,List<int>? tableid);
    Item getItem(int itemid);
    List<Item> getItemsForcategory(int categoryid,string searchedItem);
    List<ModifierModel> getModifiersForItem(int itemid);
    void getOrderdItemQuantity(int? orderid, int itemid,MenuOrderAppModel model);
    Order getOrderfromOrderid(int orderId);
    MenuOrderAppModel getRunningTableOrder(int tableid);
    void loadOrderedItemsData(int? orderid, MenuOrderAppModel responseModel);
    void saveCustomerDetails(CustomerModel customer);
    void saveOrderWiseComment(MenuOrderAppModel model);
}
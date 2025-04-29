using DAL.Data;

public interface IMenuOrderAppService
{
    bool createOrder(OrderDetailsViewModel orderDetails);
    Item getItem(int itemid);
    List<Item> getItemsForcategory(int categoryid,string searchedItem);
    List<ModifierModel> getModifiersForItem(int itemid);
    Order getOrderfromOrderid(int orderId);
    void saveCustomerDetails(CustomerModel customer);
    void saveOrderWiseComment(MenuOrderAppModel model);
}
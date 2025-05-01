using DAL.Data;

public class MenuOrderAppImple : IMenuOrderAppService
{
    private readonly IMenuOrderAppRepository _menuOrderAppRepository;

    public MenuOrderAppImple(IMenuOrderAppRepository menuOrderAppRepository)
    {
        _menuOrderAppRepository = menuOrderAppRepository;
    }

    public bool createOrder(OrderDetailsViewModel orderDetails)
    {
        return _menuOrderAppRepository.createOrder(orderDetails);
    }

    public CustomerModel getcustomerDetails(int customerid,List<int>? tableid)
    {
        if(tableid != null){

        return _menuOrderAppRepository.getcustomerDetails(customerid,tableid);
        }
        else{
        return _menuOrderAppRepository.getcustomerDetails(customerid,null);
        }
    }

    public Item getItem(int itemid)
    {
        return _menuOrderAppRepository.getItem(itemid);
    }

    public List<Item> getItemsForcategory(int categoryid,string searchedItem="")
    {
        return _menuOrderAppRepository.getItemsForcategory(categoryid,searchedItem);
    }

    public List<ModifierModel> getModifiersForItem(int itemid)
    {
       return _menuOrderAppRepository.getModifiersForItem(itemid);
    }

    public void getOrderdItemQuantity(int? orderid, int itemid,MenuOrderAppModel model,List<int> modifiers)
    {
         _menuOrderAppRepository.getOrderdItemQuantity(orderid,itemid,model,modifiers);
    }

    public Order getOrderfromOrderid(int orderId)
    {
        return _menuOrderAppRepository.getOrderfromOrderid(orderId);
    }

    public MenuOrderAppModel getRunningTableOrder(int tableid)
    {
        return _menuOrderAppRepository.getRunningTableOrder(tableid);
    }

    public void loadOrderedItemsData(int? orderid, MenuOrderAppModel responseModel)
    {
        _menuOrderAppRepository.loadOrderedItemsData(orderid,responseModel);
    }

    public void saveCustomerDetails(CustomerModel customer)
    {
         _menuOrderAppRepository.saveCustomerDetails(customer);
    }

    public void saveOrderWiseComment(MenuOrderAppModel model)
    {
        _menuOrderAppRepository.saveOrderWiseComment(model);
    }
}
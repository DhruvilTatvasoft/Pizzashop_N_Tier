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

    public Order getOrderfromOrderid(int orderId)
    {
        return _menuOrderAppRepository.getOrderfromOrderid(orderId);
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
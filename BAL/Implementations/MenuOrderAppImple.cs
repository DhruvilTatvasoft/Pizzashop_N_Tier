using DAL.Data;

public class MenuOrderAppImple : IMenuOrderAppService
{
    private readonly IMenuOrderAppRepository _menuOrderAppRepository;

    public MenuOrderAppImple(IMenuOrderAppRepository menuOrderAppRepository)
    {
        _menuOrderAppRepository = menuOrderAppRepository;
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

    public void saveCustomerDetails(CustomerModel customer)
    {
         _menuOrderAppRepository.saveCustomerDetails(customer);
    }
}
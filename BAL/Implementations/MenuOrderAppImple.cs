using DAL.Data;

public class MenuOrderAppImple : IMenuOrderAppService
{
    private readonly IMenuOrderAppRepository _menuOrderAppRepository;

    public MenuOrderAppImple(IMenuOrderAppRepository menuOrderAppRepository)
    {
        _menuOrderAppRepository = menuOrderAppRepository;
    }
    public List<Item> getItemsForcategory(int categoryid)
    {
        return _menuOrderAppRepository.getItemsForcategory(categoryid);
    }
}
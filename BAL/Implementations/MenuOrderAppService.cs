
using QRCoder;
using DAL.Data;




public class MenuOrderAppService : IMenuOrderAppService
{
    private readonly IMenuOrderAppRepository _menuOrderAppRepository;

    public MenuOrderAppService(IMenuOrderAppRepository menuOrderAppRepository)
    {
        _menuOrderAppRepository = menuOrderAppRepository;
    }

    public bool cancelTheOrder(ItemDetail itemDetails)
    {
        return _menuOrderAppRepository.cancelTheOrder(itemDetails);
    }

    public bool completeTheOrder(ItemDetail itemdetails)
    {
        return _menuOrderAppRepository.completeTheOrder(itemdetails);
    }

    public int createOrder(OrderDetailsViewModel orderDetails)
    {
        return _menuOrderAppRepository.createOrder(orderDetails);
    }


    public byte[] GenerateQRCode(string text, int width = 250, int height = 250)
    {
        using (var qrGenerator = new QRCodeGenerator())
    {
        var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);
        
        using (var ms = new MemoryStream())
        {
            qrCode.GetGraphic(20).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();  
        }
    }
    }

    public MenuOrderAppModel getAssignedTableDetails(int tableid)
    {
        return _menuOrderAppRepository.getAssignedTableDetails(tableid);
    }

    public CustomerModel getcustomerDetails(int customerid, List<int>? tableid)
    {
        if (tableid != null)
        {

            return _menuOrderAppRepository.getcustomerDetails(customerid, tableid);
        }
        else
        {
            return _menuOrderAppRepository.getcustomerDetails(customerid, null);
        }
    }

    public void getDashBoardDetails(int timeId, string fromDate, string startDate, DashboardViewModel model)
    {
        _menuOrderAppRepository.getDashBoardDetails(model, timeId, startDate, fromDate);
    }

    public Item getItem(int itemid)
    {
        return _menuOrderAppRepository.getItem(itemid);
    }

    public List<Item> getItemsForcategory(int categoryid, string ItemType, string searchedItem = "")
    {
        return _menuOrderAppRepository.getItemsForcategory(categoryid, ItemType, searchedItem);
    }

    public List<ModifierModel> getModifiersForItem(int itemid)
    {
        return _menuOrderAppRepository.getModifiersForItem(itemid);
    }

    public void getOrderdItemQuantity(int? orderid, int itemid, MenuOrderAppModel model, List<int> modifiers)
    {
        _menuOrderAppRepository.getOrderdItemQuantity(orderid, itemid, model, modifiers);
    }


    public MenuOrderAppModel getRunningTableOrder(int tableid)
    {
        return _menuOrderAppRepository.getRunningTableOrder(tableid);
    }

    public void loadOrderedItemsData(int? orderid, MenuOrderAppModel responseModel)
    {
        _menuOrderAppRepository.loadOrderedItemsData(orderid, responseModel);
    }

    public void saveCustomerDetails(CustomerModel customer)
    {
        _menuOrderAppRepository.saveCustomerDetails(customer);
    }

    public void saveCustomerReview(customerReviewViewModel model)
    {
        _menuOrderAppRepository.saveCustomerReview(model);
    }

  

}
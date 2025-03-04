using DAL.Data;

public class ItemsImple : IItemService
{

    public IItemRepository _itemRepository;

    public ItemsImple(IItemRepository itemRepository){
        _itemRepository = itemRepository;
    }
    public void getItemsForcategory(int categoryId,ItemModel model)
    {
        List<Item> itemList = _itemRepository.getItemsForCategory(categoryId);
        model.items = itemList;
    }
}
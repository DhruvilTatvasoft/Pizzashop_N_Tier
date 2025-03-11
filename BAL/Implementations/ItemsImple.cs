using System;
using DAL.Data;

public class ItemsImple : IItemService
{

    public IItemRepository _itemRepository;

    public ItemsImple(IItemRepository itemRepository){
        _itemRepository = itemRepository;
    }

    public void addItem(Item i, string email)
    {
        _itemRepository.addItemInDb(i,email);
    }

    public bool deleteItem(int itemid)
    {
        return _itemRepository.deleteItemFromDb(itemid);
    }

    public void deleteItems(List<int> itemIds)
    {
        try{
            foreach(int itemId in itemIds){
                _itemRepository.deleteItemFromDb(itemId);
                Console.WriteLine("OKKKKK");
            }
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
    }

    public Item getItemFromId(int itemid)
    {
        Item item = _itemRepository.getItem(itemid);
        return item;
    }

    public void getItemsForcategory(int categoryId,ItemModel model)
    {
        List<Item> itemList = _itemRepository.getItemsForCategory(categoryId);
        List<Category> categoryList = _itemRepository.getAllCategories();
        List<Unit> units = _itemRepository.getAllUnits();
        List<Modifiergroup> modifiergroups = _itemRepository.getAllModifierGroups();
        model.categoryId = categoryId;
        model.items = itemList;
        model.categories = categoryList;
        model.units = units;
        model.modifiergroups = modifiergroups;
    }

    public List<Item> getSearchedItem(string searchedItem,ItemModel model,int categoryId)
    {
       List<Item> itemList = _itemRepository.getSearchedItemFromDb(searchedItem.ToLower(),categoryId);
       model.items = itemList;
       List<Unit> unitlist = _itemRepository.getAllUnits();
       List<Category> categoryList = _itemRepository.getAllCategories();
       model.units = unitlist;
       model.categories = categoryList;
       return itemList;
    }
}
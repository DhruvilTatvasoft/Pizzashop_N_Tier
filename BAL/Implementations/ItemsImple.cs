using System;
using DAL.Data;
using DAL.interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ItemsImple : IItemService
{

    public IItemRepository _itemRepository;

     public IImagePath _imagePath;

     public IModifierRepository _modifierRepository;
    public ItemsImple(IItemRepository itemRepository,IImagePath imagePath,IModifierRepository modifierRepository){
        _itemRepository = itemRepository;
        _imagePath = imagePath;
        _modifierRepository = modifierRepository;
    }

    public bool addItem(ItemViewModel itemViewModel, string email)
    {
        if(itemViewModel.ImagePath!= null){
        string imagePath = _imagePath.getImagePath(itemViewModel.ImagePath); 
       return _itemRepository.addItemInDb(itemViewModel,email,imagePath);
        }
        else{
      return  _itemRepository.addItemInDb(itemViewModel, email, null);
        }
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

    public List<Category> getAllCategories()
    {
        return _itemRepository.getAllCategories();
    }

    public List<Modifiergroup> getAllModifierGroups()
    {
        return _itemRepository.getAllModifierGroups();
    }

    public List<Unit> getAllUnits(){
        return _itemRepository.getAllUnits();
    }

    public Item getItemFromId(int itemid)
    {
        Item item = _itemRepository.getItem(itemid);
        return item;
    }

    public int getItemFromItemName(string itemname)
    {
        return _itemRepository.getItemFromItemName(itemname);
    }

    public void getItemsForcategory(int categoryId,ItemModel model,int pageSize,int pageNumber)
    {
        List<Item> itemList = _itemRepository.getItemsForCategory(categoryId,pageSize,pageNumber);
        List<Category> categoryList = _itemRepository.getAllCategories();
        List<Unit> units = _itemRepository.getAllUnits();
        List<Modifiergroup> modifiergroups = _itemRepository.getAllModifierGroups();
        model.categoryId = categoryId;
        model.items = itemList;
        model.categories = categoryList;
        model.units = units;
        model.modifiergroups = modifiergroups;
        model.totalrecords =  _itemRepository.getAllItemsForCategory(categoryId);
        model.pageSize = pageSize;
        model.pageNumber = pageNumber;
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

    public ItemViewModel loadItemModel(ItemViewModel model, int itemId)
    {
        Item item = _itemRepository.getItemFromItemId(itemId);
        model.Itemname = item.Itemname;
        model.Itemrate = (int)item.Itemrate;
        model.Itemtype = item.Itemtype;
        model.Itemquantity = item.Itemquantity;
        model.Isavailable = item.Isavailable;
        model.Categoryid = item.Categoryid;
        model.Isdefaulttax = item.Isdefaulttax;
        model.Taxpercentage = (int)item.Taxpercentage;
        model.Shortcode = item.Shortcode;
        model.Description = item.Description;
        model.ItemImagePathString = item.Itemimage;
        model.categories = _itemRepository.getAllCategories();
        model.units = _itemRepository.getAllUnits();
        model.modifiergroups = _itemRepository.getAllModifierGroups();
        List<ModifierModel> modifierModels = _modifierRepository.getModifiersForItem(itemId);
        model.itemid = itemId;

        foreach(var modifiergroup in modifierModels){
            modifiergroup.mg = _modifierRepository.GetModifiergroup(modifiergroup.ModifiergroupId);
        }
        model.ModifierModels = modifierModels;
        return model;
    }

    public void updateItemdetails(ItemViewModel model, int itemid)
    {
             _itemRepository.updateItemdetails(model);
        _modifierRepository.updateModifiersForItem(model.ModifierModels, model.itemid);
    }
}
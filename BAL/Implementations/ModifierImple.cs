using BAL.Interfaces;
using DAL.Data;
using DAL.interfaces;
using Microsoft.AspNetCore.Routing.Template;

public class ModifierImple : IModifierService
{

    private readonly IModifierRepository _modifierRepository;
    public ModifierImple(IModifierRepository modifierRepository)
    {
        _modifierRepository = modifierRepository;
    }
    public ItemModel getModifiersForMGroup(int modifiergroupId,int pageSize,int pageNumber)
    {
        return _modifierRepository.getModifiersForMG(modifiergroupId,pageSize,pageNumber);
    }
    public Modifiergroup GetModifiergroup(int modifiergroupId)
    {
        return _modifierRepository.GetModifiergroup(modifiergroupId);
    }
    public List<Modifiergroup> getAllModifierGroups()
    {
        return _modifierRepository.getAllModifierGroups();
    }

    public ItemModel getAllModifiers(int? modifierGroupId,int pageSize,int pageNumber)
    {   
        if (modifierGroupId != null || modifierGroupId != 0)
        {
            return _modifierRepository.getAllMOdifiersForModifierGroup(modifierGroupId,pageSize,pageNumber);
        }
        else
        {
            return _modifierRepository.getAllModifiers(pageSize,pageNumber);
        }
    }

    public void addModifiersForItem(List<ModifierModel> modifierModels, int itemid, string email)
    {
        foreach (var modifier in modifierModels)
        {
            _modifierRepository.addModifiersForItem(modifier, itemid, email);
        }
    }

    public List<ModifierModel> getModifiersForItem(int itemid)
    {
        List<ModifierModel> modifierModels = _modifierRepository.getModifiersForItem(itemid);
        return modifierModels;
    }

    public List<Modifier> getSelectedModifiers(List<int> modifierIds)
    {
        return _modifierRepository.getSelectedModifiers(modifierIds);
    }

    public ItemModel getSearchedModifier(string searchedModifier,int pageSize,int pageNumber)
    {
            return _modifierRepository.getSearchedModifier(searchedModifier,pageSize,pageNumber);
    }

    public bool AddNewModifierGroup(Modifiergroup mg, List<int> modifierIds)
    {
        return _modifierRepository.AddNewModifierGroup(mg, modifierIds);
    }

    public void deleteModifier(int modifierid, int modifiergroupid)
    {
        _modifierRepository.deleteModifier(modifierid, modifiergroupid);
    }

    public bool updateModifierGroup(Modifiergroup mg, List<int> modifierIds)
    {
        return _modifierRepository.updateModifierGroup(mg, modifierIds);
    }

    public void deleteModifierGroup(int modifierGroupId)
    {
        _modifierRepository.deleteModifierGroup(modifierGroupId);
    }

    public List<Unit> GetAllUnits()
    {
        return _modifierRepository.GetAllUnits();
    }

    public bool AddNewModifier(ModifierModel modifier)
    {
        return _modifierRepository.AddNewModifier(modifier);
    }

    public Modifier getModifier(int modifierid, int modifierGroupId)
    {
        return _modifierRepository.getModifierFromDb(modifierid);
    }

    public void updateModifier(ModifierModel modifier, int modifierGroupId)
    {
        _modifierRepository.updateModifier(modifier, modifierGroupId);
    }

    public List<Modifier> getModifiersForMGroupForItem(int modifiergroupId)
    {
        return _modifierRepository.getModifiersForMGroupForItem(modifiergroupId);   
    }

    public ItemModel getModifiersForModifierGroup(int modifiergroupid)
    {
        return _modifierRepository.getModifiersForModifierGroup(modifiergroupid);
    }
}

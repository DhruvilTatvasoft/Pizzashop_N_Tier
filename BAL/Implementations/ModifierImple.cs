using BAL.Interfaces;
using DAL.Data;
using DAL.interfaces;
using Microsoft.AspNetCore.Routing.Template;

public class ModifierImple : IModifierService
{

    private readonly IModifierRepository _modifierRepository;
    public ModifierImple(IModifierRepository modifierRepository){
        _modifierRepository = modifierRepository;
    }
    public List<Modifier> getModifiersForMGroup(int modifiergroupId)
    {
        return _modifierRepository.getModifiersForMG(modifiergroupId);
    }
    public Modifiergroup GetModifiergroup(int modifiergroupId){
        return _modifierRepository.GetModifiergroup(modifiergroupId);
    }
    public List<Modifiergroup> getAllModifierGroups()
    {
        return _modifierRepository.getAllModifierGroups();
    }

    public List<Modifier> getAllModifiers()
    {
        return _modifierRepository.getAllModifiers();
    }

    public void addModifiersForItem(List<ModifierModel> modifierModels, int itemid,string email)
    {
       foreach (var modifier in modifierModels){
        _modifierRepository.addModifiersForItem(modifier, itemid,email);
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

    public List<Modifier> getSearchedModifier(string searchedModifier)
    {
       List<Modifier> modifiers = _modifierRepository.getSearchedModifier(searchedModifier);
        return modifiers;
    }

    public void AddNewModifierGroup(Modifiergroup mg, List<int> modifierIds)
    {
        _modifierRepository.AddNewModifierGroup(mg,modifierIds);
    }

    public void deleteModifier(int modifierid, int modifiergroupid)
    {
        _modifierRepository.deleteModifier(modifierid,modifiergroupid);
    }

    public void updateModifierGroup(Modifiergroup mg, List<int> modifierIds)
    {
       _modifierRepository.updateModifierGroup(mg,modifierIds);
    }

    public void deleteModifierGroup(int modifierGroupId)
    {
        _modifierRepository.deleteModifierGroup(modifierGroupId);
    }

    public List<Unit> GetAllUnits()
    {
        return _modifierRepository.GetAllUnits();
    }

    public void AddNewModifier(Modifier modifier)
    {
        _modifierRepository.AddNewModifierGroup(modifier);
    }

    public Modifier getModifier(int modifierid, int modifierGroupId)
    {
        return _modifierRepository.getModifierFromDb(modifierid);
    }

    public void updateModifier(Modifier modifier, int modifierGroupId){
         _modifierRepository.updateModifier(modifier,modifierGroupId);
    }
}

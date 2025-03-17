using BAL.Interfaces;
using DAL.Data;
using DAL.interfaces;

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
}

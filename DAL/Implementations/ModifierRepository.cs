using DAL.Data;
using DAL.interfaces;

public class ModifierRepository : IModifierRepository{
    private readonly PizzashopCContext _context;
    public ModifierRepository(PizzashopCContext context){
        _context = context;
    }
    public List<Modifier>  getModifiersForMG(int ModifierGroupId)
    {
        List<Modifier> modifiers = _context.Modifiers.Where(m=>m.Modifiergroupid == ModifierGroupId).ToList();
        return modifiers;
    }
    public Modifiergroup GetModifiergroup(int ModifierGroupId){
        Modifiergroup m = _context.Modifiergroups.FirstOrDefault(mg=>mg.Modifiergroupid == ModifierGroupId);
        return m;
    }

    public List<Modifiergroup> getAllModifierGroups()
    {
        return _context.Modifiergroups.ToList();
    }

    public List<Modifier> getAllModifiers()
    {
        return _context.Modifiers.ToList();
    }
}
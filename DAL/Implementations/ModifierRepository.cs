using DAL.Data;
using DAL.interfaces;
using Microsoft.EntityFrameworkCore;

public class ModifierRepository : IModifierRepository
{
    private readonly PizzashopCContext _context;
    public ModifierRepository(PizzashopCContext context)
    {
        _context = context;
    }
    public List<Modifier> getModifiersForMG(int ModifierGroupId)
    {
        List<Modifier> modifiers = _context.Modifiers.Where(m => m.Modifiergroupid == ModifierGroupId && m.Isdeleted == false).ToList();
        
        modifiers.ForEach(m =>
        {
            m.Unit = _context.Units.FirstOrDefault(u => u.Unitid == m.Unitid) ?? new Unit();
        });
        return modifiers;
    }
    public Modifiergroup GetModifiergroup(int ModifierGroupId)
    {
        Modifiergroup m = _context.Modifiergroups.FirstOrDefault(mg => mg.Modifiergroupid == ModifierGroupId) ?? new Modifiergroup();
        return m;
    }

    public List<Modifiergroup> getAllModifierGroups()
    {
        return _context.Modifiergroups.ToList();
    }

    public List<Modifier> getAllModifiers()
    {
        List<Modifier> modifiers = _context.Modifiers.Where(modifier=>modifier.Isdeleted == false).ToList();
        modifiers.ForEach(m =>
        {
            m.Unit = _context.Units.FirstOrDefault(u => u.Unitid == m.Unitid) ?? new Unit();
        });
        return modifiers;
    }

    public void addModifiersForItem(ModifierModel modifier, int itemid, string email)
    {
        try
        {
            Itemsandmodifier im = new Itemsandmodifier();
            im.Itemid = itemid;
            im.Modifiergroupid = modifier.ModifiergroupId;
            im.Allowedmaxselection = modifier.max_value;
            im.Requiredminselection = modifier.min_value;
            im.Isdeleted = false;
            im.Createdat = DateTime.Now;
            im.Modifiedat = DateTime.Now;
            int userid = _context.Users.FirstOrDefault(u => u.Email == email)?.Userid ?? 1;
            im.Createdby = userid;
            im.Modifiedby = userid;
            _context.Itemsandmodifiers.Add(im);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public List<ModifierModel> getModifiersForItem(int itemid)
    {
        List<ModifierModel> modifiersMdls = new List<ModifierModel>();
        List<Itemsandmodifier> itemmodifiers = _context.Itemsandmodifiers.Where(im => im.Itemid == itemid).ToList();
        ModifierModel modifierModel = new ModifierModel();
        foreach (var im in itemmodifiers)
        {
            modifierModel.ModifiergroupId = im.Modifiergroupid;
            modifierModel.max_value = im.Allowedmaxselection;
            modifierModel.min_value = im.Requiredminselection;
            modifierModel.modifiers = _context.Modifiers.Where(m => m.Modifiergroupid == im.Modifiergroupid).Distinct().ToList();
            modifierModel.modifiers.ForEach(m =>
            {
                m.Unit = _context.Units.FirstOrDefault(u => u.Unitid == m.Unitid) ?? new Unit();
            });
            modifiersMdls.Add(modifierModel);
        }
        return modifiersMdls;
    }

    public List<Modifier> getSelectedModifiers(List<int> modifierIds)
    {
        List<Modifier> modifiers = new List<Modifier>();
        foreach (var id in modifierIds)
        {
            Modifier m = _context.Modifiers.FirstOrDefault(m => m.Modifierid == id) ?? new Modifier();
            m.Unit = _context.Units.FirstOrDefault(u => u.Unitid == m.Unitid) ?? new Unit();
            if (m != null)
            {
                modifiers.Add(m);
            }
        }

        return modifiers;
    }

    public List<Modifier> getSearchedModifier(string searchedModifier)
    {
        List<Modifier> modifiers = _context.Modifiers.Where(m => m.Modifiername.ToLower().Contains(searchedModifier.ToLower().Trim())).Distinct().ToList();
        modifiers.ForEach(m =>
        {
            m.Unit = _context.Units.FirstOrDefault(u => u.Unitid == m.Unitid) ?? new Unit();
        });
        return modifiers;
    }

    public void AddNewModifierGroup(Modifiergroup mg, List<int> modifierIds)
    {
        Modifiergroup newModifierGroup = new Modifiergroup();
        newModifierGroup.Modifiergroupname = mg.Modifiergroupname;
        newModifierGroup.Description = mg.Description;
        newModifierGroup.Isdeleted = false;
        newModifierGroup.Createdat = DateTime.Now;
        newModifierGroup.Modifiedat = DateTime.Now;
        newModifierGroup.Createdby = 1;
        newModifierGroup.Modifiedby = 1;
        _context.Modifiergroups.Add(newModifierGroup);
        _context.SaveChanges();

int NewModifierGroupId = _context.Modifiergroups.FirstOrDefault(modifierGroup => modifierGroup.Modifiergroupname == mg.Modifiergroupname)?.Modifiergroupid ?? 1;
        if (modifierIds.Count > 0)
        {

            foreach (var id in modifierIds)
            {
                Modifier modifier = _context.Modifiers.FirstOrDefault(m => m.Modifierid == id) ?? new Modifier();
                Modifier newModifier = new Modifier();
                newModifier.Modifiername = modifier.Modifiername;
                newModifier.Description = modifier.Description;
                newModifier.Isdeleted = false;
                newModifier.Modifiergroupid = _context.Modifiergroups.FirstOrDefault(modifierGroup => modifierGroup.Modifiergroupname == mg.Modifiergroupname)?.Modifiergroupid ?? 1;
                newModifier.Modifierrate = modifier.Modifierrate;
                newModifier.Modifierquantity = modifier.Modifierquantity;
                newModifier.Unitid = modifier.Unitid;
                newModifier.Createdat = DateTime.Now;
                newModifier.Modifiedat = DateTime.Now;
                newModifier.Createdby = 1;
                newModifier.Modifiedby = 1;
                newModifier.Modifierid = _context.Modifiers.Count() + 1;
                _context.Modifiers.Add(newModifier);
                _context.SaveChanges();
            }
        }
    }

    public void deleteModifier(int modifierid, int modifiergroupid)
    {
        Modifier modifier = _context.Modifiers.FirstOrDefault(m => m.Modifierid == modifierid && m.Modifiergroupid == modifiergroupid) ?? new Modifier();
        modifier.Isdeleted = true;
        _context.Update(modifier);
        _context.SaveChanges();
    }
}
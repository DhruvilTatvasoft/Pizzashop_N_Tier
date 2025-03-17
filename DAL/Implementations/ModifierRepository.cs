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

    public void addModifiersForItem(ModifierModel modifier, int itemid,string email)
    {
        try{
       Itemsandmodifier im = new Itemsandmodifier();
         im.Itemid = itemid;
        im.Modifiergroupid = modifier.ModifiergroupId;
        im.Allowedmaxselection = modifier.max_value;
        im.Requiredminselection = modifier.min_value;
        im.Isdeleted = false;
        im.Createdat = DateTime.Now;
        im.Modifiedat = DateTime.Now;
        int userid = _context.Users.FirstOrDefault(u=>u.Email == email).Userid;
        im.Createdby = userid;
        im.Modifiedby = userid;
        _context.Itemsandmodifiers.Add(im);
        _context.SaveChanges();
        }
        catch(Exception e){
            throw e;
        }
    }
}
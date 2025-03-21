using DAL.Data;

public class SectionRepository : ISectionRepository
{
     private readonly PizzashopCContext _context;

     public SectionRepository(PizzashopCContext context){
        _context = context;
     }
     public List<Section> getAllSections(){
        return _context.Sections.ToList();
     }

    public List<Table> getTableForSection(int sectionid)
    {
        return _context.Tables.Where(table=>table.Sectionid == sectionid).ToList();
    }

}
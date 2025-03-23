using DAL.Data;

public class SectionRepository : ISectionRepository
{
    private readonly PizzashopCContext _context;

    public SectionRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public bool addNewSection(Section section)
    {
        if (_context.Sections.FirstOrDefault(SectionInDb => SectionInDb.Sectionname.ToLower().Trim() == section.Sectionname.ToLower().Trim()) != null)
        {
            return false;
        }
        else
        {
            Section newSection = new Section();
            newSection.Sectionname = section.Sectionname;
            newSection.Description = section.Description;
            newSection.Modifiedat = DateTime.Now;
            newSection.Createdat = DateTime.Now;
            newSection.Createdby = 1;
            _context.Sections.Add(newSection);
            _context.SaveChanges();
            return true;
        }
    }

    public List<Section> getAllSections()
    {
        return _context.Sections.ToList();
    }

    public Section getAllSections(int sectionId)
    {
        return _context.Sections.FirstOrDefault(section => section.Sectionid == sectionId && section.Isdeleted == false)!;
    }

    public List<Table> getTableForSection(int sectionid)
    {
        return _context.Tables.Where(table => table.Sectionid == sectionid).ToList();
    }

    public bool updateSection(Section section)
    {
        try
        {
            if (_context.Sections.Where(s => s.Sectionname.ToLower().Trim() == section.Sectionname.ToLower().Trim()) == null)
            {
                Section updatedSection = _context.Sections.FirstOrDefault(Section => Section.Sectionid == section.Sectionid && Section.Isdeleted == false)!;
                updatedSection.Sectionname = section.Sectionname;
                updatedSection.Description = section.Description;
                _context.Sections.Update(updatedSection);
                _context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
}
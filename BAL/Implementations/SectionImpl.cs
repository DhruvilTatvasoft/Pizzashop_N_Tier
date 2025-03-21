using DAL.Data;

public class SectionImpl : ISectionService
{
    private readonly ISectionRepository _sectionRepository;

    public SectionImpl(ISectionRepository sectionRepository){
        _sectionRepository = sectionRepository;
    }
    public List<Section> getAllSections()
    {
        return _sectionRepository.getAllSections();
    }
    
}
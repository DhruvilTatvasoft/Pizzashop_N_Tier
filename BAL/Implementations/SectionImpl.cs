using DAL.Data;

public class SectionImpl : ISectionService
{
    private readonly ISectionRepository _sectionRepository;

    public SectionImpl(ISectionRepository sectionRepository){
        _sectionRepository = sectionRepository;
    }

    public bool addNewSection(TableAndSectionViewModel model)
    {
        return _sectionRepository.addNewSection(model.section);
    }

    public Section getSection(int sectionId)
    {
        return _sectionRepository.getAllSections(sectionId);
    }

    public List<Section> getAllSections()
    {
        return _sectionRepository.getAllSections();
    }

    public bool updateSection(TableAndSectionViewModel model)
    {
        return  _sectionRepository.updateSection(model.section);
    }
    public bool deleteSection(int sectionId){
        return _sectionRepository.deleteSection(sectionId);
    }
}
using DAL.Data;

public interface ISectionService{
    bool addNewSection(TableAndSectionViewModel model);
    Section getSection(int sectionId);
    List<Section> getAllSections();
    bool updateSection(TableAndSectionViewModel model);
}
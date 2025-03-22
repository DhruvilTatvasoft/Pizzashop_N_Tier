using DAL.Data;

public interface ISectionService{
    bool addNewSection(TableAndSectionViewModel model);
    List<Section> getAllSections();
}
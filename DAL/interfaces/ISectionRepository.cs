using DAL.Data;

public interface ISectionRepository{
    bool addNewSection(Section section);
    List<Section> getAllSections();
    
} 
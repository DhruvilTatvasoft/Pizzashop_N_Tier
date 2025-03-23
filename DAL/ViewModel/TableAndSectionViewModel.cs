using Azure.Identity;
using DAL.Data;
public class TableAndSectionViewModel{
    public List<Section>? sections{get;set;}

    public List<Table>? tables{get;set;}

    public Section? section{get;set;}

    public int sectionId {get;set;}
}
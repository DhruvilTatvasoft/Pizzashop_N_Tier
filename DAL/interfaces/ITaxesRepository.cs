using DAL.Data;

public interface ITaxesRepository{
    void addNewTax(Taxesandfee tax);
    void deleteTax(int taxid);
    List<Taxesandfee> getAlltaxes();
    List<Taxesandfee> getSearchedTax(string search);
    Taxesandfee getTaxById(int taxid);
}
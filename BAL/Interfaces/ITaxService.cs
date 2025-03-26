using DAL.Data;

public interface ITaxService{
    void addNewTax(Taxesandfee tax);
    void deleteTax(int taxid);
    List<Taxesandfee> getAllTaxes();
    Taxesandfee getTaxById(int taxid);
    List<Taxesandfee> searchTax(string search);
}
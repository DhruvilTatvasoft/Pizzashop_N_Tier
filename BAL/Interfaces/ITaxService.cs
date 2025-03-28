using DAL.Data;

public interface ITaxService{
    bool addNewTax(Taxesandfee tax);
    void deleteTax(int taxid);
    List<Taxesandfee> getAllTaxes();
    Taxesandfee getTaxById(int taxid);
    List<Taxesandfee> searchTax(string search);
}
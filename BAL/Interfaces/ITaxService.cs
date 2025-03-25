using DAL.Data;

public interface ITaxService{
    void addNewTax(Taxesandfee tax);
    void deleteTax(int taxid);
    List<Taxesandfee> getAllTaxes();
}
using DAL.Data;

public interface ITaxesRepository{
    void addNewTax(Taxesandfee tax);
    void deleteTax(int taxid);
    List<Taxesandfee> getAlltaxes();
}
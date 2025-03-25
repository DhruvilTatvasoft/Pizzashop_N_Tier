using DAL.Data;

public class TaxesImpl : ITaxService
{

    private readonly ITaxesRepository _taxesRepository;

    public TaxesImpl(ITaxesRepository taxesRepository){
        _taxesRepository = taxesRepository;
    }

    public void addNewTax(Taxesandfee tax)
    {
         _taxesRepository.addNewTax(tax);
    }

    public void deleteTax(int taxid)
    {
       _taxesRepository.deleteTax(taxid);
    }

    public List<Taxesandfee> getAllTaxes()
    {
       return _taxesRepository.getAlltaxes();
    }
}
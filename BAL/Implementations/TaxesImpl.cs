using DAL.Data;

public class TaxesImpl : ITaxService
{

    private readonly ITaxesRepository _taxesRepository;

    public TaxesImpl(ITaxesRepository taxesRepository){
        _taxesRepository = taxesRepository;
    }

    public bool addNewTax(Taxesandfee tax)
    {
         return _taxesRepository.addNewTax(tax);
    }

    public void deleteTax(int taxid)
    {
       _taxesRepository.deleteTax(taxid);
    }

    public List<Taxesandfee> getAllTaxes()
    {
       return _taxesRepository.getAlltaxes();
    }

    public Taxesandfee getTaxById(int taxid)
    {
        return _taxesRepository.getTaxById(taxid);
    }

    public List<Taxesandfee> searchTax(string search)
    {
        return _taxesRepository.getSearchedTax(search);
    }
}
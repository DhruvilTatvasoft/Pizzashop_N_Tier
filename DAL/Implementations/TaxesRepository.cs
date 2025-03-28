using DAL.Data;

public class TaxesRepository : ITaxesRepository
{

    private readonly PizzashopCContext _context;

    public TaxesRepository(PizzashopCContext context){
        _context = context;
    }

    public bool addNewTax(Taxesandfee tax)
    {
        if(_context.Taxesandfees.FirstOrDefault(t=>t.Taxname == tax.Taxname.ToLower().Trim()) == null){

        if(_context.Taxesandfees.FirstOrDefault(t=>t.Taxid == tax.Taxid) != null){
            updateTaxDetails(tax);
        }
        else
        {
        Taxesandfee newTax = new Taxesandfee();
        newTax.Taxname = tax.Taxname;
        newTax.Taxpercentage = tax.Taxpercentage;
        newTax.Isdefault = tax.Isdefault;
        newTax.Isenabled = tax.Isenabled;
        newTax.Taxtype = tax.Taxtype;
        newTax.Createdat = DateTime.Now;
        newTax.Isdeleted = false;
        newTax.Createdby = 1;
        newTax.Modifiedby = 1;
        newTax.Modifiedat = DateTime.Now;
        _context.Taxesandfees.Add(newTax);
        _context.SaveChanges(); 
        }
        return true;
        }
        else{
            return false;
        }
    }

    public void updateTaxDetails(Taxesandfee tax){
        var taxToUpdate = _context.Taxesandfees.FirstOrDefault(t=>t.Taxid == tax.Taxid && t.Isdeleted == false);
        taxToUpdate.Taxname = tax.Taxname;
        taxToUpdate.Taxpercentage = tax.Taxpercentage;
        taxToUpdate.Isdefault = tax.Isdefault;
        taxToUpdate.Isenabled = tax.Isenabled;
        taxToUpdate.Taxtype = tax.Taxtype;

        _context.Taxesandfees.Update(taxToUpdate);
        _context.SaveChanges();
    }
    public void deleteTax(int taxid)
    {
        Taxesandfee tax = _context.Taxesandfees.FirstOrDefault(tax=>tax.Taxid == taxid)!;
        tax.Isdeleted = true;
        _context.Taxesandfees.Update(tax);
        _context.SaveChanges();
    }

    public List<Taxesandfee> getAlltaxes()
    {
        return _context.Taxesandfees.Where(taxes=>taxes.Isdeleted == false).ToList();
    }

    public Taxesandfee getTaxById(int taxid)
    {
        return _context.Taxesandfees.FirstOrDefault(tax=>tax.Taxid == taxid)!;
    }

    public List<Taxesandfee> getSearchedTax(string search)
    {
        List<Taxesandfee> taxes = _context.Taxesandfees.Where(tax=>tax.Taxname.ToLower().Trim().Contains(search.ToLower().Trim()) && tax.Isdeleted == false).ToList();
        return taxes;
    }
}
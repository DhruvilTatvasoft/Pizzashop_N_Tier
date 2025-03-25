using DAL.Data;

public class TaxesRepository : ITaxesRepository
{

    private readonly PizzashopCContext _context;

    public TaxesRepository(PizzashopCContext context){
        _context = context;
    }

    public void addNewTax(Taxesandfee tax)
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
}
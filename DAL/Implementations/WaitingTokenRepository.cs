using DAL.Data;

public class WaitingTokenRepository : IWaitingTokenRepository
{
    private readonly PizzashopCContext _context;

    public WaitingTokenRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public bool addNewWaitingToken(WaitingTokenModel model)
    {
        Waitingtoken token = new Waitingtoken();
        token.Createdat = DateTime.Now;
        token.Sectionid = model.sectionId;
        token.Totalpersons = model.personCount;
        Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customername.ToLower().Trim() == model.customer.name.ToLower().Trim() && customer.Isdeleted == false);
        if(customer == null){
            createCustomer(model.customer);
            token.Customerid = getCustomerId(model.customer.name);
        }
        else{
            token.Customerid = customer.Customerid;
        }
        token.Isdeleted = false;
        token.Createdat = DateTime.Now;
        token.Modifiedat = DateTime.Now;
        token.Createdby = 1;
        token.Modifiedby = 1;

        _context.Waitingtokens.Add(token);
        _context.SaveChanges();

        return true;
    }

    public  void createCustomer(CustomerModel model){
        Customer customer = new Customer();
        customer.Customername = model.name;
        customer.Email = model.email;
        customer.Phonenumber = model.phone;
        customer.Isdeleted = false;
        customer.Createdby = 1;
        customer.Modifiedby = 1;
        _context.Customers.Add(customer);
        _context.SaveChanges();
        
    }
    public int getCustomerId(string name){
        Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customername.ToLower().Trim() == name.ToLower().Trim() && customer.Isdeleted == false);
        if(customer != null){
            return customer.Customerid;
        }
        else{
            return 0;
        }
    }

    public WaitingTokenModel getAllWaingTokens(int sectionid)
    {
        WaitingTokenModel model = new WaitingTokenModel();
        IQueryable<Waitingtoken> query = _context.Waitingtokens.Where(token => token.Isdeleted == false);
        if(sectionid == 0){
         query = query.Where(token => token.Isdeleted == false);
        model.TotalWaitingTokens = query.ToList().Count();
        }
        else{
            query = query.Where(token => token.Isdeleted == false && token.Sectionid == sectionid);
        }
        model.waitingTokens = query.ToList();
        foreach (var token in model.waitingTokens)
        {
            // token.Customer = _context.Customers.Where(c => c.Customerid == token.Customerid).FirstOrDefault() ?? new Customer();
        }
        return model;
    }

    public Dictionary<Section,int> getSectionsWithWaitingTokens()
    {
        List<Section> sections = _context.Sections.Where(section=>section.Isdeleted == false).ToList();
        Dictionary<Section,int> SectionAndTokenCount = new Dictionary<Section, int>();
        foreach(var section in sections){
            int tokenCount = _context.Waitingtokens.Where(token => token.Isdeleted == false && token.Sectionid == section.Sectionid).Count();
            SectionAndTokenCount.Add(section,tokenCount);
        }
        return SectionAndTokenCount;
    }

    public WaitingTokenModel getTokenDetails(int waitingTokenId)
    {
        WaitingTokenModel model = new WaitingTokenModel();
        model.tokenId = waitingTokenId;
        var waitingToken = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == waitingTokenId && token.Isdeleted == false);
        int customerid = waitingToken != null ? waitingToken.Customerid : 0;
        Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customerid == customerid) ?? new Customer();
        Console.WriteLine(customer.Customername);
        CustomerModel Customer = new CustomerModel();
        Customer.name = customer.Customername;
        Customer.email = customer.Email;
        Customer.phone = customer.Phonenumber;
        model.customer = Customer;
        model.personCount = waitingToken.Totalpersons;
        model.sectionId = waitingToken.Sectionid;
        return model;

    }

    public bool updateWaitingToken(WaitingTokenModel model)
    {
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == model.tokenId)!;
        token.Totalpersons = model.personCount;
        token.Sectionid = model.sectionId;
        updateCustomerDetail(token.Customerid,model.customer);
        _context.Waitingtokens.Update(token);
        _context.SaveChanges();
        return true;
        
    }

    private void updateCustomerDetail(int customerid, CustomerModel customer)
    {
        Customer Customer = _context.Customers.FirstOrDefault(Customer=>Customer.Customerid == customerid);
            Customer.Customername = customer.name;
            Customer.Phonenumber = customer.phone;
            Customer.Email = customer.email;
            _context.Customers.Update(Customer);
            _context.SaveChanges();
    }

    public int getTotalWaitingTokens()
    {
        return _context.Waitingtokens.Where(token=>token.Isdeleted == false).Count();
    }
}
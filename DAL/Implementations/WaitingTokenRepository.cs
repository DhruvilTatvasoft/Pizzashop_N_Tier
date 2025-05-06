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
        Customer Customer = _context.Customers.FirstOrDefault(Customer=>Customer.Email.ToLower().Trim() == model.customer.email.ToLower().Trim());
        if(Customer != null){
        Waitingtoken isTokenCreated = _context.Waitingtokens.FirstOrDefault(token => token.Customerid == Customer.Customerid && token.Isdeleted == false);
        if(isTokenCreated != null){
            return false;
        }
        }
        token.Createdat = DateTime.Now;
        token.Sectionid = model.sectionId ?? 0;
        token.Totalpersons = model.personCount ?? 0;
        // Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customername.ToLower().Trim() == model.customer.name.ToLower().Trim() && customer.Isdeleted == false);
        if(Customer == null){
            int customerid = createCustomer(model.customer);
            token.Customerid = getCustomerId(customerid);
        }
        else{
            token.Customerid = Customer.Customerid;
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

    public  int createCustomer(CustomerModel model){
        Customer customer = new Customer();
        customer.Customername = model.name;
        customer.Email = model.email;
        customer.Phonenumber = model.phone;
        customer.Createdat = DateTime.Now;
        customer.Modifiedat = DateTime.Now;
        customer.Isdeleted = false;
        customer.Createdby = 1;
        customer.Modifiedby = 1;
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return customer.Customerid;
        
    }
    public int getCustomerId(int customerid){
        Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customerid == customerid)!;
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
            token.Customer = _context.Customers.Where(c => c.Customerid == token.Customerid).FirstOrDefault() ?? new Customer();
        }
        model.sectionId = sectionid;
        return model;
    }

    public Dictionary<Section,int> getSectionsWithWaitingTokens()
    {
        List<Section> sections = _context.Sections.Where(section=>section.Isdeleted == false).OrderBy(section=>section.Sectionid).ToList();
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
        token.Totalpersons = model.personCount ?? 0;
        token.Sectionid = model.sectionId ?? 0;
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

    public List<CustomerModel> getSuggestedCustomerList(string name)
    {
       var customer = _context.Customers.Where(customer=>customer.Email.ToLower().Trim().Contains(name.ToLower().Trim())).ToList();
       List<CustomerModel> customerModel = new List<CustomerModel>();
       foreach (var item in customer){
        CustomerModel model = new CustomerModel();
        model.name = item.Customername;
        model.email = item.Email;
        model.phone = item.Phonenumber;
        customerModel.Add(model);
       }
       return customerModel;
    }

    public bool deleteWaitingToken(int tokenid)
    {
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid)!;
        token.Isdeleted = true;
        _context.Waitingtokens.Update(token);
        _context.SaveChanges();
        return true;
        
    }

    public int getSectionIdOfToken(int tokenid)
    {
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid && token.Isdeleted == false)!;
        return token.Sectionid;
    }

    public List<Table> getTablesForToken(int tokenid)
    {
        int sectionId = getSectionIdOfToken(tokenid);
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid && token.Isdeleted == false)!;
        List<Table> tables = _context.Tables.Where(table=>table.Sectionid == sectionId && table.Statusname == "Available" && table.Capacity >= token.Totalpersons).ToList();
        return tables;
    }

    public bool assignTable(int tableid, int tokenid)
    {
        Table table = _context.Tables.FirstOrDefault(table=>table.Tableid == tableid)!;
        table.Status = false;
        table.Statusname = "Assigned";
        // Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid)!;
        // table.Customerid = token.Customerid;
        // _context.Waitingtokens.Update(token);
        _context.Tables.Update(table);
        _context.SaveChanges();
        return true;
    }

    public List<CustomerModel> getCustomerTokensForSection(int sectionid,List<int> tableid)
    {
        List<CustomerModel> customerViewModels = new List<CustomerModel>();
        List<Table> tables = new List<Table>();
        int tableCapacity = 0;
        foreach(var id in tableid){
            Table table = _context.Tables.FirstOrDefault(table=>table.Tableid == id)!;
            tables.Add(table);
            tableCapacity += table.Capacity;
        }
        List<Waitingtoken> tokens = _context.Waitingtokens.Where(token => token.Isdeleted == false && token.Totalpersons >= tableCapacity && token.Sectionid == sectionid).ToList();
        foreach (var token in tokens)
        {
            Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customerid == token.Customerid && customer.Isdeleted == false)!;
            CustomerModel model = new CustomerModel();
            model.name = customer.Customername;
            model.phone = customer.Phonenumber;
            model.email = customer.Email;
            model.section = _context.Sections.FirstOrDefault(section=>section.Sectionid == sectionid && section.Isdeleted == false)!;
            model.PersonCount = token.Totalpersons;
            model.tokenid = token.Waitingtokenid;
            customerViewModels.Add(model);
        }
        return customerViewModels;
    }

    public CustomerModel getCustomerForWaitingToken(int tokenid, List<int> tableid)
    {
        CustomerModel model = new CustomerModel();
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid && token.Isdeleted == false)!;
        Customer customer = _context.Customers.FirstOrDefault(customer=>customer.Customerid == token.Customerid && customer.Isdeleted == false)!;
        model.customerId = customer.Customerid;
        model.name = customer.Customername;
        model.phone = customer.Phonenumber;
        model.email = customer.Email;
        List<Table> tables = new List<Table>();
        List<int> tableids = new List<int>();
        int sectionid = 0;
        foreach(var id in tableid){
            if(id != 0){
            Table table = _context.Tables.FirstOrDefault(table=>table.Tableid == id)!;
            tableids.Add(table.Tableid);
            sectionid = table.Sectionid;
            tables.Add(table);
            }
        }
        if(sectionid != 0){
            model.section = _context.Sections.FirstOrDefault(section=>section.Sectionid == sectionid && section.Isdeleted == false)!;
        }
        model.PersonCount = token!.Totalpersons;
        model.tableids = tableids;
        model.tables = tables;
        return model;
    }

    public int getTokenidFromCustomerEmail(string email)
    {
        Customer customer = _context.Customers.FirstOrDefault(ExistingCustomer=>ExistingCustomer.Email == email)!;
        int tokenid = _context.Waitingtokens.FirstOrDefault(token=>token.Customerid == customer.Customerid)!.Waitingtokenid;
        return tokenid;
    }
}
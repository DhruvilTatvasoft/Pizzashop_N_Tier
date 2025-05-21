using System.Data;
using DAL.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class WaitingTokenRepository : IWaitingTokenRepository
{
    private readonly PizzashopCContext _context;
    private readonly IConfiguration _configuration;

    public WaitingTokenRepository(PizzashopCContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public bool addNewWaitingToken(WaitingTokenModel model)
    {
        // Waitingtoken token = new Waitingtoken();
        // Customer Customer = _context.Customers.FirstOrDefault(Customer => Customer.Email.ToLower().Trim() == model.customer.email.ToLower().Trim());
        // if (Customer != null)
        // {
        //     Waitingtoken isTokenCreated = _context.Waitingtokens.FirstOrDefault(token => token.Customerid == Customer.Customerid && token.Isdeleted == false);
        //     if (isTokenCreated != null)
        //     {
        //         return false;
        //     }
        // }
        // token.Createdat = DateTime.Now;
        // token.Sectionid = model.sectionId ?? 0;
        // token.Totalpersons = model.personCount ?? 0;
        // if (Customer == null)
        // {
        //     token.Customerid = createCustomer(model.customer).Customerid;
        // }
        // else
        // {
        //     token.Customerid = Customer.Customerid;
        // }
        // token.Isdeleted = false;
        // token.Createdat = DateTime.Now;
        // token.Modifiedat = DateTime.Now;
        // token.Createdby = 1;
        // token.Modifiedby = 1;

        // _context.Waitingtokens.Add(token);
        // _context.SaveChanges();

        // IN p_customername character varying(50),
        // IN p_phonenumber character varying(50),
        // IN p_email character varying(100),
        // IN p_sectionid integer,
        // IN p_personcount integer,
        // OUT p_success boolean

        const string query = @"SELECT public.createnewwaitingtoken(@p_customername, @p_phonenumber, @p_email, @p_sectionid, @p_personcount)";

        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();

            var result = connection.QueryFirstOrDefault<bool>(query, new
            {
                p_customername = model.customer.name,
                p_phonenumber = model.customer.phone,
                p_email = model.customer.email,
                p_sectionid = model.sectionId,
                p_personcount = model.personCount
            });
            return result;
        }
    }

    public Customer createCustomer(CustomerModel model)
    {
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
        return customer;
    }
    public int getCustomerId(int customerid)
    {
        Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == customerid)!;
        if (customer != null)
        {
            return customer.Customerid;
        }
        else
        {
            return 0;
        }
    }

    public WaitingTokenModel getAllWaingTokens(int sectionid)
    {
        WaitingTokenModel model = new WaitingTokenModel();
        List<Section> sections = _context.Sections.Where(section => section.Isdeleted == false).OrderBy(section => section.Sectionid).ToList();
        const string query = @" SELECT * FROM getAllWaitingTokensOfSection(@sectionId)";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        var result = connection.Query<dynamic>(query, new { sectionId = sectionid }).ToList();
        List<Waitingtoken> waitingTokenList = new List<Waitingtoken>();
        foreach (var row in result)
        {
            Waitingtoken token = new Waitingtoken();
            token.Sectionid = row.sectionid;
            token.Customerid = row.customerid;
            token.Waitingtokenid = row.waitingtokenid;
            token.Totalpersons = row.totalpersons;
            token.Isdeleted = false;
            token.Createdat = row.createdat;
            waitingTokenList.Add(token);
            Customer c = new Customer();
            c.Customerid = row.customerid;
            c.Customername = row.customername;
            c.Phonenumber = row.phonenumber;
            c.Email = row.email;
            token.Customer = c;
        }
        model.waitingTokens = waitingTokenList;
        if (sectionid == 0)
        {
            model.TotalWaitingTokens = waitingTokenList.Count;
        }
        model.sectionId = sectionid;
        return model;
    }

    public Dictionary<Section, int> getSectionsWithWaitingTokens()
    {
        List<Section> sections = _context.Sections.Where(section => section.Isdeleted == false).OrderBy(section => section.Sectionid).ToList();
        const string query = @" SELECT * FROM getsectionswithtokencount()";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        connection.Open();

        var results = connection.Query<dynamic>(query).ToList();
        Dictionary<Section, int> SectionAndTokenCount = new Dictionary<Section, int>();
        foreach (var row in results)
        {
            Section section = new Section();
            section.Sectionid = row.section_id;
            section.Sectionname = row.section_name;
            int tokenCount = (int)row.token_count;
            SectionAndTokenCount.Add(section, tokenCount);
        }
        return SectionAndTokenCount;
    }

    public WaitingTokenModel getTokenDetails(int waitingTokenId)
    {
        WaitingTokenModel model = new WaitingTokenModel();
        List<Section> sections = _context.Sections.Where(section => section.Isdeleted == false).OrderBy(section => section.Sectionid).ToList();
        const string query = @" SELECT * FROM getTokenDetails(@tokenid)";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        connection.Open();
        var result = connection.Query<dynamic>(query, new { tokenid = waitingTokenId }).FirstOrDefault();
        model.tokenId = waitingTokenId;
        Waitingtoken token = new Waitingtoken();
        token.Waitingtokenid = result.waitingtokenid;
        CustomerModel customer = new CustomerModel();
        customer.name = result.customername;
        customer.customerId = result.customerid;
        customer.phone = result.phonenumber;
        customer.email = result.email;
        customer.PersonCount = result.totalpersons;
        model.personCount = result.totalpersons;
        model.sectionId = result.totalpersons;
        var waitingToken = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == waitingTokenId && token.Isdeleted == false);
        int customerid = waitingToken != null ? waitingToken.Customerid : 0;
        model.customer = customer;
        return model;
    }

    public bool updateWaitingToken(WaitingTokenModel model)
    {
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == model.tokenId)!;
        token.Totalpersons = model.personCount ?? 0;
        token.Sectionid = model.sectionId ?? 0;
        updateCustomerDetail(token.Customerid, model.customer);
        _context.Waitingtokens.Update(token);
        _context.SaveChanges();
        return true;
    }

    private void updateCustomerDetail(int customerid, CustomerModel customer)
    {
        Customer Customer = _context.Customers.FirstOrDefault(Customer => Customer.Customerid == customerid);
        const string query = @"CALL updatecustomer(@customerId , @customername, @phonenumber, @Email)";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        connection.Open();
        var result = connection.Execute(query, new
        {
            customerId = customerid,
            customername = customer.name,
            phonenumber = customer.phone,
            Email = customer.email,
        });
        // Customer.Customername = customer.name;
        // Customer.Phonenumber = customer.phone;
        // Customer.Email = customer.email;
        // _context.Customers.Update(Customer);
        // _context.SaveChanges();
    }

    public int getTotalWaitingTokens()
    {
        return _context.Waitingtokens.Where(token => token.Isdeleted == false).Count();
    }

    public List<CustomerModel> getSuggestedCustomerList(string name)
    {
        List<CustomerModel> customerModel = new List<CustomerModel>();
        // var customer = _context.Customers.Where(customer => customer.Email.ToLower().Trim().Contains(name.ToLower().Trim())).ToList();
        const string query = @"select *from search_customers_by_email_part(@name)";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        connection.Open();
        var result = connection.Query<dynamic>(query, new { name = name }).ToList();
        foreach (var row in result)
        {
            CustomerModel model = new CustomerModel();
            model.name = row.customername;
            model.email = row.email;
            model.phone = row.phonenumber;
            customerModel.Add(model);
        }
        return customerModel;
    }

    public bool deleteWaitingToken(int tokenid)
    {
        // Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid)!;
        // token.Isdeleted = true;
        // token.Modifiedat = DateTime.Now;
        // _context.Waitingtokens.Update(token);
        // _context.SaveChanges();
        // return true;

        const string query = @"CALL deletewaitingtoken(@tokenid)";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        connection.Open();
        var result = connection.Query<dynamic>(query, new { tokenid = tokenid });
        return true;

    }

    public int getSectionIdOfToken(int tokenid)
    {
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid && token.Isdeleted == false)!;
        return token.Sectionid;
    }

    public List<Table> getTablesForToken(int tokenid)
    {
        int sectionId = getSectionIdOfToken(tokenid);
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid && token.Isdeleted == false)!;
        List<Table> tables = _context.Tables.Where(table => table.Sectionid == sectionId && table.Statusname == "Available" && table.Capacity >= token.Totalpersons).ToList();
        return tables;
    }

    public bool assignTable(List<int> tableids, int tokenid)
    {
        // List<Table> tables = _context.Tables.Where(table => tableids.Contains(table.Tableid)).ToList()!;
        // Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid)!;
        // foreach (var table in tables)
        // {
        //     table.Status = false;
        //     table.Statusname = "Assigned";
        //     Ordertable orderedTable = new Ordertable();
        //     orderedTable.Tableid = table.Tableid;
        //     orderedTable.Customerid = token.Customerid;
        //     orderedTable.Isdeleted = false;
        //     orderedTable.Createdat = DateTime.Now;
        //     orderedTable.Modifiedat = DateTime.Now;
        //     orderedTable.Createdby = 1;
        //     orderedTable.Modifiedby = 1;
        //     orderedTable.TotalPersonCount = token.Totalpersons;
        //     _context.Ordertables.Add(orderedTable);
        //     _context.Tables.Update(table);
        // }
        // token.Isdeleted = true;
        // token.Modifiedat = DateTime.Now;
        // _context.Waitingtokens.Update(token);
        // _context.SaveChanges();

        const string query = @"CALL public.assign_tables_to_token(@p_tableids, @p_tokenid)";
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            connection.Execute(query, new
            {
                p_tableids = tableids.ToArray(),
                p_tokenid = tokenid
            });
        }
        return true;
    }

    public List<CustomerModel> getCustomerTokensForSection(int sectionid, List<int> tableid)
    {
        List<CustomerModel> customerViewModels = new List<CustomerModel>();
        List<Table> tables = new List<Table>();
        int tableCapacity = 0;
        foreach (var id in tableid)
        {
            Table table = _context.Tables.FirstOrDefault(table => table.Tableid == id)!;
            tables.Add(table);
            tableCapacity += table.Capacity;
        }
        List<Waitingtoken> tokens = _context.Waitingtokens.Where(token => token.Isdeleted == false && token.Totalpersons <= tableCapacity && token.Sectionid == sectionid).ToList();
        foreach (var token in tokens)
        {
            Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == token.Customerid && customer.Isdeleted == false)!;
            CustomerModel model = new CustomerModel();
            model.name = customer.Customername;
            model.phone = customer.Phonenumber;
            model.email = customer.Email;
            model.section = _context.Sections.FirstOrDefault(section => section.Sectionid == sectionid && section.Isdeleted == false)!;
            model.PersonCount = token.Totalpersons;
            model.tokenid = token.Waitingtokenid;
            customerViewModels.Add(model);
        }
        return customerViewModels;
    }

    public CustomerModel getCustomerForWaitingToken(int tokenid, List<int> tableid)
    {
        CustomerModel model = new CustomerModel();
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid && token.Isdeleted == false)!;
        // Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == token.Customerid && customer.Isdeleted == false)!;
        const string query = @"SELECT * FROM getcustomerforwaitingtoken(@tokenid)";
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            var result = connection.Query<dynamic>(query, new { tokenid = tokenid }).FirstOrDefault();
            model.customerId = result.customerid;
            model.name = result.customername;
            model.phone = result.phonenumber;
            model.email = result.email;
        }
        List<Table> tables = new List<Table>();
        List<int> tableids = new List<int>();
        int sectionid = 0;
        foreach (var id in tableid)
        {
            if (id != 0)
            {
                Table table = _context.Tables.FirstOrDefault(table => table.Tableid == id)!;
                tableids.Add(table.Tableid);
                sectionid = table.Sectionid;
                tables.Add(table);
            }
        }
        if (sectionid != 0)
        {
            model.section = _context.Sections.FirstOrDefault(section => section.Sectionid == sectionid && section.Isdeleted == false)!;
        }
        model.PersonCount = token!.Totalpersons;
        model.tableids = tableids;
        model.tables = tables;
        return model;
    }           
    public int getTokenidFromCustomerEmail(string email)
    {
        Customer customer = _context.Customers.FirstOrDefault(ExistingCustomer => ExistingCustomer.Email == email)!;
        int tokenid = _context.Waitingtokens.FirstOrDefault(token => token.Customerid == customer.Customerid)!.Waitingtokenid;
        return tokenid;
    }

    public int? getMaxPersonCountForSection(List<int> tableids)
    {
        int maxPersonCount = _context.Tables.Where(tables => tableids.Contains(tables.Tableid)).Select(tables => tables.Capacity).Sum();
        return maxPersonCount;
    }
}
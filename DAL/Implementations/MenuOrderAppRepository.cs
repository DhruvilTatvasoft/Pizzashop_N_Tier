using System.Data;
using DAL.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using Newtonsoft.Json;
using System.Transactions;
using Newtonsoft.Json.Linq;

public class MenuOrderAppRepository : IMenuOrderAppRepository
{
    private readonly PizzashopCContext _context;
    private readonly IConfiguration _configuration;

    public MenuOrderAppRepository(PizzashopCContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public int createOrder(OrderDetailsViewModel orderDetails)
    {
        var transaction = _context.Database.BeginTransaction();
        try
        {
            Order order = new Order();
            if (orderDetails.orderid != 0)
            {
                order = _context.Orders.FirstOrDefault(existingOrder => existingOrder.Orderid == orderDetails.orderid)!;
            }
            order.Customerid = orderDetails.customerid;
            if (orderDetails.ordercomment != "" || orderDetails.ordercomment != null)
            {
                order.Ordercomment = orderDetails.ordercomment;
            }
            order.Totalamount = (decimal?)orderDetails.totalamount;
            order.PaymentStatus = "Pending";
            order.Statusid = 4;
            order.Sectionid = orderDetails.sectionid;
            order.Totalpersons = orderDetails.totalPersons;
            order.Paymentmethod = orderDetails.PaymentMethod;
            if (orderDetails.orderid == 0)
            {
                order.Createdat = DateTime.Now;
                order.Createdby = 1;
            }
            order.Modifiedby = 1;
            order.IsDeleted = false;
            if (orderDetails.orderid != 0)
            {
                _context.Orders.Update(order);
            }
            else
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();

            if (orderDetails.orderid != 0)
            {
                List<string> existingUniqueids = _context.Orderitems.Where(orderedItem => orderedItem.Orderid == orderDetails.orderid).Select(orderedItem => orderedItem.Uniqueid).ToList()!;
                List<string> currentUniqueIds = orderDetails.uniqueids!;
                List<string> itemsToDelete = existingUniqueids.Except(currentUniqueIds ?? new List<string>()).ToList();

                foreach (var id in itemsToDelete)
                {
                    int itemDetailId = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Uniqueid == id)!.Orderitemid;
                    Orderitem orderedItem = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Uniqueid == id)!;
                    _context.Orderitems.Remove(orderedItem);
                    List<OrderItemModifier> itemsAndModifiers = _context.OrderItemModifiers.Where(OrderdItemModifiers => OrderdItemModifiers.Orderitemdetailid == itemDetailId).ToList();
                    _context.OrderItemModifiers.RemoveRange(itemsAndModifiers);
                }
                _context.SaveChanges();
            }
            if (orderDetails.orderid == 0)
            {
                foreach (var tableid in orderDetails.tableids)
                {
                    Ordertable ordertable = new Ordertable();
                    bool isTableAdded = false;
                    if (orderDetails.orderid != 0)
                    {
                        ordertable = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Orderid == orderDetails.orderid)!;
                    }
                    if (ordertable.Ordertableid == 0)
                    {
                        ordertable = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Customerid == orderDetails.customerid && orderedTable.Isdeleted == false)!;
                    }
                    if (ordertable == null)
                    {
                        ordertable = new Ordertable();
                        ordertable.Orderid = order.Orderid;
                        ordertable.Tableid = tableid;
                        ordertable.Isdeleted = false;
                        ordertable.Createdby = 1;
                        ordertable.Modifiedby = 1;
                        ordertable.Customerid = orderDetails.customerid;
                        ordertable.TotalPersonCount = orderDetails.totalPersons;
                        isTableAdded = true;
                    }
                    else
                    {
                        if (ordertable.Orderid == null || ordertable.Orderid == 0)
                        {
                            ordertable.Orderid = order.Orderid;
                        }
                    }
                    Table table = _context.Tables.FirstOrDefault(tbl => tbl.Tableid == tableid)!;
                    table.Status = false;
                    table.Statusname = "Running";
                    table.Customerid = orderDetails.customerid;
                    _context.Tables.Update(table);
                    if (!isTableAdded)
                    {
                        _context.Ordertables.Update(ordertable);
                    }
                    else
                    {
                        _context.Ordertables.Add(ordertable);
                    }
                }
            }
            _context.SaveChanges();
            foreach (var item in orderDetails.itemDetails)
            {
                string uniqueid = "item_" + item.itemId + "_";
                Orderitem? orderedItem = new Orderitem();
                Orderitem? orderedItem2 = new Orderitem();
                orderedItem = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Orderid == orderDetails.orderid && orderedItem.Uniqueid == item.uniqueid);
                bool itemUpdated = false;
                if (orderedItem != null)
                {
                    orderedItem.Orderitemquantity = int.Parse(item.quantity!);
                    orderedItem.Specialcomment = item.itemcomment;
                    _context.Orderitems.Update(orderedItem);
                    itemUpdated = true;
                }
                else
                {
                    orderedItem2.Orderid = order.Orderid;
                    orderedItem2.Itemid = int.Parse(item.itemId!);
                    orderedItem2.Orderitemquantity = int.Parse(item.quantity!);
                    orderedItem2.Createdby = 1;
                    orderedItem2.Modifiedby = 1;
                    orderedItem2.Specialcomment = item.itemcomment;
                    orderedItem2.Readyitemquanitiy = 0;
                    _context.Orderitems.Add(orderedItem2);
                }

                _context.SaveChanges();
                if (!itemUpdated)
                {
                    foreach (var modifierid in item.modifierIds!)
                    {
                        OrderItemModifier? itemModifier = new OrderItemModifier();
                        itemModifier.ItemId = int.Parse(item.itemId!);
                        itemModifier.Modifierid = int.Parse(modifierid);
                        itemModifier.ModifierQuantity = int.Parse(item.quantity!);
                        itemModifier.Orderitemdetailid = orderedItem2.Orderitemid;
                        itemModifier.Orderid = order.Orderid;
                        _context.OrderItemModifiers.Add(itemModifier);
                    }
                    _context.SaveChanges();
                    List<int> modifierIds = item.modifierIds.Select(modifierId => int.Parse(modifierId)).ToList();
                    modifierIds.Sort();
                    foreach (var id in modifierIds)
                    {
                        uniqueid += id + "_";
                    }
                    uniqueid = uniqueid.TrimEnd('_');
                    orderedItem2.Uniqueid = uniqueid;
                    _context.Orderitems.Update(orderedItem2);
                    _context.SaveChanges();
                }
            }
            if (orderDetails.orderid != 0)
            {
                List<Ordertaxesandfee> taxAndFees = _context.Ordertaxesandfees.Where(Tax => Tax.Orderid == orderDetails.orderid).ToList();
                _context.Ordertaxesandfees.RemoveRange(taxAndFees);
                _context.SaveChanges();
            }
            foreach (var tax in orderDetails.appliedTaxes)
            {
                Ordertaxesandfee taxAndFees = new Ordertaxesandfee();
                if (orderDetails.orderid != null)
                {
                    var taxesToRemove = _context.Ordertaxesandfees.Where(Orderedtaxesandfees => Orderedtaxesandfees.Orderid == orderDetails.orderid);
                    _context.Ordertaxesandfees.RemoveRange(taxesToRemove);
                }
                taxAndFees.Orderid = order.Orderid;
                taxAndFees.Taxname = tax.taxname;
                taxAndFees.TaxPercentage = (decimal?)float.Parse(tax.taxPercentage.ToString());
                taxAndFees.Taxtype = tax.taxtype;
                _context.Ordertaxesandfees.Add(taxAndFees);
            }
            _context.SaveChanges();
            transaction.Commit();
            return order.Orderid;
        }
        catch (Exception)
        {
            transaction.Rollback();
            return 0;
        }
    }
    public Item getItem(int itemid)
    {
        return _context.Items.FirstOrDefault(item => item.Itemid == itemid)!;
    }
    public List<Item> getItemsForcategory(int categoryid, string ItemType, string searchedItem)
    {
        const string query = "CALL getitemsforcategory(@p_categoryid, @p_itemtype, @p_searcheditem, @result_cur)";
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString"));
        connection.Open();
        using var transaction = connection.BeginTransaction();
        using var command = new NpgsqlCommand(query, connection, transaction);
        command.Parameters.AddWithValue("p_categoryid", categoryid);
        command.Parameters.AddWithValue("p_itemtype", string.IsNullOrWhiteSpace(ItemType) ? (object)DBNull.Value : ItemType);
        command.Parameters.AddWithValue("p_searcheditem", string.IsNullOrWhiteSpace(searchedItem) ? (object)DBNull.Value : searchedItem);
        command.Parameters.Add(new NpgsqlParameter("result_cur", NpgsqlDbType.Refcursor) { Direction = ParameterDirection.InputOutput, Value = "mycursor" });
        command.ExecuteNonQuery();
        var result = connection.Query<dynamic>($"FETCH ALL FROM mycursor;", transaction: transaction).ToList();
        List<Item> itemlist = new List<Item>();
        foreach (var item in result)
        {
            Item item1 = new Item();
            item1.Itemname = item.itemname;
            item1.Itemid = item.itemid;
            item1.Itemtype = item.itemtype;
            item1.Itemrate = item.itemrate;
            item1.Itemimage = item.itemimage;
            item1.Categoryid = item.categoryid;
            item1.Isfavourite = item.isfavourite;
            itemlist.Add(item1);
        }
        transaction.Commit();
        return itemlist;
        // bool? isVeg = null;
        // if (!string.IsNullOrEmpty(ItemType))
        // {
        //     if (ItemType.ToLower() == "veg")
        //         isVeg = true;
        //     else if (ItemType.ToLower() == "non-veg")
        //         isVeg = false;
        // }
        // var query = _context.Items.AsQueryable();
        // query = query.Where(item => item.Isdeleted == false);
        // if (categoryid != 0)
        // {
        //     query = query.Where(item => item.Categoryid == categoryid);
        // }
        // if (!string.IsNullOrEmpty(searchedItem))
        // {
        //     string lowerSearch = searchedItem.ToLower().Trim();
        //     query = query.Where(item => item.Itemname.ToLower().Trim().Contains(lowerSearch));
        // }
        // if (isVeg.HasValue)
        // {
        //     query = query.Where(item => item.Itemtype == isVeg.Value);
        // }
        // return query.ToList();
    }
    public List<ModifierModel> getModifiersForItem(int itemid)
    {
        List<ModifierModel> model = new List<ModifierModel>();
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL getitemmodifiers(@itemid,@result_cur);", connection, transaction))
                {
                    cmd.Parameters.AddWithValue("itemid", itemid);
                    cmd.Parameters.Add(new NpgsqlParameter("result_cur", NpgsqlDbType.Refcursor) { Direction = ParameterDirection.InputOutput, Value = "mycursor" });
                    cmd.ExecuteNonQuery();
                    var result = connection.Query<dynamic>($"FETCH ALL FROM mycursor;", transaction: transaction).ToList();

                    HashSet<int> processedGroups = new HashSet<int>();
                    foreach (var row in result)
                    {
                        if (processedGroups.Contains(row.modifiergroupid))
                        {
                            continue;
                        }
                        ModifierModel modifierModel = new ModifierModel();
                        Modifiergroup mg = new Modifiergroup
                        {
                            Modifiergroupname = row.modifiergroupname,
                            Modifiergroupid = row.modifiergroupid
                        };
                        modifierModel.modifiergroup = mg;
                        modifierModel.min_value = row.min_value;
                        modifierModel.max_value = row.max_value;
                        List<Modifier> modifierList = new List<Modifier>();
                        foreach (var modifier in result)
                        {
                            if (modifier.modifiergroupid == row.modifiergroupid)
                            {
                                Modifier mod = new Modifier
                                {
                                    Modifierid = modifier.modifierid,
                                    Modifiername = modifier.modifiername,
                                    Modifierrate = modifier.modifierrate
                                };
                                modifierList.Add(mod);
                            }
                        }
                        modifierModel.modifiers = modifierList;
                        model.Add(modifierModel);
                        processedGroups.Add(row.modifiergroupid);
                    }
                }
                transaction.Commit();
            }
        }
        return model;
        // List<ModifierModel> model = new List<ModifierModel>();
        // List<Itemsandmodifier> itemsandmodifiers = _context.Itemsandmodifiers.Where(x => x.Itemid == itemid && x.Isdeleted == false).ToList();
        // foreach (var itemmodifier in itemsandmodifiers)
        // {
        //     ModifierModel modifierModel = new ModifierModel();
        //     modifierModel.modifiergroup = _context.Modifiergroups.FirstOrDefault(mg => mg.Modifiergroupid == itemmodifier.Modifiergroupid && mg.Isdeleted == false);
        //     if (modifierModel.modifiergroup != null)
        //     {
        //         List<Modifier> modifiers = _context.Modifiers.Where(modifier => modifier.Modifiergroupid == itemmodifier.Modifiergroupid && modifier.Isdeleted == false).ToList();
        //         if (modifiers.Count == 0)
        //         {
        //             modifierModel.modifiers = new List<Modifier>();
        //         }
        //         else
        //         {
        //             modifierModel.modifiers = modifiers;
        //         }
        //         modifierModel.min_value = itemmodifier.Requiredminselection;
        //         modifierModel.max_value = itemmodifier.Allowedmaxselection;
        //         model.Add(modifierModel);
        //     }
        // }
        // return model;
    }
    public void saveCustomerDetails(CustomerModel customer)
    {

        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand("CALL savecustomerdetails(@customerid,@customername,@email,@phone,@totalpersoncount)", connection, transaction))
                {
                    cmd.Parameters.AddWithValue("customerid", customer.customerId!); ;
                    cmd.Parameters.AddWithValue("customername", customer.name); ;
                    cmd.Parameters.AddWithValue("email", customer.email);
                    cmd.Parameters.AddWithValue("phone", customer.phone);
                    cmd.Parameters.AddWithValue("totalpersoncount", customer.PersonCount);
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }
        // Customer ExistingCustomer = _context.Customers.FirstOrDefault(Customer => Customer.Customerid == customer.customerId)!;
        // ExistingCustomer.Customername = customer.name;
        // ExistingCustomer.Email = customer.email;
        // ExistingCustomer.Phonenumber = customer.phone;
        // ExistingCustomer.Modifiedat = DateTime.Now;
        // Ordertable table = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Customerid == customer.customerId && orderedTable.Isdeleted == false)!;
        // table.TotalPersonCount = customer.PersonCount;
        // _context.Ordertables.Update(table);
        // Waitingtoken token = _context!.Waitingtokens.FirstOrDefault(token => token.Customerid == customer.customerId && token.Isdeleted == false)!;
        // if (token != null)
        // {
        //     token.Totalpersons = customer.PersonCount;
        //     _context.Waitingtokens.Update(token);
        // }   
        // _context.Customers.Update(ExistingCustomer);
        // _context.SaveChanges();
    }

    // public void saveOrderWiseComment(MenuOrderAppModel model)
    // {
    //     Order order = _context.Orders.FirstOrDefault(Order => Order.Orderid == model.orderid);
    //     order.Ordercomment = model.order.Ordercomment;
    //     _context.Orders.Update(order);
    //     _context.SaveChanges();
    // }

    public MenuOrderAppModel getRunningTableOrder(int tableid)
    {
        OrderDetailsViewModel model = new OrderDetailsViewModel();
        MenuOrderAppModel Model = new MenuOrderAppModel();
        Model.orderDetailModel = loadingorderDetailsModel(tableid);
        Model.customer = Model.orderDetailModel.customerModel;
        Model.isTableAssigned = true;
        Model.orderid = Model.orderDetailModel.orderid ?? 0;
        Model.orderComment = Model.orderDetailModel.ordercomment;
        return Model;
    }

    public OrderDetailsViewModel loadingorderDetailsModel(int tableid)
    {
        OrderDetailsViewModel resultmodel = new OrderDetailsViewModel();
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var query = "Call get_order_details_for_table(@tableid, @result_json)";
                using (var command = new NpgsqlCommand(query, connection, transaction))
                {
                    Ordertable? orderTable = _context.Ordertables.FirstOrDefault(orderTable => orderTable.Tableid == tableid && orderTable.Isdeleted == false);
                    Order order = _context.Orders.FirstOrDefault(o => o.Orderid == orderTable!.Orderid) ?? new Order();
                    List<int> tableids = _context.Ordertables.Where(table => table.Orderid == order.Orderid).Select(table => table.Tableid).ToList();
                    command.Parameters.AddWithValue("tableid", tableid);
                    List<Table> Ordertables = new List<Table>();
                    foreach (var id in tableids)
                    {
                        Table table = _context.Tables.FirstOrDefault(table => table.Tableid == id)!;
                        Ordertables.Add(table);
                    }
                    var resultParameter = new NpgsqlParameter("result_json", NpgsqlDbType.Json)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "[]"
                    };
                    command.Parameters.Add(resultParameter);
                    command.ExecuteNonQuery();
                    var result = resultParameter.Value.ToString();
                    var model = JsonConvert.DeserializeObject<List<customerOrderDetailsViewModel>>(result!);
                    CustomerModel customermodel = new CustomerModel();
                    customermodel.tables = Ordertables;
                    if (model != null && model.Count > 0)
                    {
                        customermodel.customerId = model[0].customerid;
                        customermodel.name = model[0].customername;
                        customermodel.email = model[0].email;
                        customermodel.phone = model[0].Phonenumber;
                        customermodel.PersonCount = model[0].personcount;
                        customermodel.tableids = tableids;
                    }

                    string queryfortaxdetails = "call get_order_tax_details(@orderid,@result_json)";
                    var commandfortaxdetails = new NpgsqlCommand(queryfortaxdetails, connection, transaction);
                    commandfortaxdetails.Parameters.AddWithValue("orderid", order.Orderid);
                    var taxResultParameter = new NpgsqlParameter("result_json", NpgsqlDbType.Json)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "[]"
                    };
                    commandfortaxdetails.Parameters.Add(taxResultParameter);
                    commandfortaxdetails.ExecuteNonQuery();
                    var taxResult = taxResultParameter.Value.ToString();
                    List<appliedTaxDetails>? appliedTaxDetails = JsonConvert.DeserializeObject<List<appliedTaxDetails>>(taxResult!);
                    if (appliedTaxDetails == null)
                    {
                        appliedTaxDetails = new List<appliedTaxDetails>();
                    }
                    resultmodel.appliedTaxes = appliedTaxDetails;
                    resultmodel.customerModel = customermodel;
                    resultmodel.orderid = order.Orderid;
                    resultmodel.ordercomment = order.Ordercomment ?? "";
                    resultmodel.PaymentMethod = order.Paymentmethod ?? "";
                }
            }
            return resultmodel;
        }

        // Ordertable orderTable = _context.Ordertables.FirstOrDefault(orderTable => orderTable.Tableid == tableid && orderTable.Isdeleted == false);
        // Order order = orderTable != null ? _context.Orders.FirstOrDefault(o => o.Orderid == orderTable.Orderid) ?? new Order() : new Order();
        // Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == orderTable.Customerid)!;
        // List<int> itemids = _context.Orderitems.Where(orderedItem => orderedItem.Orderid == order.Orderid).Select(orderedItem => orderedItem.Itemid).ToList();
        // List<int> tableids = _context.Ordertables.Where(table => table.Orderid == order.Orderid).Select(table => table.Tableid).ToList();
        // List<Ordertaxesandfee> appliedTaxAndFees = _context.Ordertaxesandfees.Where(appliedtax => appliedtax.Orderid == order.Orderid).ToList();
        // List<appliedTaxDetails> appliedTaxDetails = new List<appliedTaxDetails>();
        // CustomerModel customermodel = new CustomerModel();
        // OrderDetailsViewModel model = new OrderDetailsViewModel();
        // List<Table> Ordertables = new List<Table>();
        // foreach (var tax in appliedTaxAndFees)
        // {
        //     appliedTaxDetails taxandfees = new appliedTaxDetails();
        //     taxandfees.taxname = tax.Taxname!;
        //     taxandfees.taxPercentage = (float)tax.TaxPercentage!;
        //     taxandfees.taxtype = tax.Taxtype!;
        // }

        // int customerid = 0;
        // int sectionid = 0;

        // foreach (var id in tableids)
        // {
        //     Table table = _context.Tables.FirstOrDefault(table => table.Tableid == id)!;
        //     Ordertables.Add(table);
        //     customerid = table.Customerid ?? customerid;
        //     sectionid = table.Sectionid;
        // }

        // customermodel.tables = Ordertables;
        // customermodel.section = _context.Sections.FirstOrDefault(section => section.Sectionid == sectionid)!;
        // customermodel.customerId = customer.Customerid;
        // customermodel.name = customer.Customername;
        // customermodel.email = customer.Email;
        // customermodel.PersonCount = orderTable.TotalPersonCount ?? 0;
        // customermodel.phone = customer.Phonenumber;
        // customermodel.tableids = tableids;

        // model.tables = Ordertables;
        // model.customerModel = customermodel;
        // model.itemDetails = getItemDetailsModels(itemids, order.Orderid);
        // model.orderid = order.Orderid;
        // model.appliedTaxes = appliedTaxDetails;
        // model.ordercomment = order.Ordercomment ?? "";
        // model.PaymentMethod = order.Paymentmethod;

        // return model;
    }

    public List<ItemDetail> getItemDetailsModels(List<int> itemids, int orderid)
    {
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            var transaction = connection.BeginTransaction();
            using (var command = new NpgsqlCommand("CALL getItemAndModifiersForGivenOrder(@orderid, @categoryid, @is_ready, @result_json)", connection))
            {
                command.Parameters.AddWithValue("orderid", orderid);
                command.Parameters.AddWithValue("categoryid", 0);
                command.Parameters.AddWithValue("is_ready", DBNull.Value);
                var resultParameter = new NpgsqlParameter("result_json", NpgsqlDbType.Json)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = "[]"
                };
                command.Parameters.Add(resultParameter);
                command.ExecuteNonQuery();
                var result = resultParameter.Value.ToString();
                List<OrderItemViewModelProc> itemmodifiersdata = result != null
                    ? JsonConvert.DeserializeObject<List<OrderItemViewModelProc>>(result) ?? new List<OrderItemViewModelProc>()
                    : new List<OrderItemViewModelProc>();
                List<ItemDetail> itemdetailsList = new List<ItemDetail>();
                HashSet<int> uniqueitemids = new HashSet<int>();
                foreach (var data in itemmodifiersdata)
                {
                    ItemDetail itemDetail = new ItemDetail();
                    if (uniqueitemids.Contains(data.orderitemdetailid))
                    {
                        ItemDetail? existingItemDetailObject = itemdetailsList.Find(obj => obj.uniqueid == data.unique_id);
                        if (existingItemDetailObject != null)
                        {
                        }
                        if (data.modifier_id != null)
                        {
                            Modifier newmodifier = new Modifier();
                            newmodifier.Modifierid = data.modifier_id ?? 0;
                            newmodifier.Modifiername = data.modifier_name ?? "";
                            existingItemDetailObject!.modifiers!.Add(newmodifier);
                        }
                    }
                    else
                    {
                        itemDetail.itemId = data.item_id.ToString();
                        Item item = new Item();
                        item.Itemid = data.item_id;
                        item.Itemname = data.item_name;
                        itemDetail.item = item;
                        itemDetail.uniqueid = data.unique_id;
                        itemDetail.itemcomment = data.itemcomment;
                        if (data.modifier_id != null)
                        {
                            Modifier m = new Modifier();
                            m.Modifierid = data.modifier_id ?? 0;
                            m.Modifiername = data.modifier_name ?? "";
                            List<Modifier> modifiers = new List<Modifier>();
                            modifiers.Add(m);
                            itemDetail.modifiers = modifiers;
                        }
                        itemdetailsList.Add(itemDetail);
                    }
                }
                transaction.Commit();
                return itemdetailsList;
            }
        }
        // List<ItemDetail> itemDetails = new List<ItemDetail>();
        // foreach (var itemid in itemids)
        // {
        //     ItemDetail itemDetail = new ItemDetail();
        //     itemDetail.itemId = itemid.ToString();
        //     Item item = _context.Items.FirstOrDefault(item => item.Itemid == itemid)!;
        //     itemDetail.item = item;
        //     Orderitem orderedItem = _context.Orderitems.FirstOrDefault(orderdItem => orderdItem.Orderid == orderid && orderdItem.Itemid == itemid);
        //     itemDetail.itemcomment = orderedItem.Specialcomment ?? "";
        //     int itemQuantity = orderedItem.Orderitemquantity ?? 0;
        //     List<Modifier> modifiersForItem = new List<Modifier>();
        //     int orderedIteDetailId = orderedItem.Orderitemid;
        //     List<int> modifierIds = _context.OrderItemModifiers
        //         .Where(orderedItemModifiers => orderedItemModifiers.Orderitemdetailid == orderedIteDetailId && orderedItemModifiers.Modifierid.HasValue)
        //         .Select(orderedItemModifiers => orderedItemModifiers.Modifierid!.Value)
        //         .ToList();
        //     foreach (var modifierid in modifierIds)
        //     {
        //         Modifier modifier = _context.Modifiers.FirstOrDefault(modifier => modifier.Modifierid == modifierid)!;
        //         modifiersForItem.Add(modifier);
        //     }
        //     itemDetail.modifiers = modifiersForItem;
        //     itemDetails.Add(itemDetail);
        // }
        // return itemDetails;
    }

    public MenuOrderAppModel getAssignedTableDetails(int tableid)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        Ordertable? orderTable = _context.Ordertables.FirstOrDefault(orderTable => orderTable.Tableid == tableid && orderTable.Isdeleted == false);
        List<int> tableids = new List<int>
        {
            tableid
        };
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                string queryForCustomerdetails = "call get_order_details_for_table(@tableid, @result_json)";
                using (var command = new NpgsqlCommand(queryForCustomerdetails, connection, transaction))
                {
                    command.Parameters.AddWithValue("tableid", tableid);
                    var resultParameter = new NpgsqlParameter("result_json", NpgsqlDbType.Json)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "[]"
                    };
                    command.Parameters.Add(resultParameter);
                    command.ExecuteNonQuery();
                    var result = resultParameter.Value.ToString();
                    var customerdatamodel = JsonConvert.DeserializeObject<List<customerOrderDetailsViewModel>>(result!);
                    CustomerModel customermodel = new CustomerModel();
                    if (customerdatamodel != null && customerdatamodel.Count > 0)
                    {
                        customermodel.customerId = customerdatamodel[0].customerid;
                        customermodel.name = customerdatamodel[0].customername;
                        customermodel.email = customerdatamodel[0].email;
                        customermodel.phone = customerdatamodel[0].Phonenumber;
                        customermodel.PersonCount = customerdatamodel[0].personcount;
                        Section section = new Section();
                        section.Sectionid = customerdatamodel[0].sectionid;
                        section.Sectionname = customerdatamodel[0].sectionname;
                        customermodel.section = section;
                        customermodel.tableids = tableids;
                    }
                    model.isTableAssigned = true;
                    model.customer = customermodel;
                    model.categoryId = 0;

                }
                transaction.Commit();
            }

            return model;
        }
        // MenuOrderAppModel model = new MenuOrderAppModel();
        // int customerid = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Tableid == tableid)!.Customerid ?? 0;
        // Ordertable ordertable = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Tableid == tableid && orderedTable.Isdeleted == false)!;
        // CustomerModel customermodel = new CustomerModel();
        // Customer customer = _context.Customers.FirstOrDefault(tableCustomer => tableCustomer.Customerid == ordertable.Customerid);
        // customermodel.customerId = customer.Customerid;
        // customermodel.name = customer.Customername;
        // customermodel.email = customer.Email;
        // Waitingtoken token = _context.Waitingtokens.FirstOrDefault(Token => Token.Customerid == customerid)!;
        // customermodel.PersonCount = ordertable.TotalPersonCount ?? 0;
        // customermodel.phone = customer.Phonenumber;
        // List<int> tableids = new List<int>
        // {
        //     tableid
        // };
        // customermodel.tableids = tableids;
        // model.customer = customermodel;
        // model.isTableAssigned = true;
        // model.categoryId = 0;
        // model.isTableAssigned = true;
        // return model;
    }
    public CustomerModel getcustomerDetails(int customerid, List<int>? tableid)
    {
        CustomerModel model = new CustomerModel();
        model.customerId = customerid;
        Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == customerid)!;
        model.name = customer.Customername;
        model.email = customer.Email;
        model.phone = customer.Phonenumber;
        List<Table> tables = new List<Table>();
        List<int> tableids = new List<int>();
        tableids = _context.Ordertables.Where(orderedTable => orderedTable.Customerid == customerid && orderedTable.Isdeleted == false).Select(orderedTable => orderedTable.Tableid).ToList();
        var maxcapacity = _context.Tables.Where(tables => tableids.Contains(tables.Tableid)).Select(tables => tables.Capacity).Sum();
        if (tableid != null)
        {
            foreach (var id in tableid)
            {
                Table table = _context.Tables.FirstOrDefault(table => table.Tableid == id)!;
                tables.Add(table);
                model.section = _context.Sections.FirstOrDefault(section => section.Sectionid == table.Sectionid)!;
            }
            model.tables = tables;
        }
        model.maxcapacity = maxcapacity;
        return model;
    }

    public void loadOrderedItemsData(int? orderid, MenuOrderAppModel responseModel)
    {
        List<int> itemids = _context.Orderitems
            .Where(orderedItem => orderedItem.Orderid == orderid)
            .Select(orderedItem => orderedItem.Itemid)
            .Distinct()
            .ToList();

        List<int> itemdetailids = new List<int>();
        foreach (var itemid in itemids)
        {
            List<int> CurrentItemDetailIds = _context.Orderitems.Where(Ordereditems => Ordereditems.Itemid == itemid && Ordereditems.Orderid == orderid).Select(Ordereditems => Ordereditems.Orderitemid).ToList();
            itemdetailids.AddRange(CurrentItemDetailIds);
        }
        List<string> uniqueids = new List<string>();

        foreach (var itemid in itemdetailids)
        {
            int orderItemId = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Orderitemid == itemid)!.Itemid;

            List<int> modifierids = _context.OrderItemModifiers
                .Where(orderedItemModifier => orderedItemModifier.Orderitemdetailid == itemid && orderedItemModifier.Orderid == orderid)
                .Select(orderedItemModifier => orderedItemModifier.Modifierid)
                .Where(modifierId => modifierId.HasValue)
                .Select(modifierId => modifierId!.Value)
                .ToList();
            string uniqueid = "item_" + orderItemId + "_";
            foreach (var id in modifierids)
            {
                uniqueid += id + "_";
            }
            uniqueid = uniqueid.TrimEnd('_');

            uniqueids.Add(uniqueid);
        }
        responseModel.uniqueids = uniqueids;
        List<Ordertaxesandfee> taxesOnOrder = _context.Ordertaxesandfees.Where(orderTax => orderTax.Orderid == orderid).ToList();
        List<appliedTaxDetails> appliedTaxes = new List<appliedTaxDetails>();
        foreach (var tax in taxesOnOrder)
        {
            appliedTaxDetails appliedTax = new appliedTaxDetails();
            appliedTax.taxname = tax.Taxname!;
            appliedTax.taxPercentage = (float)tax.TaxPercentage!;
            appliedTax.taxtype = tax.Taxtype!;
            appliedTaxes.Add(appliedTax);
        }
        responseModel.appliedTax = appliedTaxes;
        responseModel.orderComment = _context.Orders.FirstOrDefault(order => order.Orderid == orderid)!.Ordercomment ?? "";
    }

    public void getOrderdItemQuantity(int? orderid, int itemid, MenuOrderAppModel model, List<int> modifiers)
    {
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new NpgsqlCommand("Call get_orderitem_quantities(@orderid,@uniqueid,@result_json)", connection, transaction))
                {
                    string uniqueid = "item_" + itemid + "_";
                    foreach (var id in modifiers)
                    {
                        uniqueid += id + "_";
                    }
                    uniqueid = uniqueid.TrimEnd('_');
                    command.Parameters.AddWithValue("orderid", orderid!);
                    command.Parameters.AddWithValue("uniqueid", uniqueid);
                    var resultParameter = new NpgsqlParameter("result_json", NpgsqlDbType.Json)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = "[]"
                    };
                    command.Parameters.Add(resultParameter);
                    command.ExecuteNonQuery();
                    var result = resultParameter.Value.ToString();
                    JArray array = JArray.Parse(result!);
                    JObject firstItem = (JObject)array[0];
                    model.itemQuantity = (int)firstItem["orderquantity"]!;
                    model.readyQuantity = (int)firstItem["readyquantity"]!;
                    model.itemcomment = (string)firstItem["itemcomment"]!;
                }
            }
            connection.Close();
        }

        // string uniqueid = "item_" + itemid + "_";
        // foreach (var id in modifiers)
        // {
        //     uniqueid += id + "_";
        // }
        // uniqueid = uniqueid.TrimEnd('_');
        // Orderitem? orderedItem = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Uniqueid == uniqueid && orderedItem.Orderid == orderid);
        // if (orderedItem == null)
        // {
        //     model.itemQuantity = 1;
        //     model.itemcomment = "";
        // }
        // else
        // {
        //     model.itemQuantity = orderedItem.Orderitemquantity ?? 1;
        //     model.itemcomment = orderedItem.Specialcomment ?? "";
        //     model.readyQuantity = orderedItem.Readyitemquanitiy ?? 0;
        // }
    }

    public bool completeTheOrder(ItemDetail itemdetails)
    {
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                string query = "CALL complete_order_if_all_ready(@orderid,@success)";
                var command = new NpgsqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("orderid", itemdetails.orderid!);
                var successParameter = new NpgsqlParameter("success", NpgsqlDbType.Boolean)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value
                };
                command.Parameters.Add(successParameter);
                command.ExecuteNonQuery();
                bool success = (bool)successParameter.Value;
                transaction.Commit();
                return success;
            }
        }

        // List<string> orderedItemIds = new List<string>();
        // orderedItemIds = itemdetails.uniqueids;
        // foreach (var itemid in orderedItemIds)
        // {
        //     Orderitem item = _context.Orderitems.FirstOrDefault(Item => Item.Orderid == itemdetails.orderid && Item.Uniqueid == itemid)!;
        //     if (item.Readyitemquanitiy != item.Orderitemquantity)
        //     {
        //         return false;
        //     }
        // }
        // Order order = _context.Orders.FirstOrDefault(currentOrder => currentOrder.Orderid == itemdetails.orderid)!;
        // order.Statusid = 1;
        // order.PaymentStatus = "Completed";
        // List<Ordertable> orderedTables = _context.Ordertables.Where(OrderedTable => OrderedTable.Orderid == itemdetails.orderid).ToList();
        // foreach (var orderedTable in orderedTables)
        // {
        //     orderedTable.Isdeleted = true;
        //     Table table = _context.Tables.FirstOrDefault(currentTable => currentTable.Tableid == orderedTable.Tableid)!;
        //     table.Status = true;
        //     table.Statusname = "Available";
        //     _context.Tables.Update(table);
        // }
        // _context.Orders.Update(order);
        // _context.SaveChanges();
        // return true;
    }

    public bool cancelTheOrder(ItemDetail itemdetails)
    {

        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                string query = "CALL cancel_order(@orderid,@result)";
                var command = new NpgsqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("orderid", itemdetails.orderid!);
                var resultParameter = new NpgsqlParameter("result", NpgsqlDbType.Boolean)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = DBNull.Value
                };
                command.Parameters.Add(resultParameter);
                command.ExecuteNonQuery();
                bool result = (bool)resultParameter.Value;
                transaction.Commit();
                return result;
            }
        }
        // List<string> orderedItemIds = new List<string>();
        // orderedItemIds = itemdetails.uniqueids;
        // foreach (var itemid in orderedItemIds)
        // {
        //     Orderitem item = _context.Orderitems.FirstOrDefault(Item => Item.Orderid == itemdetails.orderid && Item.Uniqueid == itemid)!;
        //     if (item.Readyitemquanitiy > 0)
        //     {
        //         return false;
        //     }
        // }

        // Order order = _context.Orders.FirstOrDefault(currentOrder => currentOrder.Orderid == itemdetails.orderid)!;
        // order.Statusid = 2;
        // List<Ordertable> orderedTables = _context.Ordertables.Where(OrderedTable => OrderedTable.Orderid == itemdetails.orderid).ToList();
        // foreach (var orderedTable in orderedTables)
        // {
        //     orderedTable.Isdeleted = true;
        //     Table table = _context.Tables.FirstOrDefault(currentTable => currentTable.Tableid == orderedTable.Tableid)!;
        //     table.Status = true;
        //     table.Statusname = "Available";
        //     _context.Tables.Update(table);
        // }
        // _context.Orders.Update(order);
        // _context.SaveChanges();
        // return true;
    }
    public void getDashBoardDetails(DashboardViewModel model, int timeId = 1, string toDate = "", string fromDate = "")
    {
        DateTime? from = null;
        DateTime? to = null;
        if (DateTime.TryParse(fromDate, out var parsedFrom))
            from = parsedFrom;
        if (DateTime.TryParse(toDate, out var parsedTo))
            to = parsedTo;
        DateTime now = DateTime.Now;
        var query = _context.Orders.AsQueryable();
        var customerQuery = _context.Customers.AsQueryable();
        var sellingQuery = _context.Orderitems.AsQueryable();
        var waitingTokenQuery = _context.Waitingtokens.AsQueryable();
        DateTime rangeStart = now;
        if (timeId == 2) rangeStart = now.AddDays(-6);
        if (timeId == 3) rangeStart = now.AddDays(-29);
        switch (timeId)
        {
            case 2:
                query = query.Where(h => h.Createdat >= rangeStart);
                customerQuery = customerQuery.Where(h => h.Createdat >= rangeStart);
                sellingQuery = sellingQuery.Where(h => h.Createdat >= rangeStart);
                waitingTokenQuery = waitingTokenQuery.Where(w => w.Createdat >= rangeStart);
                break;
            case 3:
                query = query.Where(h => h.Createdat >= rangeStart);
                customerQuery = customerQuery.Where(h => h.Createdat >= rangeStart);
                sellingQuery = sellingQuery.Where(h => h.Createdat >= rangeStart);
                waitingTokenQuery = waitingTokenQuery.Where(w => w.Createdat >= rangeStart);
                break;
            case 4:
                query = query.Where(h => h.Createdat.HasValue &&
                                         h.Createdat.Value.Month == now.Month &&
                                         h.Createdat.Value.Year == now.Year);
                customerQuery = customerQuery.Where(h => h.Createdat.HasValue &&
                h.Createdat.Value.Month == now.Month &&
                h.Createdat.Value.Year == now.Year);
                sellingQuery = sellingQuery.Where(h => h.Createdat.HasValue &&
                h.Createdat.Value.Month == now.Month &&
                h.Createdat.Value.Year == now.Year);
                waitingTokenQuery = waitingTokenQuery.Where(w => w.Createdat >= rangeStart);
                break;
            case 5:
                if (from.HasValue && to.HasValue)
                {
                    query = query.Where(h => h.Createdat >= from && h.Createdat <= to);
                    customerQuery = customerQuery.Where(h => h.Createdat >= from && h.Createdat <= to);
                    sellingQuery = sellingQuery.Where(h => h.Createdat >= from && h.Createdat <= to);
                    waitingTokenQuery = waitingTokenQuery.Where(w => w.Createdat >= rangeStart && w.Createdat <= to);
                }
                break;
        }
        double totalSales = (double)query.Sum(h => h.Totalamount ?? 0);
        int totalOrders = query.Count();
        int noOfCustomers = customerQuery.Count();
        double avgOrderValue = totalOrders == 0 ? 0 : totalSales / totalOrders;
        var tokens = waitingTokenQuery.ToList();
        TimeSpan averageWaitingTime = TimeSpan.Zero;
        int validTokens = 0;

        foreach (var token in tokens)
        {
            if (token.Modifiedat.HasValue && token.Createdat.HasValue)
            {
                averageWaitingTime += token.Modifiedat.Value - token.Createdat.Value;
                validTokens++;
            }
        }

        if (validTokens > 0)
        {
            averageWaitingTime = TimeSpan.FromTicks(averageWaitingTime.Ticks / validTokens);
        }
        else
        {
            averageWaitingTime = TimeSpan.Zero;
        }

        Dictionary<string, double> dailySales = new();
        Dictionary<string, int> totalCustomers = new();
        if (timeId == 1)
        {
            var allOrders = _context.Orders
                .Where(o => o.Createdat.HasValue);
            var allCustomers = _context.Customers.Where(h => h.Createdat.HasValue);
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var allMonths = Enumerable.Range(1, 12)
                .Select(month => new
                {
                    YearMonth = new DateTime(currentYear, month, 1).ToString("yyyy-MM"),
                    Month = month
                })
                .ToList();
            var salesGroup = allOrders
                .GroupBy(o => o.Createdat!.Value.Month)
                .Select(g => new
                {
                    YearMonth = new DateTime(currentYear, g.Key, 1).ToString("yyyy-MM"),
                    Total = g.Sum(x => x.Totalamount)
                })
                .ToList();
            var customerGroup = allCustomers.GroupBy(h => h.Createdat!.Value.Month).Select(g => new
            {
                YearMonth = new DateTime(currentYear, g.Key, 1).ToString("yyyy-MM"),
                Total = g.Count()
            }).ToList();
            var combinedSales = allMonths
                .Select(month =>
                {
                    var sales = salesGroup.FirstOrDefault(sg => sg.YearMonth == month.YearMonth);
                    return new
                    {
                        YearMonth = month.YearMonth,
                        Total = sales?.Total ?? 0
                    };
                })
                .ToList();
            var combinedCustomer = allMonths.Select(month =>
            {
                var totalCustomersCount = customerGroup.FirstOrDefault(cg => cg.YearMonth == month.YearMonth);
                return new
                {
                    YearMonth = month.YearMonth,
                    totalCustomersCount = totalCustomersCount?.Total
                };
            });
            dailySales = combinedSales
                .ToDictionary(
                    sg => sg.YearMonth,
                    sg => (double)sg.Total
                );
            totalCustomers = combinedCustomer.ToDictionary(cg => cg.YearMonth, cg => cg.totalCustomersCount ?? 0);
        }
        else if (timeId == 2)
        {
            rangeStart = now.Date.AddDays(-6);
            var last7DaysQuery = _context.Orders
                .Where(o => o.Createdat.HasValue
         && o.Createdat.Value.Date >= rangeStart
         && o.Createdat.Value.Date <= now.Date);
            var last7DaysQueryForCustomers = _context.Customers.Where(h => h.Createdat.HasValue
                                                                        && h.Createdat.Value.Date >= rangeStart
                                                                        && h.Createdat.Value.Date <= now.Date);
            var dayNames = Enumerable.Range(0, 7)
                .Select(i => rangeStart.AddDays(i).DayOfWeek.ToString())
                .ToList();
            dailySales = dayNames
                .ToDictionary(day => day, _ => 0.0);
            totalCustomers = dayNames.ToDictionary(day => day, _ => 0);
            var salesGroup = last7DaysQuery
                .GroupBy(o => o.Createdat!.Value.DayOfWeek)
                .Select(g => new
                {
                    DayName = g.Key.ToString(),
                    Total = g.Sum(x => x.Totalamount)
                })
                .ToList();
            var customerGroup = last7DaysQueryForCustomers.GroupBy(h => h.Createdat!.Value.DayOfWeek).Select(g => new
            {
                DayName = g.Key.ToString(),
                Total = g.Count()
            }).ToList();
            foreach (var sg in salesGroup)
                if (dailySales.ContainsKey(sg.DayName))
                    dailySales[sg.DayName] = (double)(sg.Total ?? 0);

            foreach (var cg in customerGroup)
            {
                if (totalCustomers.ContainsKey(cg.DayName))
                {
                    totalCustomers[cg.DayName] = cg.Total;
                }
            }
        }
        else if (timeId == 3)
        {
            var dates = Enumerable.Range(0, 30)
                .Select(i => now.Date.AddDays(-i).ToString("dd"))
                .ToList();
            foreach (var date in dates)
            {
                dailySales[date] = 0;
                totalCustomers[date] = 0;
            }
            var grouped = query
                .Where(h => h.Createdat.HasValue)
                .GroupBy(h => h.Createdat!.Value.Date)
                .Select(g => new { Date = g.Key.ToString("dd"), Total = g.Sum(x => x.Totalamount) })
                .ToList();
            var groupedCustomer = customerQuery
                .Where(h => h.Createdat.HasValue)
                .GroupBy(h => h.Createdat!.Value.Date)
                .Select(g => new { Date = g.Key.ToString("dd"), Total = g.Count() })
                .ToList();

            foreach (var item in grouped)
                dailySales[item.Date] = (double)(item.Total ?? 0);
            foreach (var customer in groupedCustomer)
            {
                totalCustomers[customer.Date] = customer.Total;
            }
        }
        else if (timeId == 4)
        {
            var monthStart = new DateTime(now.Year, now.Month, 1);
            int daysInMonth = (now.Date - monthStart).Days + 1;
            var dateList = Enumerable.Range(0, daysInMonth)
                .Select(i => monthStart.AddDays(i).ToString("dd"))
                .ToList();
            dailySales = dateList.ToDictionary(date => date, _ => 0.0);
            totalCustomers = dateList.ToDictionary(day => day, _ => 0);
            var salesGroup = _context.Orders
                .Where(o => o.Createdat.HasValue
        && o.Createdat.Value.Date >= monthStart
        && o.Createdat.Value.Date <= now.Date)
                .GroupBy(o => o.Createdat!.Value.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd"),
                    Total = g.Sum(x => x.Totalamount)
                })
                .ToList();
            var customerGroup = _context.Customers
                                .Where(o => o.Createdat.HasValue
                                && o.Createdat.Value.Date >= monthStart
                                && o.Createdat.Value.Date <= now.Date)
                                .GroupBy(o => o.Createdat!.Value.Date)
                                .Select(g => new
                                {
                                    Date = g.Key.ToString("dd"),
                                    Total = g.Count()
                                })
                                .ToList();
            foreach (var sg in salesGroup)
            {
                if (dailySales.ContainsKey(sg.Date))
                    dailySales[sg.Date] = (double)(sg.Total ?? 0);
            }
            foreach (var cg in customerGroup)
            {
                if (totalCustomers.ContainsKey(cg.Date))
                {
                    totalCustomers[cg.Date] = cg.Total;
                }
            }
        }
        else if (timeId == 5)
        {
            if (from.HasValue && to.HasValue)
            {
                var range = to.Value - from.Value;
                if (range.Days <= 7)
                {
                    rangeStart = now.Date.AddDays(-6);
                    var last7DaysQuery = _context.Orders
                        .Where(o => o.Createdat.HasValue && o.Createdat.Value.Date >= rangeStart && o.Createdat.Value.Date <= now.Date);
                    var last7DaysQueryForCustomers = _context.Customers
                        .Where(h => h.Createdat.HasValue && h.Createdat.Value.Date >= rangeStart && h.Createdat.Value.Date <= now.Date);

                    var dayNames = Enumerable.Range(0, 7)
                        .Select(i => rangeStart.AddDays(i).DayOfWeek.ToString())
                        .ToList();

                    dailySales = dayNames.ToDictionary(day => day, _ => 0.0);
                    totalCustomers = dayNames.ToDictionary(day => day, _ => 0);
                    var salesGroup = last7DaysQuery
                        .GroupBy(o => o.Createdat!.Value.DayOfWeek)
                        .Select(g => new { DayName = g.Key.ToString(), Total = g.Sum(x => x.Totalamount) })
                        .ToList();
                    var customerGroup = last7DaysQueryForCustomers
                        .GroupBy(h => h.Createdat!.Value.DayOfWeek)
                        .Select(g => new { DayName = g.Key.ToString(), Total = g.Count() })
                        .ToList();

                    foreach (var sg in salesGroup)
                    {
                        if (dailySales.ContainsKey(sg.DayName))
                            dailySales[sg.DayName] = (double)(sg.Total ?? 0);
                    }

                    foreach (var cg in customerGroup)
                    {
                        if (totalCustomers.ContainsKey(cg.DayName))
                            totalCustomers[cg.DayName] = cg.Total;
                    }
                }
                else if (range.Days <= 31)
                {
                    var dates = Enumerable.Range(0, 30)
                        .Select(i => now.Date.AddDays(-i).ToString("dd"))
                        .ToList();
                    foreach (var date in dates)
                    {
                        dailySales[date] = 0;
                        totalCustomers[date] = 0;
                    }

                    var grouped = _context.Orders
                        .Where(o => o.Createdat.HasValue)
                        .GroupBy(o => o.Createdat!.Value.Date)
                        .Select(g => new { Date = g.Key.ToString("dd"), Total = g.Sum(x => x.Totalamount) })
                        .ToList();

                    var groupedCustomer = _context.Customers
                        .Where(h => h.Createdat.HasValue)
                        .GroupBy(h => h.Createdat!.Value.Date)
                        .Select(g => new { Date = g.Key.ToString("dd"), Total = g.Count() })
                        .ToList();

                    foreach (var item in grouped)
                        dailySales[item.Date] = (double)(item.Total ?? 0);

                    foreach (var customer in groupedCustomer)
                    {
                        totalCustomers[customer.Date] = customer.Total;
                    }
                }
                else
                {
                    var allOrders = _context.Orders
            .Where(o => o.Createdat.HasValue);
                    var allCustomers = _context.Customers
                        .Where(h => h.Createdat.HasValue);

                    var currentYear = DateTime.Now.Year;
                    var allMonths = Enumerable.Range(1, 12)
                        .Select(month => new
                        {
                            YearMonth = new DateTime(currentYear, month, 1).ToString("yyyy-MM"),
                            Month = month
                        })
                        .ToList();
                    var salesGroup = allOrders
                        .GroupBy(o => o.Createdat!.Value.Month)
                        .Select(g => new
                        {
                            YearMonth = new DateTime(currentYear, g.Key, 1).ToString("yyyy-MM"),
                            Total = g.Sum(x => x.Totalamount)
                        })
                        .ToList();
                    var customerGroup = allCustomers
                        .GroupBy(h => h.Createdat!.Value.Month)
                        .Select(g => new
                        {
                            YearMonth = new DateTime(currentYear, g.Key, 1).ToString("yyyy-MM"),
                            Total = g.Count()
                        })
                        .ToList();
                    var combinedSales = allMonths
                        .Select(month =>
                        {
                            var sales = salesGroup.FirstOrDefault(sg => sg.YearMonth == month.YearMonth);
                            return new
                            {
                                YearMonth = month.YearMonth,
                                Total = sales?.Total ?? 0
                            };
                        })
                        .ToList();
                    var combinedCustomer = allMonths
                        .Select(month =>
                        {
                            var totalCustomersCount = customerGroup.FirstOrDefault(cg => cg.YearMonth == month.YearMonth);
                            return new
                            {
                                YearMonth = month.YearMonth,
                                totalCustomersCount = totalCustomersCount?.Total ?? 0
                            };
                        })
                        .ToList();
                    dailySales = combinedSales
                        .ToDictionary(sg => sg.YearMonth, sg => (double)sg.Total);

                    totalCustomers = combinedCustomer
                        .ToDictionary(cg => cg.YearMonth, cg => cg.totalCustomersCount);
                }
            }
        }
        var topItems = sellingQuery
    .GroupBy(oi => oi.Itemid)
    .Select(g => new
    {
        ItemId = g.Key,
        OrderCount = g.Select(oi => oi.Orderid).Distinct().Count(),
        TotalQuantity = g.Sum(oi => oi.Orderitemquantity ?? 0)
    })
    .OrderByDescending(x => x.OrderCount)
    .ThenByDescending(x => x.TotalQuantity)
    .Take(2)
    .Join(
        _context.Items,
        g => g.ItemId,
        i => i.Itemid,
        (g, i) => new sellingItemDetail
        {
            item = i,
            OrderCount = g.OrderCount,
            TotalQuantity = g.TotalQuantity
        })
    .ToList();

        var customerCount = customerQuery.ToList().Count();
        var leastSellingItems = sellingQuery
      .GroupBy(oi => oi.Itemid)
      .Select(g => new
      {
          ItemId = g.Key,
          OrderCount = g.Select(oi => oi.Orderid).Distinct().Count(),
          TotalQuantity = g.Sum(oi => oi.Orderitemquantity ?? 0)
      })
      .OrderBy(x => x.OrderCount)
      .ThenBy(x => x.TotalQuantity)
      .Take(2)
      .Join(
          _context.Items,
          g => g.ItemId,
          i => i.Itemid,
          (g, i) => new sellingItemDetail
          {
              item = i,
              OrderCount = g.OrderCount,
              TotalQuantity = g.TotalQuantity
          })
      .ToList();

        var waitingListCount = _context.Waitingtokens.Where(token => token.Isdeleted == false).Count();

        model.totalsales = (float)totalSales;
        model.totalorders = totalOrders;
        model.averageOrderValue = (float)avgOrderValue;
        model.dailySales = dailySales;
        model.totalCustomers = totalCustomers;
        model.noOfCustomers = noOfCustomers;
        model.topSellingItem = topItems;
        model.leastSellingItem = leastSellingItems;
        model.waitingListCount = waitingListCount;
        model.timeid = timeId;
        model.averageWaitingTime = averageWaitingTime;
    }

    public void saveCustomerReview(customerReviewViewModel model)
    {
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MyConnectionString")))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new NpgsqlCommand("CALL savecustomerreview(@orderid,@food,@service,@ambience,@comment)", connection, transaction))
                {
                    command.Parameters.AddWithValue("orderid", model.orderid);
                    command.Parameters.AddWithValue("food", model.food ?? 0);
                    command.Parameters.AddWithValue("service", model.service ?? 0);
                    command.Parameters.AddWithValue("ambience", model.service ?? 0);
                    command.Parameters.AddWithValue("comment", model.comment ?? "");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

        // Orderreview review = new Orderreview();
        // review.Orderid = model.orderid;
        // review.Foodreview = model.food;
        // review.Servicereview = model.service;
        // review.Ambiencereview = model.ambience;
        // if (model.food != 0 && model.service != 0 && model.ambience != 0)
        // {
        //     review.Averagerating = (model.food + model.service + model.ambience) / 3;
        // }
        // review.Createdat = DateTime.Now;
        // review.Comment = model.comment;
        // _context.Orderreviews.Add(review);
        // _context.SaveChanges();
    }
}
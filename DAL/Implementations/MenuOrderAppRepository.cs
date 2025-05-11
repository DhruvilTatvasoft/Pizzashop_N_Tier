


using System.Security.Cryptography;
using DAL.Data;

public class MenuOrderAppRepository : IMenuOrderAppRepository
{
    private readonly PizzashopCContext _context;

    public MenuOrderAppRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public int createOrder(OrderDetailsViewModel orderDetails)
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
            List<string> existingUniqueids = _context.Orderitems.Where(orderedItem => orderedItem.Orderid == orderDetails.orderid).Select(orderedItem => orderedItem.Uniqueid).ToList();
            List<string> currentUniqueIds = orderDetails.uniqueids;
            List<string> itemsToDelete = existingUniqueids.Except(currentUniqueIds ?? new List<string>()).ToList();

            foreach (var id in itemsToDelete)
            {
                int itemDetailId = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Uniqueid == id).Orderitemid;
                Orderitem orderedItem = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Uniqueid == id);
                _context.Orderitems.Remove(orderedItem);
                // _context.OrderItemModifiers.RemoveRange();
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
                    ordertable = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Customerid == orderDetails.customerid)!;
                }
                if (ordertable == null)
                {
                    ordertable.Tableid = tableid;
                    ordertable.Orderid = order.Orderid;
                    ordertable.Isdeleted = false;
                    ordertable.Createdby = 1;
                    ordertable.Modifiedby = 1;
                    ordertable.Customerid = orderDetails.customerid;
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
                orderedItem.Orderitemquantity = int.Parse(item.quantity);
                orderedItem.Specialcomment = item.itemcomment;
                _context.Orderitems.Update(orderedItem);
                itemUpdated = true;
            }
            else
            {
                orderedItem2.Orderid = order.Orderid;
                orderedItem2.Itemid = int.Parse(item.itemId);
                orderedItem2.Orderitemquantity = int.Parse(item.quantity);
                orderedItem2.Createdby = 1;
                orderedItem2.Modifiedby = 1;
                orderedItem2.Specialcomment = item.itemcomment;
                orderedItem2.Readyitemquanitiy = 0;
                _context.Orderitems.Add(orderedItem2);
            }

            _context.SaveChanges();
            if (!itemUpdated)
            {
                foreach (var modifierid in item.modifierIds)
                {
                    OrderItemModifier? itemModifier = new OrderItemModifier();
                    itemModifier.ItemId = int.Parse(item.itemId);
                    itemModifier.Modifierid = int.Parse(modifierid);
                    itemModifier.ModifierQuantity = int.Parse(item.quantity);
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
        return order.Orderid;
    }

    public Item getItem(int itemid)
    {
        return _context.Items.FirstOrDefault(item => item.Itemid == itemid)!;
    }

    public List<Item> getItemsForcategory(int categoryid, string ItemType, string searchedItem)
    {
        bool? isVeg = null;
        if (!string.IsNullOrEmpty(ItemType))
        {
            if (ItemType.ToLower() == "veg")
                isVeg = true;
            else if (ItemType.ToLower() == "non-veg")
                isVeg = false;
        }


        var query = _context.Items.AsQueryable();


        query = query.Where(item => item.Isdeleted == false);


        if (categoryid != 0)
        {
            query = query.Where(item => item.Categoryid == categoryid);
        }


        if (!string.IsNullOrEmpty(searchedItem))
        {
            string lowerSearch = searchedItem.ToLower().Trim();
            query = query.Where(item => item.Itemname.ToLower().Trim().Contains(lowerSearch));
        }


        if (isVeg.HasValue)
        {
            query = query.Where(item => item.Itemtype == isVeg.Value);
        }

        return query.ToList();
    }


    public List<ModifierModel> getModifiersForItem(int itemid)
    {
        List<ModifierModel> model = new List<ModifierModel>();
        List<Itemsandmodifier> itemsandmodifiers = _context.Itemsandmodifiers.Where(x => x.Itemid == itemid && x.Isdeleted == false).ToList();
        foreach (var itemmodifier in itemsandmodifiers)
        {
            ModifierModel modifierModel = new ModifierModel();
            modifierModel.modifiergroup = _context.Modifiergroups.FirstOrDefault(mg => mg.Modifiergroupid == itemmodifier.Modifiergroupid && mg.Isdeleted == false);
            if (modifierModel.modifiergroup != null)
            {
                List<Modifier> modifiers = _context.Modifiers.Where(modifier => modifier.Modifiergroupid == itemmodifier.Modifiergroupid && modifier.Isdeleted == false).ToList();
                if (modifiers.Count == 0)
                {
                    modifierModel.modifiers = new List<Modifier>();
                }
                else
                {
                    modifierModel.modifiers = modifiers;
                }
                modifierModel.min_value = itemmodifier.Requiredminselection;
                modifierModel.max_value = itemmodifier.Allowedmaxselection;
                model.Add(modifierModel);
            }
        }
        return model;
    }

    public Order getOrderfromOrderid(int orderId)
    {
        return _context.Orders.FirstOrDefault(order => order.Orderid == orderId)!;
    }



    public void saveCustomerDetails(CustomerModel customer)
    {
        Customer ExistingCustomer = _context.Customers.FirstOrDefault(Customer => Customer.Customerid == customer.customerId)!;
        ExistingCustomer.Customername = customer.name;
        ExistingCustomer.Email = customer.email;
        ExistingCustomer.Phonenumber = customer.phone;
        ExistingCustomer.Modifiedat = DateTime.Now;
        Waitingtoken token = _context!.Waitingtokens.FirstOrDefault(token => token.Customerid == customer.customerId && token.Isdeleted == false)!;
        token.Totalpersons = customer.PersonCount;
        _context.Customers.Update(ExistingCustomer);
        _context.Waitingtokens.Update(token);
        _context.SaveChanges();
    }

    public void saveOrderWiseComment(MenuOrderAppModel model)
    {
        Order order = _context.Orders.FirstOrDefault(Order => Order.Orderid == model.orderid);
        order.Ordercomment = model.order.Ordercomment;
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public MenuOrderAppModel getRunningTableOrder(int tableid)
    {
        OrderDetailsViewModel model = new OrderDetailsViewModel();
        int orderid = _context.Ordertables.FirstOrDefault(orderTable => orderTable.Tableid == tableid && orderTable.Isdeleted == false)?.Orderid ?? 0;
        List<int> itemids = _context.Orderitems.Where(orderedItem => orderedItem.Orderid == orderid).Select(orderedItem => orderedItem.Itemid).ToList();
        List<Item> items = new List<Item>();
        Order order = _context.Orders.FirstOrDefault(order => order.Orderid == orderid)!;
        List<ItemDetail> itemDetails = new List<ItemDetail>();
        CustomerModel customermodel = new CustomerModel();
        foreach (var itemid in itemids)
        {
            // loading item
            ItemDetail itemDetail = new ItemDetail();
            itemDetail.itemId = itemid.ToString();
            Item item = _context.Items.FirstOrDefault(item => item.Itemid == itemid)!;
            itemDetail.item = item;
            itemDetail.itemcomment = _context.Orderitems.FirstOrDefault(orderdItem => orderdItem.Orderid == orderid && orderdItem.Itemid == itemid)!.Specialcomment ?? "";
            int itemQuantity = _context.Orderitems.FirstOrDefault(Orderitems => Orderitems.Orderid == orderid && Orderitems.Itemid == itemid)?.Orderitemquantity ?? 0;

            // loading modifiers for item
            List<Modifier> modifiersForItem = new List<Modifier>();
            int orderedIteDetailId = _context.Orderitems.FirstOrDefault(orderedIteDetail => orderedIteDetail.Itemid == itemid && orderedIteDetail.Orderid == orderid)!.Orderitemid;
            List<int> modifierIds = _context.OrderItemModifiers
                .Where(orderedItemModifiers => orderedItemModifiers.Orderitemdetailid == orderedIteDetailId && orderedItemModifiers.Modifierid.HasValue)
                .Select(orderedItemModifiers => orderedItemModifiers.Modifierid!.Value)
                .ToList();
            foreach (var modifierid in modifierIds)
            {
                Modifier modifier = _context.Modifiers.FirstOrDefault(modifier => modifier.Modifierid == modifierid)!;
                modifiersForItem.Add(modifier);
            }
            itemDetail.modifiers = modifiersForItem;
            itemDetails.Add(itemDetail);
        }
        model.itemDetails = itemDetails;

        // loading applied tax and fees
        List<Ordertaxesandfee> appliedTaxAndFees = _context.Ordertaxesandfees.Where(appliedtax => appliedtax.Orderid == orderid).ToList();
        List<appliedTaxDetails> appliedTaxDetails = new List<appliedTaxDetails>();
        foreach (var tax in appliedTaxAndFees)
        {
            appliedTaxDetails taxandfees = new appliedTaxDetails();
            taxandfees.taxname = tax.Taxname!;
            taxandfees.taxPercentage = (float)tax.TaxPercentage!;
            taxandfees.taxtype = tax.Taxtype!;
        }
        model.appliedTaxes = appliedTaxDetails;

        // loading tables
        List<int> tableids = _context.Ordertables.Where(table => table.Orderid == orderid).Select(table => table.Tableid).ToList();
        List<Table> Ordertables = new List<Table>();
        int customerid = 0;
        int sectionid = 0;
        foreach (var id in tableids)
        {
            Table table = _context.Tables.FirstOrDefault(table => table.Tableid == id)!;
            Ordertables.Add(table);
            customerid = table.Customerid ?? customerid;
            sectionid = table.Sectionid;
        }
        model.tables = Ordertables;
        customermodel.tables = Ordertables;
        customermodel.section = _context.Sections.FirstOrDefault(section => section.Sectionid == sectionid)!;

        // loading customerdetails
        Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == customerid)!;
        customermodel.customerId = customer.Customerid;
        customermodel.name = customer.Customername;
        customermodel.email = customer.Email;
        customermodel.PersonCount = _context.Orders.FirstOrDefault(order => order.Orderid == orderid).Totalpersons;
        customermodel.phone = customer.Phonenumber;
        model.customerModel = customermodel;
        customermodel.tableids = tableids;

        //loading order comment and payment method
        model.ordercomment = order.Ordercomment ?? "";
        model.PaymentMethod = order.Paymentmethod;
        MenuOrderAppModel Model = new MenuOrderAppModel();
        Model.orderDetailModel = model;
        Model.customer = customermodel;
        Model.isTableAssigned = true;
        Model.orderid = orderid;
        Model.orderComment = order.Ordercomment;
        return Model;
    }

    public MenuOrderAppModel getAssignedTableDetails(int tableid)
    {
        MenuOrderAppModel model = new MenuOrderAppModel();
        int customerid = _context.Ordertables.FirstOrDefault(orderedTable => orderedTable.Tableid == tableid)!.Customerid ?? 0;
        CustomerModel customermodel = new CustomerModel();
        Customer customer = _context.Customers.FirstOrDefault(tableCustomer => tableCustomer.Customerid == customerid);
        customermodel.customerId = customer.Customerid;
        customermodel.name = customer.Customername;
        customermodel.email = customer.Email;
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(Token => Token.Customerid == customerid)!;
        customermodel.PersonCount = token.Totalpersons;
        customermodel.phone = customer.Phonenumber;
        List<int> tableids = new List<int>
        {
            tableid
        };
        customermodel.tableids = tableids;
        model.customer = customermodel;
        model.isTableAssigned = true;
        model.tokenid = token.Waitingtokenid;
        model.categoryId = 0;
        model.isTableAssigned = true;
        return model;
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
                .Select(modifierId => modifierId.Value)
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
        string uniqueid = "item_" + itemid + "_";
        foreach (var id in modifiers)
        {
            uniqueid += id + "_";
        }
        uniqueid = uniqueid.TrimEnd('_');
        Orderitem? orderedItem = _context.Orderitems.FirstOrDefault(orederedItem => orederedItem.Uniqueid == uniqueid);
        if (orderedItem == null)
        {
            model.itemQuantity = 1;
            model.itemcomment = "";
        }
        else
        {
            model.itemQuantity = orderedItem.Orderitemquantity ?? 1;
            model.itemcomment = orderedItem.Specialcomment ?? "";
            model.readyQuantity = orderedItem.Readyitemquanitiy ?? 0;

        }
    }

    public bool completeTheOrder(ItemDetail itemdetails)
    {
        List<string> orderedItemIds = new List<string>();
        orderedItemIds = itemdetails.uniqueids;
        foreach (var itemid in orderedItemIds)
        {
            Orderitem item = _context.Orderitems.FirstOrDefault(Item => Item.Orderid == itemdetails.orderid && Item.Uniqueid == itemid)!;
            if (item.Readyitemquanitiy < 1)
            {
                return false;
            }
        }
        Order order = _context.Orders.FirstOrDefault(currentOrder => currentOrder.Orderid == itemdetails.orderid)!;
        order.Statusid = 1;
        order.PaymentStatus = "Completed";
        Ordertable orderedTable = _context.Ordertables.FirstOrDefault(OrderedTable => OrderedTable.Orderid == itemdetails.orderid)!;
        orderedTable.Isdeleted = true;
        Table table = _context.Tables.FirstOrDefault(currentTable => currentTable.Tableid == orderedTable.Tableid)!;
        table.Status = true;
        table.Statusname = "Available";
        _context.Tables.Update(table);
        _context.Orders.Update(order);
        _context.SaveChanges();
        return true;
    }

    public bool cancelTheOrder(ItemDetail itemdetails)
    {
        List<string> orderedItemIds = new List<string>();
        orderedItemIds = itemdetails.uniqueids;
        foreach (var itemid in orderedItemIds)
        {
            Orderitem item = _context.Orderitems.FirstOrDefault(Item => Item.Orderid == itemdetails.orderid && Item.Uniqueid == itemid)!;
            if (item.Readyitemquanitiy > 0  )
            {
                return false;
            }
        }

        Order order = _context.Orders.FirstOrDefault(currentOrder => currentOrder.Orderid == itemdetails.orderid)!;
        order.Statusid = 2;
        Ordertable orderedTable = _context.Ordertables.FirstOrDefault(OrderedTable => OrderedTable.Orderid == itemdetails.orderid)!;
        orderedTable.Isdeleted = true;
        Table table = _context.Tables.FirstOrDefault(currentTable => currentTable.Tableid == orderedTable.Tableid)!;
        table.Status = true;
        table.Statusname = "Available";
        _context.Tables.Update(table);
        _context.Orders.Update(order);
        _context.SaveChanges();
        return true;
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

        DateTime rangeStart = now;
        if (timeId == 2) rangeStart = now.AddDays(-6);
        if (timeId == 3) rangeStart = now.AddDays(-29);
        switch (timeId)
        {
            case 2:
                query = query.Where(h => h.Createdat >= rangeStart);
                customerQuery = customerQuery.Where(h => h.Createdat >= rangeStart);
                sellingQuery = sellingQuery.Where(h => h.Createdat >= rangeStart);
                break;
            case 3:
                query = query.Where(h => h.Createdat >= rangeStart);
                customerQuery = customerQuery.Where(h => h.Createdat >= rangeStart);
                sellingQuery = sellingQuery.Where(h => h.Createdat >= rangeStart);
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
                break;
            case 5:
                if (from.HasValue && to.HasValue)
                {
                    query = query.Where(h => h.Createdat >= from && h.Createdat <= to);
                    customerQuery = customerQuery.Where(h => h.Createdat >= from && h.Createdat <= to);
                    sellingQuery = sellingQuery.Where(h => h.Createdat >= from && h.Createdat <= to);
                }
                break;
        }
        double totalSales = (double)query.Sum(h => h.Totalamount);
        int totalOrders = query.Count();
        int noOfCustomers = customerQuery.Count();
        double avgOrderValue = totalOrders == 0 ? 0 : totalSales / totalOrders;
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
                    dailySales[sg.DayName] = (double)sg.Total;

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
                dailySales[item.Date] = (double)item.Total;
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
                    dailySales[sg.Date] = (double)sg.Total;
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
                            dailySales[sg.DayName] = (double)sg.Total;
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
                        dailySales[item.Date] = (double)item.Total;

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

        var waitingListCount = _context.Waitingtokens.Count();

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
    }
}
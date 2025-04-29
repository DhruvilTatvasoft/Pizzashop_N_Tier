using System.Reflection.Metadata.Ecma335;
using DAL.Data;
using Microsoft.IdentityModel.Tokens;

public class MenuOrderAppRepository : IMenuOrderAppRepository
{
    private readonly PizzashopCContext _context;

    public MenuOrderAppRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public bool createOrder(OrderDetailsViewModel orderDetails)
    {
        Order order = new Order();
       order.Customerid = orderDetails.customerid;
       if(orderDetails.ordercomment != "" || orderDetails.ordercomment != null)
       order.Ordercomment = orderDetails.ordercomment;
       order.Totalamount = (decimal?)orderDetails.totalamount;
       order.PaymentStatus = "Pending";
       order.Statusid = 4;
       order.Sectionid = orderDetails.sectionid;
       order.Totalpersons = orderDetails.totalPersons;
       order.Paymentmethod = orderDetails.PaymentMethod;
       order.Createdat = DateTime.Now;
       order.Createdby = 1;
       order.Modifiedby = 1;
       order.IsDeleted = false; 
       _context.Orders.Add(order);
       _context.SaveChanges();

       foreach(var tableid in orderDetails.tableids){
        Ordertable ordertable = new Ordertable();
        ordertable.Tableid = tableid;
        ordertable.Orderid = order.Orderid;
        ordertable.Isdeleted = false;
        // Table table = _context.Tables.FirstOrDefault(tbl=>tbl.Tableid == tableid)!;
        // table.Status = false;
        // table.Statusname = "Running";
        // _context.Tables.Update(table);
        ordertable.Createdby = 1;
        ordertable.Modifiedby = 1;
        ordertable.Customerid = orderDetails.customerid;    
        _context.Ordertables.Add(ordertable);
       }

        foreach(var item in orderDetails.itemDetails)
        {
            Orderitem orderedItem = new Orderitem();
            orderedItem.Orderid = order.Orderid;
            orderedItem.Itemid = int.Parse(item.itemId);
            orderedItem.Orderitemquantity = int.Parse(item.quantity);
            orderedItem.Createdby = 1;
            orderedItem.Modifiedby = 1;
            orderedItem.Specialcomment = item.itemcomment;
            _context.Orderitems.Add(orderedItem);
            _context.SaveChanges();
            foreach(var modifierid in item.modifierIds){
                OrderItemModifier itemModifier = new OrderItemModifier();
                itemModifier.ItemId = int.Parse(item.itemId);
                itemModifier.Modifierid = int.Parse(modifierid);
                itemModifier.ModifierQuantity = int.Parse(item.quantity);
                itemModifier.Orderitemdetailid = orderedItem.Orderitemid;
                itemModifier.Orderid = order.Orderid;
                _context.OrderItemModifiers.Add(itemModifier);
                _context.SaveChanges();
            }
        }

        foreach(var tax in orderDetails.appliedTaxes){
            Ordertaxesandfee taxAndFees = new Ordertaxesandfee();
            taxAndFees.Orderid = order.Orderid;
            taxAndFees.Taxname = tax.taxname;
            taxAndFees.TaxPercentage = (decimal?)float.Parse(tax.taxPercentage.ToString());
            taxAndFees.Taxtype = tax.taxtype;
            _context.Ordertaxesandfees.Add(taxAndFees);
        }
       _context.SaveChanges();
       return true;

    }

    public Item getItem(int itemid)
    {
        return _context.Items.FirstOrDefault(item => item.Itemid == itemid)!;
    }

    public List<Item> getItemsForcategory(int categoryid, string searchedItem)
    {
        if (searchedItem != "")
        {
            if (categoryid != 0)
            {
                return _context.Items.Where(item => item.Itemname.ToLower().Trim().Contains(searchedItem.ToLower().Trim()) && item.Isdeleted == false && item.Categoryid == categoryid).ToList();
            }
            else
            {
                return _context.Items.Where(item => item.Itemname.ToLower().Trim().Contains(searchedItem.ToLower().Trim()) && item.Isdeleted == false).ToList();
            }
        }
        if (categoryid == 0)
        {
            List<Category> categories = _context.Categories.Where(category => category.Isdeleted == false).ToList();
            List<Item> items = new List<Item>();
            foreach (var category in categories)
            {
                var itemsInCategory = _context.Items.Where(item => item.Categoryid == category.Categoryid && item.Isdeleted == false).ToList();
                items.AddRange(itemsInCategory);
            }
            return items;
        }
        else
        {
            return _context.Items.Where(item => item.Categoryid == categoryid && item.Isdeleted == false).ToList();
        }
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
        return _context.Orders.FirstOrDefault(order=>order.Orderid == orderId)!;
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
        Order order = _context.Orders.FirstOrDefault(Order=>Order.Orderid == model.orderid);
        order.Ordercomment = model.order.Ordercomment;
        _context.Orders.Update(order);
        _context.SaveChanges();
    }
}
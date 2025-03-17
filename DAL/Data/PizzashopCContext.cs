using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class PizzashopCContext : DbContext
{
    public PizzashopCContext()
    {
    }

    public PizzashopCContext(DbContextOptions<PizzashopCContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Itemsandmodifier> Itemsandmodifiers { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<Modifiergroup> Modifiergroups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Ordermodifier> Ordermodifiers { get; set; }

    public virtual DbSet<Orderreview> Orderreviews { get; set; }

    public virtual DbSet<Ordertable> Ordertables { get; set; }

    public virtual DbSet<Ordertaxesandfee> Ordertaxesandfees { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolesandpermission> Rolesandpermissions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Taxesandfee> Taxesandfees { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Useraccount> Useraccounts { get; set; }

    public virtual DbSet<Waitingtoken> Waitingtokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost,5432;Database=Pizzashop_c;User id=postgres;Password=Dhruvil@23;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(50)
                .HasColumnName("categoryname");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("cities_pkey");

            entity.ToTable("cities");

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Cityname)
                .HasMaxLength(30)
                .HasColumnName("cityname");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Stateid).HasColumnName("stateid");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.Stateid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cities_stateid_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Countryname)
                .HasMaxLength(30)
                .HasColumnName("countryname");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Customername)
                .HasMaxLength(50)
                .HasColumnName("customername");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Invoiceid).HasName("invoices_pkey");

            entity.ToTable("invoices");

            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Orderitemid).HasColumnName("orderitemid");
            entity.Property(e => e.Ordermodifierid).HasColumnName("ordermodifierid");

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoices_orderid_fkey");

            entity.HasOne(d => d.Orderitem).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Orderitemid)
                .HasConstraintName("invoices_orderitemid_fkey");

            entity.HasOne(d => d.Ordermodifier).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.Ordermodifierid)
                .HasConstraintName("invoices_ordermodifierid_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("items_pkey");

            entity.ToTable("items");

            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Isavailable).HasColumnName("isavailable");
            entity.Property(e => e.Isdefaulttax).HasColumnName("isdefaulttax");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isfavourite).HasColumnName("isfavourite");
            entity.Property(e => e.Itemimage)
                .HasMaxLength(256)
                .HasColumnName("itemimage");
            entity.Property(e => e.Itemname)
                .HasMaxLength(50)
                .HasColumnName("itemname");
            entity.Property(e => e.Itemquantity).HasColumnName("itemquantity");
            entity.Property(e => e.Itemrate)
                .HasPrecision(10, 2)
                .HasColumnName("itemrate");
            entity.Property(e => e.Itemtype).HasColumnName("itemtype");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Shortcode)
                .HasMaxLength(50)
                .HasColumnName("shortcode");
            entity.Property(e => e.Taxpercentage)
                .HasPrecision(3)
                .HasColumnName("taxpercentage");
            entity.Property(e => e.Unitid).HasColumnName("unitid");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_categoryid_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Items)
                .HasForeignKey(d => d.Unitid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("items_unitid_fkey");
        });

        modelBuilder.Entity<Itemsandmodifier>(entity =>
        {
            entity.HasKey(e => e.Itemsandmodifiersid).HasName("itemsandmodifiers_pkey");

            entity.ToTable("itemsandmodifiers");

            entity.Property(e => e.Itemsandmodifiersid).HasColumnName("itemsandmodifiersid");
            entity.Property(e => e.Allowedmaxselection).HasColumnName("allowedmaxselection");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiergroupid).HasColumnName("modifiergroupid");
            entity.Property(e => e.Requiredminselection).HasColumnName("requiredminselection");

            entity.HasOne(d => d.Item).WithMany(p => p.Itemsandmodifiers)
                .HasForeignKey(d => d.Itemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itemsandmodifiers_itemid_fkey");

            entity.HasOne(d => d.Modifiergroup).WithMany(p => p.Itemsandmodifiers)
                .HasForeignKey(d => d.Modifiergroupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itemsandmodifiers_modifiergroupid_fkey");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_pkey");

            entity.ToTable("login");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.Modifierid).HasName("modifiers_pkey");

            entity.ToTable("modifiers");

            entity.Property(e => e.Modifierid).HasColumnName("modifierid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiergroupid).HasColumnName("modifiergroupid");
            entity.Property(e => e.Modifiername)
                .HasMaxLength(50)
                .HasColumnName("modifiername");
            entity.Property(e => e.Modifierquantity).HasColumnName("modifierquantity");
            entity.Property(e => e.Modifierrate)
                .HasPrecision(10, 2)
                .HasColumnName("modifierrate");
            entity.Property(e => e.Unitid).HasColumnName("unitid");

            entity.HasOne(d => d.Modifiergroup).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.Modifiergroupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifiers_modifiergroupid_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.Unitid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifiers_unitid_fkey");
        });

        modelBuilder.Entity<Modifiergroup>(entity =>
        {
            entity.HasKey(e => e.Modifiergroupid).HasName("modifiergroups_pkey");

            entity.ToTable("modifiergroups");

            entity.Property(e => e.Modifiergroupid).HasColumnName("modifiergroupid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiergroupname)
                .HasMaxLength(50)
                .HasColumnName("modifiergroupname");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Issuggesionselected).HasColumnName("issuggesionselected");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Ordercomment)
                .HasMaxLength(256)
                .HasColumnName("ordercomment");
            entity.Property(e => e.Ordernumber).HasColumnName("ordernumber");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(20)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Subtotalamount)
                .HasPrecision(10, 2)
                .HasColumnName("subtotalamount");
            entity.Property(e => e.Taxamount).HasColumnName("taxamount");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");
            entity.Property(e => e.Totalpersons).HasColumnName("totalpersons");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_customerid_fkey");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Orderitemid).HasName("orderitems_pkey");

            entity.ToTable("orderitems");

            entity.Property(e => e.Orderitemid).HasColumnName("orderitemid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isready)
                .HasDefaultValueSql("false")
                .HasColumnName("isready");
            entity.Property(e => e.Itemid).HasColumnName("itemid");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Orderitemname)
                .HasMaxLength(50)
                .HasColumnName("orderitemname");
            entity.Property(e => e.Orderitemquantity).HasColumnName("orderitemquantity");
            entity.Property(e => e.Orderitemrate)
                .HasPrecision(10, 2)
                .HasColumnName("orderitemrate");
            entity.Property(e => e.Readyitemquanitiy).HasColumnName("readyitemquanitiy");
            entity.Property(e => e.Specialcomment)
                .HasMaxLength(256)
                .HasColumnName("specialcomment");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");

            entity.HasOne(d => d.Item).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Itemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderitems_itemid_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderitems_orderid_fkey");
        });

        modelBuilder.Entity<Ordermodifier>(entity =>
        {
            entity.HasKey(e => e.Ordermodifierid).HasName("ordermodifiers_pkey");

            entity.ToTable("ordermodifiers");

            entity.Property(e => e.Ordermodifierid).HasColumnName("ordermodifierid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifierid).HasColumnName("modifierid");
            entity.Property(e => e.Orderitemid).HasColumnName("orderitemid");
            entity.Property(e => e.Ordermodifiername)
                .HasMaxLength(50)
                .HasColumnName("ordermodifiername");
            entity.Property(e => e.Ordermodifierquantity).HasColumnName("ordermodifierquantity");
            entity.Property(e => e.Ordermodifierrate)
                .HasPrecision(10, 2)
                .HasColumnName("ordermodifierrate");
            entity.Property(e => e.Totalamount)
                .HasPrecision(10, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Modifier).WithMany(p => p.Ordermodifiers)
                .HasForeignKey(d => d.Modifierid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordermodifiers_modifierid_fkey");

            entity.HasOne(d => d.Orderitem).WithMany(p => p.Ordermodifiers)
                .HasForeignKey(d => d.Orderitemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordermodifiers_orderitemid_fkey");
        });

        modelBuilder.Entity<Orderreview>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("orderreviews_pkey");

            entity.ToTable("orderreviews");

            entity.Property(e => e.Reviewid).HasColumnName("reviewid");
            entity.Property(e => e.Ambiencereview).HasColumnName("ambiencereview");
            entity.Property(e => e.Averagerating).HasColumnName("averagerating");
            entity.Property(e => e.Comment)
                .HasMaxLength(256)
                .HasColumnName("comment");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Foodreview).HasColumnName("foodreview");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Servicereview).HasColumnName("servicereview");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderreviews)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderreviews_orderid_fkey");
        });

        modelBuilder.Entity<Ordertable>(entity =>
        {
            entity.HasKey(e => e.Ordertableid).HasName("ordertables_pkey");

            entity.ToTable("ordertables");

            entity.Property(e => e.Ordertableid).HasColumnName("ordertableid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Tableid).HasColumnName("tableid");
            entity.Property(e => e.Totalpersonsoftable).HasColumnName("totalpersonsoftable");

            entity.HasOne(d => d.Order).WithMany(p => p.Ordertables)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordertables_orderid_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Ordertables)
                .HasForeignKey(d => d.Tableid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordertables_tableid_fkey");
        });

        modelBuilder.Entity<Ordertaxesandfee>(entity =>
        {
            entity.HasKey(e => e.Ordertaxid).HasName("ordertaxesandfees_pkey");

            entity.ToTable("ordertaxesandfees");

            entity.Property(e => e.Ordertaxid).HasColumnName("ordertaxid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Orderitemid).HasColumnName("orderitemid");
            entity.Property(e => e.Ordertaxamount)
                .HasPrecision(10, 2)
                .HasColumnName("ordertaxamount");
            entity.Property(e => e.Ordertaxname)
                .HasColumnType("character varying")
                .HasColumnName("ordertaxname");
            entity.Property(e => e.Taxid).HasColumnName("taxid");

            entity.HasOne(d => d.Orderitem).WithMany(p => p.Ordertaxesandfees)
                .HasForeignKey(d => d.Orderitemid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordertaxesandfees_orderitemid_fkey");

            entity.HasOne(d => d.Tax).WithMany(p => p.Ordertaxesandfees)
                .HasForeignKey(d => d.Taxid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordertaxesandfees_taxid_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Invoiceid).HasColumnName("invoiceid");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Paymentmethod).HasColumnName("paymentmethod");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Invoiceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_invoiceid_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Permissionid).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Permissionname)
                .HasMaxLength(50)
                .HasColumnName("permissionname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Rolesandpermission>(entity =>
        {
            entity.HasKey(e => e.Rolesandpermissionid).HasName("rolesandpermissions_pkey");

            entity.ToTable("rolesandpermissions");

            entity.Property(e => e.Rolesandpermissionid).HasColumnName("rolesandpermissionid");
            entity.Property(e => e.Candelete).HasColumnName("candelete");
            entity.Property(e => e.Canedit).HasColumnName("canedit");
            entity.Property(e => e.Canview).HasColumnName("canview");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Permission).WithMany(p => p.Rolesandpermissions)
                .HasForeignKey(d => d.Permissionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolesandpermissions_permissionid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolesandpermissions)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolesandpermissions_roleid_fkey");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("sections_pkey");

            entity.ToTable("sections");

            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Sectionname)
                .HasMaxLength(50)
                .HasColumnName("sectionname");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("states_pkey");

            entity.ToTable("states");

            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Statename)
                .HasMaxLength(30)
                .HasColumnName("statename");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.Countryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("states_countryid_fkey");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Tableid).HasName("tables_pkey");

            entity.ToTable("tables");

            entity.Property(e => e.Tableid).HasColumnName("tableid");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tablename)
                .HasMaxLength(50)
                .HasColumnName("tablename");

            entity.HasOne(d => d.Section).WithMany(p => p.Tables)
                .HasForeignKey(d => d.Sectionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tables_sectionid_fkey");
        });

        modelBuilder.Entity<Taxesandfee>(entity =>
        {
            entity.HasKey(e => e.Taxid).HasName("taxesandfees_pkey");

            entity.ToTable("taxesandfees");

            entity.Property(e => e.Taxid).HasColumnName("taxid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdefault)
                .HasDefaultValueSql("true")
                .HasColumnName("isdefault");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isenabled)
                .HasDefaultValueSql("true")
                .HasColumnName("isenabled");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Taxname)
                .HasMaxLength(20)
                .HasColumnName("taxname");
            entity.Property(e => e.Taxpercentage)
                .HasPrecision(10, 2)
                .HasColumnName("taxpercentage");
            entity.Property(e => e.Taxtype).HasColumnName("taxtype");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Unitid).HasName("units_pkey");

            entity.ToTable("units");

            entity.Property(e => e.Unitid).HasColumnName("unitid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Shortname)
                .HasMaxLength(20)
                .HasColumnName("shortname");
            entity.Property(e => e.Unitname)
                .HasMaxLength(50)
                .HasColumnName("unitname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Address)
                .HasMaxLength(256)
                .HasColumnName("address");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Profilephoto)
                .HasMaxLength(256)
                .HasColumnName("profilephoto");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(50)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.Cityid)
                .HasConstraintName("users_cityid_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.Countryid)
                .HasConstraintName("users_countryid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_roleid_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Users)
                .HasForeignKey(d => d.Stateid)
                .HasConstraintName("users_stateid_fkey");
        });

        modelBuilder.Entity<Useraccount>(entity =>
        {
            entity.HasKey(e => e.Accountid).HasName("useraccounts_pkey");

            entity.ToTable("useraccounts");

            entity.HasIndex(e => e.Email, "useraccounts_email_key").IsUnique();

            entity.Property(e => e.Accountid).HasColumnName("accountid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isfirstlogin).HasColumnName("isfirstlogin");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Role).WithMany(p => p.Useraccounts)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("useraccounts_roleid_fkey");
        });

        modelBuilder.Entity<Waitingtoken>(entity =>
        {
            entity.HasKey(e => e.Waitingtokenid).HasName("waitingtokens_pkey");

            entity.ToTable("waitingtokens");

            entity.Property(e => e.Waitingtokenid).HasColumnName("waitingtokenid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Isassigned)
                .HasDefaultValueSql("false")
                .HasColumnName("isassigned");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Sectionid).HasColumnName("sectionid");
            entity.Property(e => e.Tableid).HasColumnName("tableid");
            entity.Property(e => e.Totalpersons).HasColumnName("totalpersons");

            entity.HasOne(d => d.Section).WithMany(p => p.Waitingtokens)
                .HasForeignKey(d => d.Sectionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("waitingtokens_sectionid_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Waitingtokens)
                .HasForeignKey(d => d.Tableid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("waitingtokens_tableid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

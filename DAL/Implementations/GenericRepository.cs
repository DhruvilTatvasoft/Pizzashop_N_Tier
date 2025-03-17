using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

public class GenericRepository : IGenericRepository
{
    public PizzashopCContext _context;

    public GenericRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public void changePass(chang_p_model model, string email, string pass)
    {
        var logger = _context.Logins.Where(lg => lg.Email == email).FirstOrDefault();
        logger.Password = pass;
        _context.Logins.Update(logger);
        _context.SaveChanges();
    }

    public bool checkEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checklogger(string email, string pass)
    {
        Console.WriteLine("email" + email);
        Console.WriteLine(pass);
        var logger = _context.Logins.FirstOrDefault(lg => lg.Email == email && lg.Password == pass);
        if (logger != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public List<Role> getAllRoles(){
        return _context.Roles.ToList();
    }

    public void deleteUserFromDb(int userId)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);
        user.Isdeleted = true;
        _context.Update(user);
        _context.SaveChanges();
    }

    public List<City> getAllCities()
    {
        List<City> cities = _context.Cities.ToList();
        return cities;
    }

    public List<Country> getAllCountries()
    {
        List<Country> countries = _context.Countries.ToList();
        return countries;
    }

    public List<State> getAllStates()
    {
        List<State> states = _context.States.ToList();
        return states;
    }

    public string GetLoggerRole(LoginViewModel lgnmdl)
    {
        User user = _context.Users.FirstOrDefault(u => u.Email == lgnmdl.username);
        string role = _context.Roles.FirstOrDefault(r => r.Roleid == user.Roleid).Rolename;
        return role;

    }

    public string getPass(string email)
    {
        return _context.Logins.FirstOrDefault(l => l.Email == email).Password;
    }

    public List<string> getRoles()
    {
        return _context.Roles.Select(r => r.Rolename).ToList();
    }

    public List<User> getSearcheduser(string search, int currentPage, int maxRows)
    {
        List<User> userlist = new List<User>();
        userlist = _context.Users.Where(u => u.Firstname + " " + u.Lastname == search)
                                .Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();
        if (userlist.Count() == 0)
        {
            userlist = _context.Users.Where(u => u.Email == search)
                                .Skip((currentPage - 1) * maxRows)
                                .Take(maxRows).ToList();
        }
        return userlist;
    }
    public List<City> getStateCities(int stateId)
    {
        return _context.Cities.Where(c => c.Stateid == stateId).ToList();
    }

    public List<State> getStatesForCountry(int countryId)
    {
        return _context.States.Where(s => s.Countryid == countryId).ToList();
    }

    public decimal getUserCount()
    {
        return _context.Users.Where(u=>u.Isdeleted == false).Count();
    }

    public User getUserDetailFromDb(int userid)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Userid == userid);
        return user;
    }
    public string getUserPass(string email)
    {
        return _context.Logins.FirstOrDefault(l => l.Email == email).Password;
    }
    public User getuserFromDb(string email)
    {
        try
        {
            Console.WriteLine("email" + email);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine("No user found in DB");
            return new User();
        }
    }

    public string getUserRole(int id)
    {
        var role = _context.Roles.FirstOrDefault(r => r.Roleid == id).Rolename;
        return role;
    }

   public List<users> getUsersForPage(int currentPage, int maxRows, string search, string sortBy, string sortOrder)
{
    var userQuery = from u in _context.Users
                    where u.Isdeleted == false
                    select new
                    {
                        u.Userid,
                        FullName = u.Firstname + " " + u.Lastname,
                        u.Email,
                        IsActive = u.Isactive ?? false,
                        Phone = u.Phonenumber.ToString(),
                        Role = _context.Roles.FirstOrDefault(r => r.Roleid == u.Roleid).Rolename,
                        u.Profilephoto,
                        u.Firstname,
                        u.Lastname
                    };

    if (!string.IsNullOrEmpty(search))
    {
        userQuery = userQuery.Where(x => x.FullName.ToLower().Contains(search.ToLower()));
    }

    switch (sortBy.ToLower())
    {
        case "name":
            userQuery = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? userQuery.OrderBy(u => u.Firstname).ThenBy(u => u.Lastname)
                : userQuery.OrderByDescending(u => u.Firstname).ThenByDescending(u => u.Lastname);
            break;

        case "role":
            userQuery = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? userQuery.OrderBy(u => u.Role)
                : userQuery.OrderByDescending(u => u.Role);
            break;

        default:
            // Optional: Define a default sorting column (e.g., by UserId)
            userQuery = userQuery.OrderBy(u => u.Userid);
            break;
    }

    var userList = userQuery.Skip((currentPage - 1) * maxRows)
                            .Take(maxRows)
                            .ToList()
                            .Select(u => new users
                            {
                                userid = u.Userid,
                                name = u.FullName,
                                Email = u.Email,
                                IsActive = u.IsActive,
                                phone = u.Phone,
                                role = u.Role,
                                profilepic = u.Profilephoto
                            })
                            .ToList();

    return userList;
}

    public void saveNewUserInDb(UserDetailModel model, string email, string pass,string imagePath)
    {
        var log = _context.Logins.FirstOrDefault(lg => lg.Email == email);
        if (log == null)
        {
            throw new Exception("Login not found");
        }
        var lastindex = _context.Users.ToList().Count();
        Console.WriteLine(model.firstname);
        var user = new User
        {
            Userid = lastindex + 1,
            Firstname = model.firstname,
            Lastname = model.lastname,
            Email = model.email,
            Phonenumber = model.Phone,
            Roleid = _context.Roles.FirstOrDefault(r => r.Rolename == model.role)?.Roleid ?? throw new Exception("Role not found"),
            Isactive = true,
            Createdby = log.Id,
            Modifiedat = DateTime.Now,
            Modifiedby = log.Id,
            Username = model.username,
            Countryid = model.countryid,
            Stateid = model.stateid,
            Cityid = model.cityid,
            Address = model.address,
            Zipcode = model.Zipcode,
            Profilephoto = imagePath, 
            Isdeleted = false
        };

        _context.Users.Add(user);
        var Logger = new Login
        {
            Id = _context.Logins.Count() + 1,
            Email = model.email,
            Password = pass
        };
        _context.Logins.Add(Logger);
        _context.SaveChanges();

        Console.WriteLine("user saved !!!");

    }


    public bool updatePassword(PasswordModel model, string pass)
    {
        try
        {

            var user = _context.Logins.FirstOrDefault(lg => lg.Email == model.email);
            user.Password = pass;
            _context.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public void updateUserInDb(UserDetailModel model, string email,string imagePath)
    {

        // string? imagePath = null;

        // if ( != null)
        // {
        //     var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        //     if (!Directory.Exists(uploadsFolder))
        //     {
        //         Directory.CreateDirectory(uploadsFolder);
        //     }

        //     var uniqueFileName = $"{Guid.NewGuid()}_{addEditUser.ImageFile.FileName}";
        //     var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //     using (var fileStream = new FileStream(filePath, FileMode.Create))
        //     {
        //         await addEditUser.ImageFile.CopyToAsync(fileStream);
        //     }

        //     imagePath = $"/uploads/{uniqueFileName}";


        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        Console.WriteLine(email + " for updateuser");
        user.Firstname = model.firstname;
        user.Lastname = model.lastname;
        user.Username = model.username;
        user.Phonenumber = model.Phone;
        user.Countryid = _context.Countries.FirstOrDefault(c => c.Countryid == model.countryid).Countryid;
        user.Cityid = _context.Cities.FirstOrDefault(c => c.Cityid == model.cityid).Cityid;
        user.Stateid = _context.States.FirstOrDefault(s => s.Stateid == model.stateid).Stateid;
        user.Address = model.address;
        user.Zipcode = model.Zipcode;
        if(imagePath != null){
        user.Profilephoto = imagePath;
        }
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public Country getUserCountry(int cid)
    {
        try
        {
            Country c = _context.Countries.FirstOrDefault(c => c.Countryid == cid)!;
            return c;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Country();
        }
    }

    public State getUserState(int id)
    {
        try
        {
            State? s = _context.States.FirstOrDefault(s => s.Stateid == id)!;
            return s;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new State();
        }

    }

    public City getUserCity(int id)
    {
        try
        {
            City? city = _context.Cities.FirstOrDefault(c => c.Cityid == id)!;
            return city;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new City();
        }

    }

    public void updateUserInDb(UserDetailModel model, int id,string imagePath)
    {
       var user = _context.Users.FirstOrDefault(u => u.Userid == id);
        Console.WriteLine(id + " for updateuser");
        user.Firstname = model.firstname;
        user.Lastname = model.lastname;
        user.Username = model.username;
        user.Phonenumber = model.Phone;
        user.Email = model.email;
        user.Countryid = model.countryid;
        user.Cityid = model.cityid;
        user.Stateid = model.stateid;
        user.Address = model.address;
        user.Zipcode = model.Zipcode;
        user.Profilephoto = imagePath;
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public List<Rolesandpermission> getPemissionsFromDb(int roleid)
    {
      var temp = _context.Rolesandpermissions.Where(rp=>rp.Roleid == roleid)
                        .OrderBy(rp=>rp.Permissionid).ToList();
      return temp;
    }

    public string getPermissionName(int permissionid)
    {
        string permissionname = _context.Permissions.FirstOrDefault(p=>p.Permissionid == permissionid).Permissionname;
        return permissionname;
    }

    public void updatePermission(int permissionid, bool can_view, bool can_Edit, bool can_delete,int roleid)
    {
        Rolesandpermission permission = _context.Rolesandpermissions.FirstOrDefault(rp=> rp.Roleid == roleid && rp.Permissionid == permissionid); 
        permission.Canedit = can_Edit;
        permission.Canview = can_Edit;
        permission.Candelete = can_delete;
        _context.Update(permission);
        _context.SaveChanges();

    }

    public List<Permission> getAllPermissions()
    {
        List<Permission> permissions = _context.Permissions
                                            .OrderBy(rp=>rp.Permissionid)
                                            .ToList();
        return permissions;
    }

    public string getRolename(int roleid)
    {
       return _context.Roles.FirstOrDefault(r=>r.Roleid == roleid).Rolename;
    }

    public List<Category> getAllCategories()
    {
        List<Category> categories = _context.Categories.ToList();
        return categories;
    }

    public void addNewCategory(string categoryName, string categoryDescription,string createdBy)
    {
        Category category = new Category();
        category.Categoryname = categoryName;
        category.Categoryid = _context.Categories.Count()+1;
        category.Createdat = DateTime.Now;
        category.Description = categoryDescription;
        category.Modifiedat = DateTime.Now;
        category.Createdby = _context.Logins.FirstOrDefault(lg=>lg.Email == createdBy).Id;
        _context.Categories.Add(category);
        _context.SaveChanges();
        
    }

    public bool IsUserExist(string email)
    {
        User u = _context.Users.FirstOrDefault(u=>u.Email == email);
        if(u != null){
            return true;
        }
        else{
            return false;
        }
    }

    public int getLoggerUId(string username)
    {
        int uid = _context.Users.FirstOrDefault(u=>u.Email == username).Userid;
        return uid;
    }

    public string getUserImagePath(int userid)
    {
        return _context.Users.FirstOrDefault(u=>u.Userid == userid).Profilephoto;
    }
}
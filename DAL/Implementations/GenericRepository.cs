using DAL.Data;

public class GenericRepository : IGenericRepository
{
    public PizzashopCContext _context;

    public GenericRepository(PizzashopCContext context)
    {
        _context = context;
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

    public List<string> getAllCities()
    {
        List<string> cities = _context.Cities.Select(c => c.Cityname).ToList();
        return cities;
    }

    public List<string> getAllCountries()
    {
        List<string> countries = _context.Countries.Select(c => c.Countryname).ToList();
        return countries;
    }

    public List<string> getAllStates()
    {
        List<string> states = _context.States.Select(s=>s.Statename).ToList();
        return states;
    }

    public string GetLoggerRole(LoginViewModel lgnmdl)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == lgnmdl.username);
        var role = _context.Roles.FirstOrDefault(r => r.Roleid == user.Roleid).Rolename;
        return role;

    }

    public string getPass(string email)
    {
        return _context.Logins.FirstOrDefault(l => l.Email == email).Password;
    }

    public User getuserFromDb(string email)
    {
        try
        {
            Console.WriteLine("email"+email);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
             
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine("No user found in DB");
            return new User();
        }
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

    public void updateUserInDb(UserDetailModel model, string email)
    {
        var user = _context.Users.FirstOrDefault(u=>u.Email == email);
       Console.WriteLine(email+" for updateuser");
        user.Firstname = model.firstname;
        user.Lastname = model.lastname;
        user.Username = model.username;
        user.Phonenumber = model.Phone;
        user.Countryid = _context.Countries.FirstOrDefault(c=>c.Countryname == model.country).Countryid;
        user.Cityid = _context.Cities.FirstOrDefault(c=>c.Cityname == model.city).Cityid;
        user.Stateid = _context.States.FirstOrDefault(s=>s.Statename == model.state).Stateid ;
        user.Address = model.address;
        user.Zipcode = model.Zipcode;
        _context.SaveChanges();
    }
}
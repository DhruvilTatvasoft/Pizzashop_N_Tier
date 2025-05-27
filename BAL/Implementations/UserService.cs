using System.Collections;
using System.Security.Cryptography.X509Certificates;
using BAL.Interfaces;
using DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class UserService : IUser
{

    public IGenericRepository _repository;
    public IAESService _aesservice;

    public IImagePath _imagePath;

    public IEmailGenService _emailService;
    public UserService(IGenericRepository repository, IAESService aESService, IEmailGenService emailService,IImagePath imagePath)
    {
        _repository = repository;
        _aesservice = aESService;
        _emailService = emailService;
        _imagePath = imagePath;
    }

    public bool changePass(HttpRequest req, chang_p_model model, string email, string password)
    {
        if (model.oldpass == password && model.confirmpass == model.newpass)
        {
            string EncryptedPass = _aesservice.Encrypt(model.newpass);
            _repository.changePass(model, email, EncryptedPass);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsUserExist(string email)
    {
        return _repository.IsUserExist(email);
    }
    public List<Country> getAllCountries()
    {
        return _repository.getAllCountries();
    }

    public List<Role> getAllRoles()
    {
        return _repository.getAllRoles();
    }
    public List<City> getStateCities(int stateId)
    {
        return _repository.getStateCities(stateId);
    }

    public string getPass(string email)
    {
        var pass = _repository.getPass(email);
        return _aesservice.Decrypt(pass);
    }

    public List<string> getRoles()
    {
        return _repository.getRoles();
    }

    public List<State> getStates(int countryId)
    {
        try
        {

            List<State> statelist = _repository.getStatesForCountry(countryId);
            return statelist;
        }
        catch (Exception)
        {
            return new List<State>();
        }
    }


    public userpagingdetailmodel loadusers(userpagingdetailmodel model, int currentPage, int maxRows, string search,string sortBy,string sortOrder)
    {

        List<users> userlist = _repository.getUsersForPage(currentPage, maxRows, search,sortBy,sortOrder);
        model.users = userlist;
        model.PageCount = (int)Math.Ceiling(_repository.getUserCount() / Convert.ToDecimal(maxRows));
        model.CurrentPageIndex = currentPage;
        model.totalusers = (int)_repository.getUserCount();
        model.maxRows = maxRows;
        model.sortBy = sortBy;
        model.sortOrder = sortOrder;

        return model;
    }

    public void updateUser(UserDetailModel model, string email)
    {

        string? imagePath = null;

        if (model.profilePicPath != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{model.profilePicPath.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.profilePicPath.CopyTo(fileStream);
            }

            imagePath = $"/uploads/{uniqueFileName}";
        }
        _repository.updateUserInDb(model, email, imagePath ?? "");
    }

    public void saveNewUser(UserDetailModel model, string email)
    {
        try
        {
            var pass = _aesservice.Encrypt(model.password!);

            string imagePath = _imagePath.getImagePath(model.profilePicPath);
            Console.WriteLine("saving user");
            _repository.saveNewUserInDb(model, email, pass,imagePath);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void deleteUser(int userId)
    {
        _repository.deleteUserFromDb(userId);
    }

    public UserDetailModel getUserDetails(int Id)
    {
        Console.WriteLine("edituser service");
        User u = _repository.getUserDetailFromDb(Id);
        UserDetailModel model = new UserDetailModel();
        model.firstname = u.Firstname;
        model.lastname = u.Lastname;
        model.userid = Id;
        model.username = u.Username;
        model.role = _repository.getUserRole(u.Roleid);
        model.email = u.Email;
        model.stateid = _repository.getUserState(u.Stateid ?? 1).Stateid;
        model.countryid = _repository.getUserCountry(u.Countryid ?? 1).Countryid;
        model.cityid = _repository.getUserCity(u.Cityid ?? 1).Cityid;
        model.Zipcode = u.Zipcode!;
        model.status = u.Isactive ?? false;
        model.address = u.Address!;
        model.Phone = u.Phonenumber!;
        model.Role = _repository.getRoles();
        model.Country = _repository.getAllCountries();
        model.State = _repository.getStatesForCountry(_repository.getUserCountry(u.Countryid ?? 1).Countryid);
        model.City = _repository.getStateCities(_repository.getUserState(u.Stateid ?? 1).Stateid);
        return model;
    }

    public void updateUser(UserDetailModel model, int id)
    {
        string imagePath = _imagePath.getImagePath(model.profilePicPath);
        _repository.updateUserInDb(model, id,imagePath);
    }

    public PermissionsModel2 permissionsForRole(int roleid)
    {
        
        List<Permission> plist = _repository.getAllPermissions();
        List<Rolesandpermission> rp = _repository.getPemissionsFromDb(roleid);
        List<int> gpermissionid = new List<int>();
        foreach (var r in rp)
        {
            gpermissionid.Add(r.Permissionid);

        }
        PermissionsModel2 model = new PermissionsModel2();
        model.plist = plist;
        model.gpermissionid = gpermissionid;
        model.grantedPermissions = rp;
        model.roleid = roleid;
        model.rolename = _repository.getRolename(roleid);
        List<PermissionModel3> permissionModel = new List<PermissionModel3>();
        foreach(var p in plist){
        PermissionModel3 model2 = new PermissionModel3();
        model2.can_view = rp.Where(Rp => Rp.Permissionid == p.Permissionid).Select(Rp => Rp.Canview).FirstOrDefault();
        model2.can_edit = rp.Where(Rp => Rp.Permissionid == p.Permissionid).Select(Rp => Rp.Canedit).FirstOrDefault();
        model2.can_delete = rp.Where(Rp => Rp.Permissionid == p.Permissionid).Select(Rp => Rp.Candelete).FirstOrDefault();
        model2.PermissionId = p.Permissionid;
        permissionModel.Add(model2);
        }
        model.permissionModel = permissionModel;
        return model;
    }


    public void updatePermissions(PermissionsModel model)
    {
        throw new NotImplementedException();
    }

    public string getUserImagePath(int userid)
    {
        if (userid == 0)
        {
            return "/images/manager.png";
        }
    
        User u = _repository.getUserDetailFromDb(userid);
        if (u.Profilephoto == null)
        {
            return "/images/manager.png";
        }
        return u.Profilephoto;
    }

    public string getUserRole(string email)
    {
        return _repository.getUserRoleFromDb(email);
    }
}
using System.Collections;
using System.Reflection;
using DAL.Data;
using Microsoft.AspNetCore.Http;

public interface IUser{
    void changePass(HttpRequest req,chang_p_model model,string email);
    List<Country> getAllCountries();
    List<City> getStateCities(int stateId);
    string getPass(string v);
    List<string> getRoles();
    List<State> getStates(int countryId);

    // userpagingdetailmodel getSearcheduser(string search);
    userpagingdetailmodel loadusers(userpagingdetailmodel model,int currentPage,int maxRows,string search);
    public void updateUser(UserDetailModel model,string email);
    public void updateUser(UserDetailModel model,int id);
    void saveNewUser(UserDetailModel model,string email);
    void deleteUser(int userId);
    UserDetailModel getUserDetails(int Id);

    List<Role> getAllRoles();
    PermissionsModel2 permissionsForRole(int roleid);
    void updatePermissions(PermissionsModel model);
}
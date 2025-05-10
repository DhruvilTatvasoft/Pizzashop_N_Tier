using DAL.Data;

public interface IGenericRepository
{
    public bool checklogger(string email,string password);

    public string GetLoggerRole(LoginViewModel lgnmdl);

    public bool checkEmail(string email);
    bool updatePassword(PasswordModel model,string pass);
    User getuserFromDb(string email);
    List<Country> getAllCountries();
    List<State> getAllStates();
    List<City> getAllCities();
    void updateUserInDb(UserDetailModel model, string email,string imagePath);
    void updateUserInDb(UserDetailModel model,int id,string imagePath);
    string getPass(string email);
    void changePass(chang_p_model model, string email,string pass);
    List<users> getUsersForPage(int currentPage,int maxRows,string search,string sortBy,string sortOrder);
    decimal getUserCount();
    List<User> getSearcheduser(string search,int currentPage,int maxRows);
    string getUserRole(int id);
    List<State> getStatesForCountry(int countryId);
    List<string> getRoles();
    List<City> getStateCities(int stateId);
    void saveNewUserInDb(UserDetailModel model,string email,string pass,string imagePath);
    void deleteUserFromDb(int userId);
    User getUserDetailFromDb(int userid);

    Country getUserCountry(int userid);
    State getUserState(int userid);
    City getUserCity(int userid);
    string getUserPass(string email);

    List<Role> getAllRoles();
    List<Rolesandpermission> getPemissionsFromDb(int roleid);
    string getPermissionName(int permissionid);
    void updatePermission(int permissionid, bool can_view1, bool can_Edit, bool can_view2,int roleid);
    List<Permission> getAllPermissions();
    string getRolename(int roleid);
    List<Category> getAllCategories();
    void addNewCategory(string categoryName, string categoryDescription,string createdBy);
    bool IsUserExist(string email);
    int getLoggerUId(string username);
}
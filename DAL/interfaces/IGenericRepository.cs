using DAL.Data;

public interface IGenericRepository
{
    public bool checklogger(string email,string password);

    public string GetLoggerRole(LoginViewModel lgnmdl);

    public bool checkEmail(string email);
    bool updatePassword(PasswordModel model,string pass);
    User getuserFromDb(string email);
    List<string> getAllCountries();
    List<string> getAllStates();
    List<string> getAllCities();
    void updateUserInDb(UserDetailModel model, string email);
    string getPass(string email);
}
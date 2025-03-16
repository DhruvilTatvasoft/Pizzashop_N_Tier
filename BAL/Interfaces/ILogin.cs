using DAL.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;

public interface ILogin{
    public bool checkloggerInDb(LoginViewModel lgnmdl);

    public string saveLogger(LoginViewModel lgnmdl);

     public bool emailExist(string email);
    void updatePass(PasswordModel model);
    User getUser(string email);
    UserDetailModel setUserInModel(User user);

    int getLoggerUId(string username);
}
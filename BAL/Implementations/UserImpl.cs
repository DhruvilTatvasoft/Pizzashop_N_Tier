using Microsoft.AspNetCore.Mvc.Diagnostics;

public class UserImpl : IUser{

    public IGenericRepository _repository;
    public UserImpl(IGenericRepository repository){
        _repository = repository;
    }

    public string getPass(string email)
    {
        return _repository.getPass(email);
    }

    public void updateUser(UserDetailModel model,string email)
    {
        Console.WriteLine(email);
        Console.WriteLine("----------------");
        _repository.updateUserInDb(model, email);
    }

}
using DAL.Data;

public class AuthService : IAuthServices
{
    public readonly IGenericRepository _repository;

    public AuthService(IGenericRepository repository){
        _repository = repository;
    }
    public string GetUserRole(string email)
    {
        User matchUser = _repository.getuserFromDb(email);
        var fetchRole = _repository.getRolename(matchUser.Roleid);
        return fetchRole;
    }
}
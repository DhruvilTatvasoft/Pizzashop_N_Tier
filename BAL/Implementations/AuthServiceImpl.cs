using DAL.Data;

public class AuthServiceImpl : IAuthServices
{
    public readonly IGenericRepository _repository;

    public AuthServiceImpl(IGenericRepository repository){
        _repository = repository;
    }
    public string GetUserRole(string email)
    {
        User matchUser = _repository.getuserFromDb(email);
        var fetchRole = _repository.getRolename(matchUser.Roleid);
        return fetchRole;
    }
}
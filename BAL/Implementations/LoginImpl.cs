using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using BAL.Interfaces;
using DAL.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BAL.Implementations;

public class LoginImpl : ILogin, IJwtTokenGenService
{
  private readonly IGenericRepository _repository;

  private readonly IConfiguration _configuration;

  private readonly IAESService _AesService;
  public LoginImpl(IGenericRepository repository, IConfiguration configuration,IAESService AesService)
  {
    _repository = repository;
    _configuration = configuration;
    _AesService = AesService;
  }
  public bool checkloggerInDb(LoginViewModel lgnmdl)
  {
    string password = _AesService.Encrypt(lgnmdl.password);
    
    return _repository.checklogger(lgnmdl.username, password);
  }
  public bool emailExist(string email){
    Console.WriteLine(email);
    return _repository.checkEmail(email);
  }

  public string GenerateJwtToken(string userName,string role)
  {

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
        {
              new(ClaimTypes.Name, userName),
              new(ClaimTypes.Role, role)
        }),
      IssuedAt = DateTime.UtcNow,
      Issuer = _configuration["JWT:Issuer"],
      Audience = _configuration["JWT:Audience"],
      Expires = DateTime.UtcNow.AddMinutes(30), // can change exprires time
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var userToken = tokenHandler.WriteToken(token);
    return userToken;
  }

    public string saveLogger(LoginViewModel lgnmdl)
  {
      var role = _repository.GetLoggerRole(lgnmdl);
      var email = lgnmdl.username;
      var token = GenerateJwtToken(role,email);
      
      return token;
  }

    public void updatePass(PasswordModel model)
    {
      if(model.newpass == model.confirmpass){
        string pass = _AesService.Encrypt(model.newpass);
        var user = _repository.updatePassword(model,pass);
      }
    }
    

    public User getUser(string email)
    {
       try{
          var user = _repository.getuserFromDb(email);
          return user;
       }
       catch(Exception e){
        Console.WriteLine("no user detail found !!!");
        return new User();
       }
    }

    public UserDetailModel setUserInModel(User user)
    {
        UserDetailModel ud = new UserDetailModel
        {
            firstname = user.Firstname,
            lastname = user.Lastname,
            address = user.Address,
            City = _repository.getAllCities(),
            Country = _repository.getAllCountries(),
            State = _repository.getAllStates(),
            Phone = user.Phonenumber,
            Zipcode = user.Zipcode,
            username = user.Username,
            ProfilePath= user.Profilephoto,
            stateid = (int)user.Stateid,
            countryid = (int)user.Countryid,
            cityid = (int)user.Cityid,
        };
        return ud;
    }
}

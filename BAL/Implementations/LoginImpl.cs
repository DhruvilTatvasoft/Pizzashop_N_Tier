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

public class LoginImpl : ILogin, IAESService, IJwtTokenGenService
{
  private readonly IGenericRepository _repository;

  private readonly IConfiguration _configuration;

  public LoginImpl(IGenericRepository repository, IConfiguration configuration)
  {
    _repository = repository;
    _configuration = configuration;
  }
  public bool checkloggerInDb(LoginViewModel lgnmdl)
  {
    string password = Encrypt(lgnmdl.password);
    
    return _repository.checklogger(lgnmdl.username, password);
  }
  public bool emailExist(string email){
    Console.WriteLine(email);
    return _repository.checkEmail(email);
  }

  public string Decrypt(string encryptedText)
  {
    string secretKey = "$ASPcAwSNIgcPPEoTSa0ODw#";


    byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);


    using (Aes aes = Aes.Create())
    {
      aes.Key = secretBytes;
      aes.Mode = CipherMode.ECB;
      aes.Padding = PaddingMode.PKCS7;

      byte[] decryptedBytes = null;
      using (ICryptoTransform decryptor = aes.CreateDecryptor())
      {
        decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
      }

      return Encoding.UTF8.GetString(decryptedBytes);
    }
  }

  public string Encrypt(string plainText)
  {
    string secretKey = "$ASPcAwSNIgcPPEoTSa0ODw#";

    byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

    using (Aes aes = Aes.Create())
    {
      aes.Key = secretBytes;
      aes.Mode = CipherMode.ECB;
      aes.Padding = PaddingMode.PKCS7;

      byte[] encryptedBytes = null;
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
      {
        encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
      }

      return Convert.ToBase64String(encryptedBytes);
    }
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
        string pass = Encrypt(model.newpass);
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
            username = user.Username
        };
        return ud;
    }
}


using System.ComponentModel.DataAnnotations;
using DAL.Data;

public class UserDetailModel
{
    public int userid { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    public string firstname { get; set; }
    [Required(ErrorMessage = "Last name is required.")]
    public string lastname { get; set; }
    
    public string email { get; set; }
    [Required(ErrorMessage = "Phone number is required.")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    public string address { get; set; }
    // [Required(ErrorMessage = "Role is required.")]
    public string role { get; set; }

    // [Required(ErrorMessage = "Status is required.")]
    public bool status { get; set; }
   
    public List<City> City { get; set; }
    public string city { get; set; }
   
    [Required(ErrorMessage = "City is required.")]
    public int cityid { get; set; }
    
    public List<State> State { get; set; }
    public string state { get; set; }
    [Required(ErrorMessage = "State is required.")]
    public int stateid { get; set; }
    public List<Country> Country { get; set; }

    public string country { get; set; }
    [Required(ErrorMessage = "Country is required.")]
    public int countryid { get; set; }
    public List<string> Role { get; set; }

    [Required(ErrorMessage = "Zipcode is required.")]
    public string Zipcode { get; set; }
    [Required(ErrorMessage = "Username is required.")]
    public string username { get; set; }
   
    public string password { get; set; }
}
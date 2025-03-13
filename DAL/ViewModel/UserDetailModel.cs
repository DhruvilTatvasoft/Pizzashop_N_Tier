
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DAL.Data;

public class UserDetailModel
{
    public int? userid { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    public string firstname { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string lastname { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    public string email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    public string address { get; set; }
    [Required(ErrorMessage = "Role is required.")]
    public string? role { get; set; }

    public bool? status { get; set; }
    [Required(ErrorMessage = "Role is required.")]
    public string? Status { get; set; }
    public List<City>? City { get; set; }
    
     

    
    [Required(ErrorMessage = "City is required.")]
    public int cityid { get; set; }

    public List<State>? State { get; set; }
    // [Required(ErrorMessage = "State is required.")]
    // public string? state { get; set; }
    [Required(ErrorMessage = "State is required.")]
    public int stateid { get; set; }
    // [Required(ErrorMessage = "Country is required.")]
    public List<Country>? Country { get; set; }

    public string? country { get; set; }
    [Required(ErrorMessage = "Country is required.")]
    public int countryid { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public List<string>? Role { get; set; }

    //  [Required(ErrorMessage = "Role is required.")]
    // public int roleid { get; set; }

    [Required(ErrorMessage = "Zipcode is required.")]
    public string? Zipcode { get; set; }
    [Required(ErrorMessage = "Username is required.")]
    public string? username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? password { get; set; } = null;
    public IFormFile profilePicPath {get;set;}
    public string ProfilePath {get;set;}

}
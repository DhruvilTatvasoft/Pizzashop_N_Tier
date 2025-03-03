
using DAL.Data;

public class UserDetailModel
{
    public int userid { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string email { get; set; }
    public string Phone { get; set; }
    public string address { get; set; }

    public string role{get;set;}

    public bool status{get;set;}
    public List<City> City { get; set; }
    public string city { get; set; }
    public int cityid { get; set; }
    public List<State> State { get; set; }
    public string state { get; set; }
    public int stateid { get; set; }
    public List<Country> Country { get; set; }
    public string country { get; set; }
    public int countryid { get; set; }
    public List<string> Role { get; set; }

    public string Zipcode { get;  set; } 
    public string username{get; set;}
    public string password{get;set;}

}
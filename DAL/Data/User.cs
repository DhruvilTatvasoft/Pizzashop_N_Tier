using System;
using System.Collections.Generic;

namespace DAL.Data;

public partial class User
{
    public int Userid { get; set; }

    public string Email { get; set; } = null!;

    public int Roleid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Profilephoto { get; set; }

    public int? Countryid { get; set; }

    public int? Stateid { get; set; }

    public int? Cityid { get; set; }

    public string? Address { get; set; }

    public string? Zipcode { get; set; }

    public string? Phonenumber { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public int Modifiedby { get; set; }

    public virtual City? City { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual State? State { get; set; }
}

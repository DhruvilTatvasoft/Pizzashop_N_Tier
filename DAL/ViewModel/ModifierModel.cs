using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;



public class ModifierModel
{
    public int max_value;
    public int min_value;
    public int ModifiergroupId;


    public Modifiergroup mg
    {
        get;
        set;
    }
    public List<Modifier> modifiers
    {
        get;
        set;
    }

    public List<Modifiergroup> modifiergroups { get; set; }
    
    [Required(ErrorMessage = "Modifiergroup is required")]
    public int Modifiergroupid { get; set; }

    [Required(ErrorMessage = "Modifier Name is required")]
    [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "please Use only letters")]
    public string Modifiername { get; set; }

    [Required(ErrorMessage = "Modifier Name is required")]
    public Modifiergroup modifiergroup { get; set; }

    [Required(ErrorMessage = "Modifier quantity is required")]
    public int Modifierquantity { get; set; }
    public int Unitid{get;set;}
    [Required(ErrorMessage = "Modifier rate is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Item rate cannot be less than 1")]
    public decimal Modifierrate { get; set; }
    public List<Unit> units { get; set; }
    public string Description { get; set; }
    public string payload { get; set; }

    public List<int> ModifierIds { get; set; }
    public Modifier modifier { get; set; }

    public int Modifierid{get;set;}
}

using System;
using System.Collections.Generic;
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
    public int Modifiergroupid { get; set; }

    public string Modifiername { get; set; }

    public Modifiergroup modifiergroup { get; set; }
    public int Modifierquantity { get; set; }
    public int Unitid{get;set;}
    public decimal Modifierrate { get; set; }
    public List<Unit> units { get; set; }
    public string Description { get; set; }
    public string payload { get; set; }

    public List<int> ModifierIds { get; set; }
    public Modifier modifier { get; set; }

    public int Modifierid{get;set;}
}

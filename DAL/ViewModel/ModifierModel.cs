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

        public Modifiergroup mg{
            get;
            set;
        }
        public List<Modifier> modifiers
        {
            get;
            set;
        }
    }

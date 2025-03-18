using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface IModifierService
    {
        List<Modifier> getModifiersForMGroup(int modifiergroupId);
        Modifiergroup GetModifiergroup(int modifiergroupId);

        List<Modifiergroup> getAllModifierGroups();
        List<Modifier> getAllModifiers();
        void addModifiersForItem(List<ModifierModel> modifierModels, int itemid,string email);
        List<ModifierModel> getModifiersForItem(int itemid);

    }
}
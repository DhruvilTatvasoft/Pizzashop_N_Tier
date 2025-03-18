using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace DAL.interfaces
{
    public interface IModifierRepository
    {
        List<Modifier> getModifiersForMG(int ModifierGroupId);
        Modifiergroup GetModifiergroup(int ModifierGroupId);
        List<Modifiergroup> getAllModifierGroups();
        List<Modifier> getAllModifiers();
        void addModifiersForItem(ModifierModel modifier, int itemid,string email);
        // List<Modifier> getModifiersForItem(int itemid);
        List<ModifierModel> getModifiersForItem(int itemid);
        List<Modifier> getSelectedModifiers(List<int> modifierIds);
    }
}
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
        List<Modifier> getSelectedModifiers(List<int> modifierIds);
        List<Modifier> getSearchedModifier(string searchedModifier);
        void AddNewModifierGroup(Modifiergroup mg, List<int> modifierIds);
        void deleteModifier(int modifierid, int modifiergroupid);
        void updateModifierGroup(Modifiergroup mg, List<int> modifierIds);
        void deleteModifierGroup(int modifierGroupId);
        List<Unit> GetAllUnits();
        void AddNewModifier(Modifier modifier);
        Modifier getModifier(int modifierid, int modifierGroupId);
        void updateModifier(Modifier modifier, int modifierGroupId);
    }
}
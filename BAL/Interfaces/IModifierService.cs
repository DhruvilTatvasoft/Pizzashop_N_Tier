using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface IModifierService
    {
        ItemModel getModifiersForMGroup(int modifiergroupId,int pageSize,int pageNumber);
        Modifiergroup GetModifiergroup(int modifiergroupId);

        List<Modifiergroup> getAllModifierGroups();
        ItemModel getAllModifiers(int? modifierGroupId,int pageSize,int pageNumber);
        void addModifiersForItem(List<ModifierModel> modifierModels, int itemid,string email);
        List<ModifierModel> getModifiersForItem(int itemid);
        List<Modifier> getSelectedModifiers(List<int> modifierIds);
        ItemModel getSearchedModifier(string searchedModifier,int pageSize,int pageNumber);
        bool AddNewModifierGroup(Modifiergroup mg, List<int> modifierIds);
        void deleteModifier(int modifierid, int modifiergroupid);
        bool updateModifierGroup(Modifiergroup mg, List<int> modifierIds);
        void deleteModifierGroup(int modifierGroupId);
        List<Unit> GetAllUnits();
        bool AddNewModifier(ModifierModel modifier);
        Modifier getModifier(int modifierid, int modifierGroupId);
        void updateModifier(ModifierModel modifier, int modifierGroupId);
        List<Modifier> getModifiersForMGroupForItem(int modifiergroupId);
        ItemModel getModifiersForModifierGroup(int modifiergroupid);
    }
}
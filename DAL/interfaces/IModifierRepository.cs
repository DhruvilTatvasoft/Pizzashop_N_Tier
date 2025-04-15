using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace DAL.interfaces
{
    public interface IModifierRepository
    {
        ItemModel getModifiersForMG(int ModifierGroupId,int pageSize,int pageNumber);
        Modifiergroup GetModifiergroup(int ModifierGroupId);
        List<Modifiergroup> getAllModifierGroups();
        ItemModel getAllModifiers(int pageSize,int pageNumber);
        void addModifiersForItem(ModifierModel modifier, int itemid,string email);
        // List<Modifier> getModifiersForItem(int itemid);
        List<ModifierModel> getModifiersForItem(int itemid);
        List<Modifier> getSelectedModifiers(List<int> modifierIds);
        List<Modifier> getSearchedModifier(string searchedModifier);
        bool AddNewModifierGroup(Modifiergroup mg, List<int> modifierIds);
        void deleteModifier(int modifierid, int modifiergroupid);
        bool updateModifierGroup(Modifiergroup mg, List<int> modifierIds);
        void deleteModifierGroup(int modifierGroupId);
 List<Unit> GetAllUnits();
        bool AddNewModifier(ModifierModel modifier);
        Modifier getModifierFromDb(int modifierid);
        void updateModifier(ModifierModel modifier, int modifierGroupId);
        void updateModifiersForItem(List<ModifierModel> modifierModels,int? itemid);
        ItemModel getAllMOdifiersForModifierGroup(int? modifierGroupId,int pageSize,int pageNumber);
        List<Modifier> getModifiersForMGroupForItem(int modifiergroupId);
    }
}
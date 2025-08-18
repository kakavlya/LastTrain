using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerInventoryHandler : InventoryHandler
{
    [SerializeField] private SharedData _sharedData;

    protected override List<string> GetAllSlotsFromSave()
    {
        return SaveManager.Instance.Data.PlayerInventorySlots;
    }

    public bool TryGiveInventoryWeaponFromSlots()
    {
        _sharedData.WeaponConfigs.Clear();
        int gaveWeaponsCount = 0;

        foreach (var slot in ActiveSlotUIs)
        {
            if (slot.GetComponentInChildren<InventoryWeapon>() != null)
            {
                _sharedData.WeaponConfigs.Add(slot.GetComponentInChildren<InventoryWeapon>().WeaponConfig);
                gaveWeaponsCount++;
            }
        }

        return gaveWeaponsCount > 0;
    }
}

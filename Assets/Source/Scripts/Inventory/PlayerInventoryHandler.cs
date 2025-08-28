using System.Collections.Generic;
using UnityEngine;
using YG;
using LastTrain.Data;

public class PlayerInventoryHandler : InventoryHandler
{
    [SerializeField] private Shop _shop;
    [SerializeField] private SharedData _sharedData;

    protected override void Start()
    {
        base.Start();
        _shop.SlotIncremented += AddNewSlot;
    }

    protected override List<string> GetAllSlotsFromSave()
    {
        return YG2.saves.PlayerInventorySlots;
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

    private void AddNewSlot()
    {
        YG2.saves.PlayerInventorySlots.Add("");
        SubmitActiveSlots();
        YG2.SaveProgress();
    }
}

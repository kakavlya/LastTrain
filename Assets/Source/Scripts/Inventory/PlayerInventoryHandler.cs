using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : InventoryHandler
{
    [SerializeField] private SharedData _sharedData;
    [SerializeField] protected PlayHandler _playHandler;

    protected override void Awake()
    {
        base.Awake();
        _playHandler.GameStarted += GiveInventoryWeaponFromSlots;
    }

    private void OnDisable()
    {
        _playHandler.GameStarted -= GiveInventoryWeaponFromSlots;
    }

    protected override List<string> GetAllSlotsFromSave()
    {
        return SaveManager.Instance.Data.PlayerInventorySlots;
    }

    private void GiveInventoryWeaponFromSlots()
    {
        _sharedData.WeaponInfos.Clear();

        foreach (var slot in ActiveSlotUIs)
        {
            if (slot.GetComponentInChildren<InventoryWeapon>() != null)
            {
                _sharedData.WeaponInfos.Add(slot.GetComponentInChildren<InventoryWeapon>().WeaponInfo);
            }
        }
    }

    protected override void SaveLocationInInventory()
    {
        while (InventorySlots.Count < ActiveSlotUIs.Count)
        {
            InventorySlots.Add("");
        }

        for (int i = 1; i < ActiveSlotUIs.Count; i++)
        {
            var inventoryWeapon = ActiveSlotUIs[i].GetComponentInChildren<InventoryWeapon>();

            if (inventoryWeapon != null && inventoryWeapon.WeaponInfo != null)
            {
                InventorySlots[i] = inventoryWeapon.WeaponInfo.WeaponName;
            }
            else
            {
                InventorySlots[i] = "";
            }
        }

        SaveManager.Instance.Save();
    }
}

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
        _sharedData.WeaponConfigs.Clear();

        foreach (var slot in ActiveSlotUIs)
        {
            if (slot.GetComponentInChildren<InventoryWeapon>() != null)
            {
                _sharedData.WeaponConfigs.Add(slot.GetComponentInChildren<InventoryWeapon>().WeaponConfig);
            }
        }
    }
}

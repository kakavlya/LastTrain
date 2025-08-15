using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerInventoryHandler : InventoryHandler
{
    [SerializeField] private SharedData _sharedData;
    //[SerializeField] protected PlayHandler _playHandler;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    _playHandler.GameStarted += TryGiveInventoryWeaponFromSlots;
    //}

    //private void OnDisable()
    //{
    //    _playHandler.GameStarted -= TryGiveInventoryWeaponFromSlots;
    //}

    protected override List<string> GetAllSlotsFromSave()
    {
        return SaveManager.Instance.Data.PlayerInventorySlots;
    }

    public bool TryGiveInventoryWeaponFromSlots()
    {
        _sharedData.WeaponConfigs.Clear();
        int gaveCount = 0;

        foreach (var slot in ActiveSlotUIs)
        {
            if (slot.GetComponentInChildren<InventoryWeapon>() != null)
            {
                _sharedData.WeaponConfigs.Add(slot.GetComponentInChildren<InventoryWeapon>().WeaponConfig);
                gaveCount++;
            }
        }

        return gaveCount > 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerSlots;
    [SerializeField] private PlayHandler _playHandler;
    [SerializeField] private SharedData _sharedData;

    private List<WeaponSlotUI> _activeSlotUIs = new List<WeaponSlotUI>();

    private void Start()
    {
        GetActiveSlots();
        _playHandler.GameStarted += GiveInventoryWeaponFromSlots;
    }

    private void GetActiveSlots()
    {
        foreach (var slot in _playerSlots)
        {
            slot.SetActive(false);

            if (slot.TryGetComponent(out WeaponSlotUI weaponSlotUI) && weaponSlotUI.IsActive)
            {
                slot.SetActive(true);
                _activeSlotUIs.Add(weaponSlotUI);
            }
        }
    }

    private void GiveInventoryWeaponFromSlots()
    {
        foreach (var slot in _activeSlotUIs)
        {
            if (slot.GetComponentInChildren<InventoryWeapon>() != null)
            {
                _sharedData.WeaponInfos.Add(slot.GetComponentInChildren<InventoryWeapon>().WeaponInfo);
            }
        }
    }
}

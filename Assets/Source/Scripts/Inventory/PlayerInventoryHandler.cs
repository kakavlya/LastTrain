using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerSlots;
    [SerializeField] private PlayHandler _playHandler;
    [SerializeField] private InventoryWeapon _weaponUIPrefab;
    [SerializeField] private SharedData _sharedData;

    private List<WeaponSlotUI> _activeSlotUIs = new List<WeaponSlotUI>();
    private string _weaponInfosFolder = "WeaponInfos";

    private void Awake()
    {
        GetActiveSlots();
        _playHandler.GameStarted += GiveInventoryWeaponFromSlots;
        _playHandler.GameStarted += SaveLocationInInventory;
    }

    private void OnDisable()
    {
        _playHandler.GameStarted -= GiveInventoryWeaponFromSlots;
        _playHandler.GameStarted -= SaveLocationInInventory;
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

        LoadLocationInInventory();
    }

    private void GiveInventoryWeaponFromSlots()
    {
        _sharedData.WeaponInfos.Clear();

        foreach (var slot in _activeSlotUIs)
        {
            if (slot.GetComponentInChildren<InventoryWeapon>() != null)
            {
                _sharedData.WeaponInfos.Add(slot.GetComponentInChildren<InventoryWeapon>().WeaponInfo);
            }
        }
    }

    private void SaveLocationInInventory()
    {
        while (SaveManager.Instance.Data.InventorySlots.Count < _activeSlotUIs.Count)
        {
            SaveManager.Instance.Data.InventorySlots.Add("");
        }

        for (int i = 1; i < _activeSlotUIs.Count; i++)
        {
            var inventoryWeapon = _activeSlotUIs[i].GetComponentInChildren<InventoryWeapon>();

            if (inventoryWeapon != null)
            {
                SaveManager.Instance.Data.InventorySlots[i] = inventoryWeapon.WeaponInfo.WeaponName;
            }
            else
            {
                SaveManager.Instance.Data.InventorySlots[i] = "";
            }

        }

        SaveManager.Instance.Save();
    }

    private void LoadLocationInInventory()
    {
        List<string> weaponsNames = SaveManager.Instance.Data.InventorySlots;

        for (int i = 0; i < _activeSlotUIs.Count && i < weaponsNames.Count; i++)
        {
            string name = weaponsNames[i];

            if (!string.IsNullOrEmpty(name))
            {
                WeaponInfo weaponInfo = GetWeaponInfoByName(name);

                if (weaponInfo != null)
                {
                    InventoryWeapon inventoryWeapon = Instantiate(_weaponUIPrefab, _playerSlots[i].transform);
                    inventoryWeapon.Init(weaponInfo);
                }
            }
        }
    }

    private WeaponInfo GetWeaponInfoByName(string weaponName)
    {
        WeaponInfo[] weaponInfos = Resources.LoadAll<WeaponInfo>(_weaponInfosFolder);

        foreach (var weaponInfo in weaponInfos)
        {
            if (weaponInfo.WeaponName == weaponName)
                return weaponInfo;
        }

        return null;
    }
}

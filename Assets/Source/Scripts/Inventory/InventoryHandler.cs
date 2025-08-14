using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _slots;
    [SerializeField] private InventoryWeapon _weaponUIPrefab;

    private string _weaponInfosFolder = "WeaponInfos";

    protected List<string> InventorySlots = new List<string>();
    protected List<WeaponSlotUI> ActiveSlotUIs = new List<WeaponSlotUI>();

    protected virtual void Awake()
    {
        InventorySlots = GetAllSlotsFromSave();
        GetActiveSlots();
    }

    protected virtual List<string> GetAllSlotsFromSave()
    {
        return SaveManager.Instance.Data.InventorySlots;
    }

    private void GetActiveSlots()
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            var weaponSlotUI = _slots[i].GetComponent<WeaponSlotUI>();
            _slots[i].SetActive(true);
            ActiveSlotUIs.Add(weaponSlotUI);
            weaponSlotUI.Filled += SaveLocationInInventory;
        }


        //foreach (var slot in _slots)
        //{
        //    slot.SetActive(false);

        //    if (slot.TryGetComponent(out WeaponSlotUI weaponSlotUI) && weaponSlotUI.IsActive)
        //    {
        //        slot.SetActive(true);
        //        ActiveSlotUIs.Add(weaponSlotUI);
        //        weaponSlotUI.Filled += SaveLocationInInventory;
        //    }
        //}

        LoadWeaponsLocationInInventory();
    }

    protected virtual void SaveLocationInInventory()
    {
        while (InventorySlots.Count < ActiveSlotUIs.Count)
        {
            InventorySlots.Add("");
        }

        for (int i = 0; i < ActiveSlotUIs.Count; i++)
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

    private void LoadWeaponsLocationInInventory()
    {
        List<string> weaponsNames = InventorySlots;

        for (int i = 0; i < ActiveSlotUIs.Count && i < weaponsNames.Count; i++)
        {
            string name = weaponsNames[i];

            if (!string.IsNullOrEmpty(name))
            {
                WeaponInfo weaponInfo = GetWeaponInfoByName(name);

                if (weaponInfo != null)
                {
                    InventoryWeapon inventoryWeapon = Instantiate(_weaponUIPrefab, _slots[i].transform);
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

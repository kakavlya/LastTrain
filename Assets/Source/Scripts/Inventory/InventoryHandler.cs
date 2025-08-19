using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _slots;
    [SerializeField] private InventoryWeapon _weaponUIPrefab;

    private string _weaponConfigsFolder = "WeaponConfigs";

    protected List<string> InventorySlots = new List<string>();
    protected List<WeaponSlotUI> ActiveSlotUIs = new List<WeaponSlotUI>();

    public WeaponSlotUI GetLastActiveSlotUIs()
    {
        if (ActiveSlotUIs.Count > 0)
            return ActiveSlotUIs[ActiveSlotUIs.Count - 1];
        return null;
    }

    protected virtual void Awake()
    {
        SubmitActiveSlots();
    }

    protected virtual List<string> GetAllSlotsFromSave()
    {
        return SaveManager.Instance.Data.InventorySlots;
    }

    public void SubmitActiveSlots()
    {
        ActiveSlotUIs.Clear();
        InventorySlots = GetAllSlotsFromSave();

        for (int i = 0; i < InventorySlots.Count; i++)
        {
            var weaponSlotUI = _slots[i].GetComponent<WeaponSlotUI>();
            _slots[i].SetActive(true);
            ActiveSlotUIs.Add(weaponSlotUI);
            weaponSlotUI.Filled += SaveLocationInInventory;
        }

        LoadWeaponsLocationInInventory();
        SaveLocationInInventory();
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

            if (inventoryWeapon != null && inventoryWeapon.WeaponConfig != null)
            {
                InventorySlots[i] = inventoryWeapon.WeaponConfig.Name;
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

        for (int i = 0; i < _slots.Length && i < weaponsNames.Count; i++)
        {
            string name = weaponsNames[i];

            if (!string.IsNullOrEmpty(name))
            {
                WeaponUpgradeConfig weaponConfig = GetWeaponConfigByName(name);

                if (weaponConfig != null)
                {
                    InventoryWeapon inventoryWeapon = Instantiate(_weaponUIPrefab, ActiveSlotUIs[i].transform);
                    inventoryWeapon.Init(weaponConfig);
                }
            }
        }

        SaveManager.Instance.Save();
    }

    private WeaponUpgradeConfig GetWeaponConfigByName(string weaponName)
    {
        WeaponUpgradeConfig[] weaponConfigs = Resources.LoadAll<WeaponUpgradeConfig>(_weaponConfigsFolder);

        foreach (var weaponInfo in weaponConfigs)
        {
            if (weaponInfo.Name == weaponName)
                return weaponInfo;
        }

        return null;
    }
}

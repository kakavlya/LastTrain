using System.Collections.Generic;
using UnityEngine;
using YG;
using LastTrain.Persistence;

namespace LastTrain.Inventory
{
    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _slots;
        [SerializeField] private InventoryWeapon _weaponUIPrefab;

        private string _ñonfigsFolder = "Configs";

        protected List<string> InventorySlots = new List<string>();
        protected List<WeaponSlotUI> ActiveSlotUIs = new List<WeaponSlotUI>();

        protected virtual void Start()
        {
            SubmitActiveSlots();
        }

        public WeaponSlotUI GetLastActiveSlotUIs()
        {
            if (ActiveSlotUIs.Count > 0)
                return ActiveSlotUIs[ActiveSlotUIs.Count - 1];

            return null;
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

        protected virtual List<string> GetAllSlotsFromSave()
        {
            return YG2.saves.InventorySlots;
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
                    InventorySlots[i] = inventoryWeapon.WeaponConfig.WeaponId;
                }
                else
                {
                    InventorySlots[i] = "";
                }
            }

            YG2.SaveProgress();
        }

        private void LoadWeaponsLocationInInventory()
        {
            List<string> weaponsIdes = InventorySlots;

            for (int i = 0; i < _slots.Length && i < weaponsIdes.Count; i++)
            {
                string id = weaponsIdes[i];

                if (!string.IsNullOrEmpty(id))
                {
                    var existingWeapon = ActiveSlotUIs[i].GetComponentInChildren<InventoryWeapon>();

                    if (existingWeapon != null)
                    {
                        if (existingWeapon.WeaponConfig.WeaponId != id)
                        {
                            Destroy(existingWeapon.gameObject);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    WeaponUpgradeConfig weaponConfig = GetWeaponConfigById(id);

                    if (weaponConfig != null)
                    {
                        InventoryWeapon inventoryWeapon = Instantiate(_weaponUIPrefab, ActiveSlotUIs[i].transform);
                        inventoryWeapon.Init(weaponConfig);
                    }
                }
            }

            YG2.SaveProgress();
        }

        private WeaponUpgradeConfig GetWeaponConfigById(string weaponId)
        {
            WeaponUpgradeConfig[] weaponConfigs = Resources.LoadAll<WeaponUpgradeConfig>(_ñonfigsFolder);

            foreach (var weaponInfo in weaponConfigs)
            {
                if (weaponInfo.WeaponId == weaponId)
                    return weaponInfo;
            }

            return null;
        }
    }
}

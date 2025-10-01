using LastTrain.Persistence;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace LastTrain.Inventory
{
    public class InventoryHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _slots;
        [SerializeField] private InventoryWeapon _weaponUIPrefab;

        private string _ñonfigsFolder = "Configs";
        private List<string> _inventorySlots = new List<string>();
        private List<WeaponSlotUI> _activeSlotUIs = new List<WeaponSlotUI>();

        protected List<WeaponSlotUI> ActiveSlotUIs => _activeSlotUIs;

        protected virtual void Start()
        {
            SubmitActiveSlots();
        }

        public WeaponSlotUI GetLastActiveSlotUIs()
        {
            if (_activeSlotUIs.Count > 0)
                return _activeSlotUIs[_activeSlotUIs.Count - 1];

            return null;
        }

        public void SubmitActiveSlots()
        {
            _activeSlotUIs.Clear();
            _inventorySlots = GetAllSlotsFromSave();

            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                var weaponSlotUI = _slots[i].GetComponent<WeaponSlotUI>();
                _slots[i].SetActive(true);
                _activeSlotUIs.Add(weaponSlotUI);
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
            while (_inventorySlots.Count < _activeSlotUIs.Count)
            {
                _inventorySlots.Add(string.Empty);
            }

            for (int i = 0; i < _activeSlotUIs.Count; i++)
            {
                var inventoryWeapon = _activeSlotUIs[i].GetComponentInChildren<InventoryWeapon>();

                if (inventoryWeapon != null && inventoryWeapon.WeaponConfig != null)
                {
                    _inventorySlots[i] = inventoryWeapon.WeaponConfig.WeaponId;
                }
                else
                {
                    _inventorySlots[i] = string.Empty;
                }
            }

            YG2.SaveProgress();
        }

        private void LoadWeaponsLocationInInventory()
        {
            List<string> weaponsIdes = _inventorySlots;

            for (int i = 0; i < _slots.Length && i < weaponsIdes.Count; i++)
            {
                string id = weaponsIdes[i];

                if (!string.IsNullOrEmpty(id))
                {
                    var existingWeapon = _activeSlotUIs[i].GetComponentInChildren<InventoryWeapon>();

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

using System.Collections.Generic;
using UnityEngine;
using YG;
using LastTrain.ShopSystem;
using LastTrain.Data;

namespace LastTrain.Inventory
{
    public class PlayerInventoryHandler : InventoryHandler
    {
        [SerializeField] private Shop _shop;

        protected override void Start()
        {
            base.Start();
            _shop.SlotIncremented += AddNewSlot;
        }

        public bool TryGiveInventoryWeaponFromSlots()
        {
            TransferData.Instance.WeaponConfigs.Clear();
            int gaveWeaponsCount = 0;

            foreach (var slot in ActiveSlotUIs)
            {
                if (slot.GetComponentInChildren<InventoryWeapon>() != null)
                {
                    TransferData.Instance.WeaponConfigs.Add(
                        slot.GetComponentInChildren<InventoryWeapon>().WeaponConfig);
                    gaveWeaponsCount++;
                }
            }

            return gaveWeaponsCount > 0;
        }

        protected override List<string> GetAllSlotsFromSave()
        {
            return YG2.saves.PlayerInventorySlots;
        }

        private void AddNewSlot()
        {
            YG2.saves.PlayerInventorySlots.Add("");
            SubmitActiveSlots();
            YG2.SaveProgress();
        }
    }
}

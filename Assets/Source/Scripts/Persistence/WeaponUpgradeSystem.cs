using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    public void UpgradeWeapon(string weaponId)
    {
        var wp = SaveManager.Instance.Data.Weapons
                    .FirstOrDefault(w => w.WeaponId == weaponId);
        if (wp == null) return;

        int cost = UpgradeCostCalculator.Calculate(wp.UpgradeLevel + 1);
        if (SaveManager.Instance.Data.Coins < cost) return;

        SaveManager.Instance.Data.Coins -= cost;
        wp.UpgradeLevel++;
        SaveManager.Instance.Save();
    }
}

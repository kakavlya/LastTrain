using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Info", fileName = "NewWeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    public string WeaponName;
    public Sprite Icon;
    public Weapon WeaponPrefab;
}

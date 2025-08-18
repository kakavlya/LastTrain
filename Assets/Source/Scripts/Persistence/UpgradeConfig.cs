using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeConfig : ScriptableObject
{
    [SerializeField] private string _weaponId;
    [SerializeField] private string _weaponName;
}

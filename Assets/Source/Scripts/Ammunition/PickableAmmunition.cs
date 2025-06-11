using UnityEngine;

public class PickableAmmunition : MonoBehaviour
{
    [field: SerializeField] public MonoBehaviour PrefabTypeOfWeapon { get; private set; }
    [field: SerializeField] public int CountProjectiles { get; private set; }
}

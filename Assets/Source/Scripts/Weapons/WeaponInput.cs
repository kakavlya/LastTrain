using System;
using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponInput : MonoBehaviour
    {
        public event Action Fired;
        public event Action StopFired;
        public event Action<int> WeaponChanged;


        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Fired?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopFired?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponChanged?.Invoke((int)KeyCode.Alpha1 - (int)KeyCode.Alpha0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponChanged?.Invoke((int)KeyCode.Alpha2 - (int)KeyCode.Alpha0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponChanged?.Invoke((int)KeyCode.Alpha3 - (int)KeyCode.Alpha0);
            }
        }
    }
}

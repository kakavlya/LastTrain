using System.Collections.Generic;
using UnityEngine;

namespace LastTrain.UI
{
    public sealed class UIScreenRouter
    {
        private readonly IReadOnlyList<GameObject> _screens;
        public UIScreenRouter(params GameObject[] screens) => _screens = screens;

        public void ShowOnly(GameObject target)
        {
            if (_screens == null) return;
            for (int i = 0; i < _screens.Count; i++)
            {
                var s = _screens[i];
                if (s) s.SetActive(s == target);
            }
        }

        public void HideAll()
        {
            if (_screens == null) return;
            for (int i = 0; i < _screens.Count; i++)
            {
                var s = _screens[i];
                if (s) s.SetActive(false);
            }
        }
    }
}

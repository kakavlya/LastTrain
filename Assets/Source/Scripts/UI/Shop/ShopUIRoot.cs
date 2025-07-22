using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIRoot : MonoBehaviour
{
    [SerializeField] private WeaponDetailsPanel _detailsPrefab;
    [SerializeField] private Transform _detailsParent;
    [SerializeField] private Image _blocker;       

    private WeaponDetailsPanel _details;

    private void Awake()
    {
        _blocker.gameObject.SetActive(false);
    }

    public void OnItemSelected(WeaponUpgradeConfig cfg, WeaponProgress prog)
    {
        if (_details == null)
            _details = Instantiate(_detailsPrefab, _detailsParent);

        _blocker.gameObject.SetActive(true);
        _details.Show(cfg, prog, CloseDetails);
    }

    private void CloseDetails()
    {
        _blocker.gameObject.SetActive(false);
    }
}

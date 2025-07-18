using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private String _shopScene;
    public void OnShopButtonClick()
    {
        LoadScene(_shopScene);
    }

    private void LoadScene(string shopSceneName)
    {
        SceneManager.LoadScene(shopSceneName);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject _globalManagersPrefab;
    [SerializeField] private string _mainMenuScene = "MainMenu";

    private IEnumerator Start()
    {
        if (FindObjectOfType<SaveManager>() == null)
        {
            Instantiate(_globalManagersPrefab);
        }

        

        yield return SceneManager.LoadSceneAsync(_mainMenuScene, LoadSceneMode.Single);
    }
}

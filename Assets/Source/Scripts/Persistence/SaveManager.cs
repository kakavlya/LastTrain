using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if YANDEX_GAMES
using YG;
#endif
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public ProgressData Data { get; private set; }

    private string _savePath;

    private void Awake()
    {

        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _savePath = Path.Combine(Application.persistentDataPath, "progress.json");
        string fullPath = System.IO.Path.Combine(Application.persistentDataPath, "progress.json");
        Debug.Log($"Full save file = {fullPath}");
        Load();
    }

    private void OnApplicationQuit()
        => Save();

    // saving local progress
    public void Save()
    {
        // Local
        string json = JsonUtility.ToJson(Data, true);

        try { File.WriteAllText(_savePath, json); }
        catch (Exception e) { Debug.LogError($"Save local failed: {e.Message}"); }

#if YANDEX_GAMES
        if (YandexGame.SDKEnabled && YandexGame.isInitialized)
        {
            YandexGame.PlayerData = json;
            YandexGame.SaveProgress();
        }
#endif   
    }


    private void Load()
    {

#if YANDEX_GAMES
        if (TryLoadFromJson(YandexGame.PlayerData, "YG cloud"))
            return;
#endif

        if (File.Exists(_savePath) &&
            TryLoadFromJson(File.ReadAllText(_savePath), "local file"))
            return;

        Data = new ProgressData();
        Save();     
        Debug.Log("<color=yellow>SaveManager:</color> created fresh ProgressData");
    }

    private bool TryLoadFromJson(string json, string sourceTag)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            Data = JsonUtility.FromJson<ProgressData>(json);
            if (Data == null)
                throw new Exception("JsonUtility returned null");

            Debug.Log($"<color=green>SaveManager:</color> loaded from {sourceTag}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"SaveManager: cannot parse {sourceTag} ({e.Message})");
            return false;

        }
    }
}

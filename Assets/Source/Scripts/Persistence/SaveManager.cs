using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public ProgressData Data { get; private set; }

    private string _savePath;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _savePath = Path.Combine(Application.persistentDataPath, "progress.json");
        Load();
    }

    private void OnApplicationQuit()
        => Save();

    // saving local progress
    public void Save()
    {
        // Local
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(_savePath, json);

#if YANDEX_GAMES
        // В облако Yandex Games
        YandexGame.PlayerData = json;
        YandexGame.SaveProgress(); // автоматически загрузит PlayerData
#endif
    }

    
    public void Load()
    {
#if YANDEX_GAMES
        // Попробуем сначала из облака (по умолчанию YandexGame.PlayerData уже подгружен)
        if (!string.IsNullOrEmpty(YandexGame.PlayerData))
        {
            Data = JsonUtility.FromJson<ProgressData>(YandexGame.PlayerData);
            return;
        }
#endif

        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            Data = JsonUtility.FromJson<ProgressData>(json);
        }
        else
        {
            Data = new ProgressData();
            Save();
        }
    }
}

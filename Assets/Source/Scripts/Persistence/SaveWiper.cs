using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SaveWiper
{
#if UNITY_EDITOR
    [MenuItem("Tools/Clear Save Data (PlayerPrefs + persistentDataPath)")]
#endif
    public static void ClearAll()
    {

        // 1) Очистить PlayerPrefs (YG LocalStorage там тоже лежит)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2) Удалить файлы из persistentDataPath (если ты туда что-то писал)
        var dir = Application.persistentDataPath;
        if (System.IO.Directory.Exists(dir))
            System.IO.Directory.Delete(dir, true);

        Debug.Log("Cleared PlayerPrefs and " + dir);
    }

    // Пример: точечно убрать флаг первой сессии YG
#if UNITY_EDITOR
    [MenuItem("Tools/Clear YG: WasFirstGameSession")]
#endif
    public static void ClearYGFirstSessionFlag()
    {
        PlayerPrefs.DeleteKey("WasFirstGameSession_YG"); // достаточно и в WebGL/Editor
        PlayerPrefs.Save();
        Debug.Log("Cleared key WasFirstGameSession_YG");
    }
}

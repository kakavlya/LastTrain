#if UNITY_EDITOR
using Assets.Source.Scripts.Enemies;
using UnityEditor;
using UnityEngine;

public class EnemyValidator : EditorWindow
{
    private GameObject _enemyPrefab;

    [MenuItem("Tools/Validate Enemy Prefab")]
    public static void ShowWindow()
    {
        GetWindow<EnemyValidator>("Enemy Validator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Проверка врага на наличие компонентов", EditorStyles.boldLabel);
        _enemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy Prefab", _enemyPrefab, typeof(GameObject), true);

        if (GUILayout.Button("Проверить"))
        {
            if (_enemyPrefab == null)
            {
                Debug.LogWarning("Пожалуйста, выбери врага.");
                return;
            }

            ValidateEnemy(_enemyPrefab);
        }
    }

    private void ValidateEnemy(GameObject enemy)
    {
        var name = enemy.name;

        void Check<T>(string label, bool required = true) where T : Component
        {
            var comp = enemy.GetComponentInChildren<T>();
            if (comp == null)
            {
                if (required)
                    Debug.LogError($"❌ {name}: отсутствует обязательный компонент <{typeof(T).Name}> ({label})");
                else
                    Debug.LogWarning($"⚠️ {name}: необязательный, но полезный компонент <{typeof(T).Name}> отсутствует ({label})");
            }
            else
            {
                Debug.Log($"✅ {name}: найден компонент <{typeof(T).Name}> ({label})");
            }
        }

        Check<EnemyHealth>("Урон и смерть");
        Check<EnemyDeathHandler>("Отключение поведения и despawn");
        Check<ModelEffects>("VFX, звук, мерцание");
        Check<EnemyMovement>("Движение");
        Check<EnemyController>("Логика и инициализация");
        Check<Collider>("Физика/столкновения");

        var visualRoot = enemy.transform.Find("VisualRoot");
        if (visualRoot == null)
        {
            Debug.LogError($"❌ {name}: отсутствует дочерний объект 'VisualRoot'");
        }
        else
        {
            var deathEffect = visualRoot.GetComponent<EnemyDeathEffect>();
            if (deathEffect == null)
                Debug.LogWarning($"⚠️ {name}: 'VisualRoot' найден, но на нём нет EnemyDeathEffect");
            else
                Debug.Log($"✅ {name}: 'VisualRoot' содержит EnemyDeathEffect");
        }
    }
}
#endif

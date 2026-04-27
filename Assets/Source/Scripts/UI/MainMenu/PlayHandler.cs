using LastTrain.Inventory;
using LastTrain.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LastTrain.UI.MainMenu
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private LevelsHandler _levelsHandler;
        [SerializeField] private PlayerInventoryHandler _playerInventoryHandler;
        [SerializeField] private Persistence.TurretUpgradeConfig[] _turretConfigs;
        [SerializeField] private string _gameplayScene;
        [SerializeField] private Button _playButton;

        private void Awake()
        {
            _playButton.onClick.AddListener(StartPlay);
        }

        private void StartPlay()
        {
            if (_levelsHandler.IsChosed && _playerInventoryHandler.TryGiveInventoryWeaponFromSlots())
            {
                // Pass all turret configs to TransferData so the gameplay scene knows what to spawn
                if (_turretConfigs != null)
                {
                    Data.TransferData.Instance.SetTurretConfigs(new System.Collections.Generic.List<Persistence.TurretUpgradeConfig>(_turretConfigs));
                }

                SceneManager.LoadScene(_gameplayScene);
            }
        }
    }
}

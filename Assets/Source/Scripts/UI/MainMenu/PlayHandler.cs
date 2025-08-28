using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LastTrain.Inventory;
using LastTrain.Level;

namespace LastTrain.UI.MainMenu
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private LevelsHandler _levelsHandler;
        [SerializeField] private PlayerInventoryHandler _playerInventoryHandler;
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
                SceneManager.LoadScene(_gameplayScene);
            }
        }
    }
}

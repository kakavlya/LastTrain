using UnityEngine;
using UnityEngine.UI;

public class MenuTraining : MonoBehaviour
{
    [Header ("Training Screens")]
    [SerializeField] private GameObject _startScreen;

    [Header("Menu Buttons")]
    [SerializeField] private Button _startLevelButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _choseLevelButton;
    [SerializeField] private Button _shopButton;


    public enum TrainingState
    {
        Hello
    }

    private void SwitchTrainingWindows(TrainingState state)
    {
        switch (state)
        {
            case TrainingState.Hello:
                _startLevelButton.interactable = false;
                _inventoryButton.interactable = false;
                _choseLevelButton.interactable= false;
                _shopButton.interactable = false;
                _startScreen.SetActive(true);
                break;
        }
    }
}

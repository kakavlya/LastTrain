using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Advertisement
{
    public class InterstitialAdv : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _returnMenuAfterWinButton;

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(() => { YG2.InterstitialAdvShow(); });
            _returnMenuAfterWinButton.onClick.AddListener(() => { YG2.InterstitialAdvShow(); });
        }
    }
}

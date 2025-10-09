using LastTrain.Core.FSM;

namespace LastTrain.Training
{
    public class ShopInfoState : IState
    {
        private readonly MenuTraining _menuTraining;
        public ShopInfoState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.ShopScreen.SetActive(true);
            _menuTraining.ShopLockerScreen.SetActive(true);
            _menuTraining.ShopInfoTrainingScreen.SetActive(true);
            _menuTraining.BackFromShopButton.interactable = false;
            _menuTraining.SaveStep(MenuTrainingState.ShopInfo);
            _menuTraining.ShopInfoTrainingOkButton.onClick.AddListener(OnNext);
        }

        public void Exit()
        {
            _menuTraining.ShopInfoTrainingOkButton.onClick.RemoveListener(OnNext);
            _menuTraining.ShopInfoTrainingScreen.SetActive(false);
        }

        private void OnNext() => _menuTraining.FSM.Switch<ShopUnlockState>();
    }
}
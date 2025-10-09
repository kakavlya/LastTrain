using LastTrain.Core.FSM;
using LastTrain.Persistence;

namespace LastTrain.Training
{
    public class ShopUnlockState : IState
    {
        private readonly MenuTraining _menuTraining;
        public ShopUnlockState(MenuTraining c) => _menuTraining = c;

        public void Enter()
        {
            _menuTraining.DisableAllTrainingScreens();
            _menuTraining.ShopScreen.SetActive(true);
            _menuTraining.ShopLockerScreen.SetActive(false);
            _menuTraining.ShopUnlockTrainingScreen.SetActive(true);
            _menuTraining.SaveStep(MenuTrainingState.ShopUnlock);
            _menuTraining.SignUpShopItemsUILock(OnUnlocked);
        }

        public void Exit()
        {
            _menuTraining.UnsubscribeShopItems(OnUnlocked);
            _menuTraining.ShopUnlockTrainingScreen.SetActive(false);
        }

        private void OnUnlocked(WeaponProgress progress, WeaponUpgradeConfig config)
        {
            _menuTraining.FSM.Switch<ShopCloseState>();
            _menuTraining.UnsubscribeShopItems(OnUnlocked);
        }
    }
}
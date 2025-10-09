namespace LastTrain.Core.FSM
{
    public class ShopState : MMState
    {
        public ShopState(UIStateMachineMenu ui)
            : base(ui)
        { }

        public override void Enter()
        {
            SetMain(false);
            UI.Router.ShowOnly(UI.ShopScreen);
        }

        public override void Exit()
        {
            UI.Router.HideAll();
        }
    }
}
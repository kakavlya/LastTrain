namespace LastTrain.Core.FSM
{
    public class WeaponState : MMState
    {
        public WeaponState(UIStateMachineMenu ui)
            : base(ui)
        { }

        public override void Enter()
        {
            SetMain(false);
            UI.Router.ShowOnly(UI.ChoseWeaponScreen);
        }

        public override void Exit()
        {
            UI.Router.HideAll();
        }
    }
}
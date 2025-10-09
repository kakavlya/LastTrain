namespace LastTrain.Core.FSM
{
    public class SettingsMenuState : MMState
    {
        public SettingsMenuState(UIStateMachineMenu ui)
            : base(ui)
        { }

        public override void Enter()
        {
            SetMain(false);
            UI.Router.ShowOnly(UI.SettingsScreen);
        }

        public override void Exit()
        {
            UI.Router.HideAll();
        }
    }
}
namespace LastTrain.Core.FSM
{
    public sealed class SettingsState : UMState
    {
        public SettingsState(UIStateMachine ui)
            : base(ui)
        { }

        public override void Enter() => UI.Router.ShowOnly(UI.SettingsScreen);
    }
}
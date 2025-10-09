namespace LastTrain.Core.FSM
{
    public class EndLevelState : UMState
    {
        public EndLevelState(UIStateMachine ui)
            : base(ui)
        { }

        public override void Enter() => UI.Router.ShowOnly(UI.GameEndScreen);
    }
}
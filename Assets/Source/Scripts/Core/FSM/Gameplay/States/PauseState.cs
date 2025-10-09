namespace LastTrain.Core.FSM
{
    public class PauseState : UMState
    {
        public PauseState(UIStateMachine ui)
            : base(ui)
        { }

        public override void Enter() => UI.Router.ShowOnly(UI.GamePauseScreen);
    }
}
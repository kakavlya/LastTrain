namespace LastTrain.Core.FSM
{
    public class PlayingState : UMState
    {
        public PlayingState(UIStateMachine ui)
            : base(ui)
        { }

        public override void Enter() => UI.Router.ShowOnly(UI.HudScreen);
    }
}
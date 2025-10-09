namespace LastTrain.Core.FSM
{
    public class LevelStartState : UMState
    {
        public LevelStartState(UIStateMachine ui)
            : base(ui)
        { }

        public override void Enter() => UI.Router.ShowOnly(UI.StartScreen);
    }
}
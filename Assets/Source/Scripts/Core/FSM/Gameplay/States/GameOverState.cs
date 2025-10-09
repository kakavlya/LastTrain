namespace LastTrain.Core.FSM
{
    public class GameOverState : UMState
    {
        public GameOverState(UIStateMachine ui)
            : base(ui)
        { }

        public override void Enter() => UI.Router.ShowOnly(UI.GameOverScreen);
    }
}
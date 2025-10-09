namespace LastTrain.Core.FSM
{
    public class LeaderboardState : MMState
    {
        public LeaderboardState(UIStateMachineMenu ui)
            : base(ui)
        { }

        public override void Enter()
        {
            SetMain(false);
            UI.Router.ShowOnly(UI.LeaderboardScreen);
        }

        public override void Exit()
        {
            UI.Router.HideAll();
        }
    }
}
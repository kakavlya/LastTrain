namespace LastTrain.Core.FSM
{
    public class LevelState : MMState
    {
        public LevelState(UIStateMachineMenu ui)
            : base(ui)
        { }

        public override void Enter()
        {
            SetMain(false);
            UI.Router.ShowOnly(UI.ChoseLevelScreen);
        }

        public override void Exit()
        {
            UI.Router.HideAll();
        }
    }
}
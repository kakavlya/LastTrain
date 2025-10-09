namespace LastTrain.Core.FSM
{
    public class RootState : MMState
    {
        public RootState(UIStateMachineMenu ui)
            : base(ui)
        { }

        public override void Enter()
        {
            UI.Router.HideAll();
            SetMain(true);
        }

        public override void Exit()
        {
            SetMain(false);
        }
    }
}
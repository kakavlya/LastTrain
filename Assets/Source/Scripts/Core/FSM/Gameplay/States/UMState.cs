namespace LastTrain.Core.FSM
{
    public abstract class UMState : IState
    {
        protected readonly UIStateMachine UI;

        protected UMState(UIStateMachine ui)
        {
            UI = ui;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }
    }
}
namespace LastTrain.Core.FSM
{
    public abstract class MMState : IState
    {
        protected readonly UIStateMachineMenu UI;

        protected MMState(UIStateMachineMenu ui)
        {
            UI = ui;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }

        protected void SetMain(bool visible)
        {
            foreach (var go in UI.MainButtons)
            {
                if (go)
                    go.SetActive(visible);
            }
        }
    }
}
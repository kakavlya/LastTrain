namespace LastTrain.Core.FSM
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}
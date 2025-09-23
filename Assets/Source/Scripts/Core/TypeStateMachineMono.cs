using UnityEngine;

namespace LastTrain.Core.FSM
{
    public abstract class TypeStateMachineMono: MonoBehaviour 
    {
        protected readonly TypeStateMachine _fsm = new();

        protected void Register<TMarker>(IState state) where TMarker : class
            => _fsm.Register<TMarker>(state);

        protected void Switch<TMarker>() where TMarker : class
            => _fsm.Switch<TMarker>();

        protected void ResetFSM() => _fsm.Reset();
    }
}

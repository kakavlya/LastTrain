using UnityEngine;

namespace LastTrain.Core.FSM
{
    public abstract class TypeStateMachineMono : MonoBehaviour
    {
        private readonly TypeStateMachine _fsm = new();

        public TypeStateMachine FSM => _fsm;
    }
}

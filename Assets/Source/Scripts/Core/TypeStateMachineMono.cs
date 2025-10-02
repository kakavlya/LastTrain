using UnityEngine;

namespace LastTrain.Core.FSM
{
    public abstract class TypeStateMachineMono : MonoBehaviour
    {
        public TypeStateMachine FSM => _fsm;

        private readonly TypeStateMachine _fsm = new();
    }
}
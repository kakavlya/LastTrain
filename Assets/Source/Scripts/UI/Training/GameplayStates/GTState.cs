using LastTrain.Core.FSM;
using System.Collections;
using UnityEngine;

namespace LastTrain.Training
{
    public abstract class GTState : IState
    {
        protected readonly GameplayTraining GT;
        protected Coroutine DelayRoutine;

        protected GTState(GameplayTraining gt)
        {
            GT = gt;
        }

        public virtual void Enter() { }

        public virtual void Exit()
        {
            if (DelayRoutine != null)
            {
                GT.StopCoroutine(DelayRoutine);
                DelayRoutine = null;
            }
        }

        protected void StartDelay(IEnumerator routine)
        {
            if (DelayRoutine != null)
            {
                GT.StopCoroutine(DelayRoutine);
            }

            DelayRoutine = GT.StartCoroutine(routine);
        }

        protected void Showed()
        {
            GT.InvokeScreenShowed();
        }

        protected void Left()
        {
            GT.InvokeScreenLeft();
        }
    }
}
using UnityEngine;
using LastTrain.Core.FSM;

namespace LastTrain.UI.FSM
{
    public sealed class ScreenState<TMarker> : IState where TMarker : class
    {
        private readonly UIScreenRouter _router;
        private readonly GameObject _screen;
        public ScreenState(UIScreenRouter router, GameObject screen)
        {
            _router = router; _screen = screen;
        }
        public void Enter() => _router.ShowOnly(_screen);
        public void Exit() {}
    }
}

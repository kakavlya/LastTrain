using System;
using System.Collections.Generic;

namespace LastTrain.Core.FSM
{
    public sealed class TypeStateMachine
    {
        private readonly Dictionary<Type, IState> _states = new();
        private IState _current;

        public Type CurrentKey { get; private set; }

        public void Register<TMarker>(IState state) where TMarker : class
            => _states[typeof(TMarker)] = state;

        public void Register<TState>(TState state) where TState : class, IState
            => _states[typeof(TState)] = state;

        public void Switch<TMarker>() where TMarker : class
        {
            var key = typeof(TMarker);
            if (!_states.TryGetValue(key, out var next) || ReferenceEquals(_current, next))
                return;
            _current?.Exit();
            _current = next;
            CurrentKey = key;
            _current.Enter();
        }

        public void Reset()
        {
            _current?.Exit();
            _current = null;
            CurrentKey = null;
        }
    }
}
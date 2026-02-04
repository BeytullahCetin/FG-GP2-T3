using System;
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public abstract class GameEventArgs : EventArgs { }

    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        private static Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public static void Register<T>(Action<T> act) where T : GameEventArgs
        {
            Type _type = typeof(T);

            if (!_events.ContainsKey(_type))
            {
                _events.Add(_type, act);
                return;
            }

            _events[_type] = Delegate.Combine(_events[_type], act);
        }

        public static void Unregister<T>(Action<T> act) where T : GameEventArgs
        {
            Type _type = typeof(T);

            if (!_events.ContainsKey(_type)) return;

            _events[_type] = Delegate.Remove(_events[_type], act);

            if (_events[_type] == null) _events.Remove(_type);
        }
        
        public static void Invoke<T>(T args) where T : GameEventArgs
        {
            Type _type = typeof(T);

            if (!_events.ContainsKey(_type)) return;

            Action<T> _callback = _events[_type] as Action<T>;
            _callback?.Invoke(args);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class ComponentFactory
    {
        private readonly Dictionary<Component, object> _pools = new();
        private Transform _mainPoolRoot;

        public ComponentFactory(Transform _root)
        {
            _mainPoolRoot = _root;
        }

        public T Spawn<T>(T _prefab, Vector3 _position, Quaternion _rotation, Transform _parent = null) where T : Component
        {
            if (!_pools.TryGetValue(_prefab, out object _poolValue))
            {
                GameObject _poolParent = new GameObject($"{_prefab.name}_Pool");
                _poolParent.transform.SetParent(_mainPoolRoot);
                
                _poolValue = new ObjectPool<T>(_prefab, 5, _poolParent.transform);
                _pools.Add(_prefab, _poolValue);
            }

            ObjectPool<T> _pool = (ObjectPool<T>)_poolValue;
            T _spawned = _pool.Get();
            
            _spawned.transform.SetParent(_parent);
            _spawned.transform.SetPositionAndRotation(_position, _rotation);
            
            return _spawned;
        }

        public void Despawn<T>(T _prefab, T _instance) where T : Component
        {
            if (_pools.TryGetValue(_prefab, out object _poolValue))
            {
                ((ObjectPool<T>)_poolValue).ReturnToPool(_instance);
            }
            else
            {
                Object.Destroy(_instance.gameObject);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class ComponentFactory
    {
        private Dictionary<int, object> _pools = new();
        private Transform _mainPoolRoot;

        public ComponentFactory(Transform _root)
        {
            _mainPoolRoot = _root;
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab == null) return null;

            int key = prefab.GetInstanceID();

            if (!_pools.TryGetValue(key, out object poolValue))
            {
                GameObject poolParent = new GameObject($"{prefab.name}_Pool");
                poolParent.transform.SetParent(_mainPoolRoot);
                
                poolValue = new ObjectPool<GameObject>(prefab, 5, poolParent.transform);
                _pools.Add(key, poolValue);
            }

            ObjectPool<GameObject> pool = (ObjectPool<GameObject>)poolValue;
            GameObject spawned = pool.Get();
            
            spawned.transform.SetParent(parent);
            spawned.transform.SetPositionAndRotation(position, rotation);
            
            return spawned;
        }

        public void Despawn(GameObject prefab, GameObject instance)
        {
            if (prefab == null || instance == null)
                return;

            int key = prefab.GetInstanceID();

            if (_pools.TryGetValue(key, out object poolValue))
            {
                ((ObjectPool<GameObject>)poolValue).ReturnToPool(instance);
                return;
            }

            Object.Destroy(instance);
        }
    }
}
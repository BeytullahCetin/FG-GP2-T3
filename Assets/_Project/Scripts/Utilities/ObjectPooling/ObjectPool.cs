using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> _objects = new();
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }

        private void CreateNewObject()
        {
            T _newObj = Object.Instantiate(_prefab, _parent);
            _newObj.gameObject.SetActive(false);
            _objects.Enqueue(_newObj);
        }

        public T Get()
        {
            if (_objects.Count == 0)
            {
                CreateNewObject();
            }

            T _obj = _objects.Dequeue();
            _obj.gameObject.SetActive(true);
            return _obj;
        }

        public void ReturnToPool(T _obj)
        {
            if (_objects.Contains(_obj)) return;

            _obj.gameObject.SetActive(false);
            _obj.transform.SetParent(_parent);
            _objects.Enqueue(_obj);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class ObjectPool<T> where T : Object
    {
        Queue<T> _objects = new();
        T _prefab;
        Transform _parent;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }

        void CreateNewObject()
        {
            T newObj = Object.Instantiate(_prefab, _parent);

            if (newObj is GameObject go) go.SetActive(false);
            else if (newObj is Component comp) comp.gameObject.SetActive(false);

            _objects.Enqueue(newObj);
        }

        public T Get()
        {
            if (_objects.Count == 0)
            {
                CreateNewObject();
            }

            T obj = _objects.Dequeue();
            
            if (obj is GameObject go) go.SetActive(true);
            else if (obj is Component comp) comp.gameObject.SetActive(true);
            
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            if (_objects.Contains(obj)) return;

            if (obj is GameObject go)
            {
                go.SetActive(false);
                go.transform.SetParent(_parent);
            }
            else if (obj is Component comp)
            {
                comp.gameObject.SetActive(false);
                comp.transform.SetParent(_parent);
            }

            _objects.Enqueue(obj);
        }
    }
}
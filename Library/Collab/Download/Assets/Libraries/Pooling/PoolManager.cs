using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace UnityEngine
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private List<Pool> _pools = new List<Pool>(100);
        private static Dictionary<int, PoolItemContainer> _itemDictionary = new Dictionary<int, PoolItemContainer>(10000);
        private static Dictionary<int, Pool> _poolDictionary = new Dictionary<int, Pool>(1000);
        private static PoolManager _instance;

        private void Awake()
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                _pools[i].Initiate();
            }
        }

        public static GameObject Produce(GameObject prefab, Transform parent = null, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (prefab != null)
            {
                Pool pool = null;
                if (!_poolDictionary.TryGetValue(prefab.GetInstanceID(), out pool))
                {
                    pool = new Pool(prefab);
                    RegisterPool(prefab, pool);
                }
                return pool.Produce(parent, position, rotation);
            }
            else
            {
                Debug.LogError("PoolManager error: Pooling failed due to null prefab");
                return null;
            }
        }

        public static void Recycle(GameObject gameObject)
        {
            GetPoolItem(gameObject, _itemDictionary).Recycle();
        }

        public static void Recycle(GameObject gameObject, float delay)
        {
            _instance.StartCoroutine(_instance.RecycleCoroutine(delay));
        }

        private static void RegisterPool(GameObject prefab, Pool pool)
        {
            _poolDictionary.Add(prefab.GetInstanceID(), pool);
        }

        private static void RegisterObject(GameObject gameObject, PoolItemContainer poolItem)
        {
            _itemDictionary.Add(gameObject.GetInstanceID(), poolItem);
        }

        private IEnumerator RecycleCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            GetPoolItem(gameObject, _itemDictionary).Recycle();
        }

        private static PoolItemContainer GetPoolItem(GameObject gameObject, Dictionary<int, PoolItemContainer> dictionary)
        {
            PoolItemContainer pool = null;
            if (dictionary.TryGetValue(gameObject.GetInstanceID(), out pool))
            {
                return pool;
            }
            else
            {
                throw new System.MemberAccessException("Pool not found for: " + gameObject.name);
            }
        }

        [System.SerializableAttribute]
        private class Pool
        {
            [SerializeField] private GameObject _prefab;
            [SerializeField] private int _initialAmount;
            [SerializeField] private bool _persistent;

            private Transform _transform;
            private Stack<PoolItemContainer> _items;

            //PUBLIC///////////////////////////////////////////////////
            public Pool(GameObject prefab)
            {
                _prefab = prefab;
                _items = new Stack<PoolItemContainer>(16);
            }

            public void Initiate()
            {
                if (_prefab != null)
                {
                    PoolManager.RegisterPool(_prefab, this);
                    _items = new Stack<PoolItemContainer>(16);
                    Expand(_initialAmount, null);
                }
            }

            public void Expand(int amount, Transform parent = null)
            {
                for (int i = 0; i < amount; i++)
                {
                    GameObject gameObject = GameObject.Instantiate(_prefab, parent);
                    PoolItemContainer item = new PoolItemContainer(this, gameObject);
                    _items.Push(item);
                    PoolManager.RegisterObject(gameObject, item);
                }
            }

            public GameObject Produce(Transform parent = null, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
            {
                if (_items.Count == 0)
                {
                    Expand(1, parent);
                }
                PoolItemContainer item = _items.Pop();
                GameObject gameObject = item.Produce();
                Transform transform = gameObject.transform;
                if (parent != null)
                {
                    transform.SetParent(parent);
                }
                transform.localPosition = position;
                transform.localRotation = rotation;
                item.OnProduce();
                return gameObject;
            }

            public void Recycle(PoolItemContainer item)
            {
                _items.Push(item);
            }
        }

        private class PoolItemContainer
        {
            private bool _recycled;
            private Pool _pool;
            private GameObject _gameObject;
            private IPoolItem _poolItem;

            public PoolItemContainer(Pool pool, GameObject gameObject)
            {
                this._pool = pool;
                this._gameObject = gameObject;
                gameObject.SetActive(false);
                _poolItem = gameObject.GetComponent<IPoolItem>();
                if (_poolItem != null)
                {
                    _poolItem.OnInstantiate();
                }
            }

            public GameObject Produce()
            {
                _recycled = false;
                _gameObject.SetActive(true);
                return _gameObject;
            }

            public void OnProduce()
            {
                if (_poolItem != null)
                {
                    _poolItem.OnProduce();
                }
            }

            public void Recycle()
            {
                if (!_recycled)
                {
                    _recycled = true;
                    _pool.Recycle(this);
                    _gameObject.SetActive(false);
                    if (_poolItem != null)
                    {
                        _poolItem.OnRecycle();
                    }
                }
            }
        }
    }

    public static class ExtensionMethods
    {
        public static GameObject GetCopy(this GameObject go, Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
        {
            return PoolManager.Produce(go, parent, position, rotation);
        }

        public static void Recycle(this GameObject go)
        {
            PoolManager.Recycle(go);
        }
    }
}

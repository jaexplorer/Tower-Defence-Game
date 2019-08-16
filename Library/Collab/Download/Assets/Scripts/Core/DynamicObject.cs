// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SimulatedObject : MonoBehaviour, IUpdatable, IPoolItem
// {
//     [SerializeField] private bool _isUpdatable;
//     [SerializeField] private bool _deleteOnSceneClear;

//     bool IUpdatable.active
//     {
//         get;
//         set;
//     }

//     bool IPoolItem.recycled
//     {
//         get;
//         set;
//     }

//     // int IUpdatable.index
//     // {
//     //     get;
//     //     set;
//     // }

//     // int IUpdatable.index
//     // {
//     //     get;
//     //     set;
//     // }

//     void IPoolItem.OnProduce()
//     {
//         if (_isUpdatable)
//         {
//             UpdateManager.AddItem(this);
//         }
//         OnProduce();
//     }


//     void IPoolItem.OnRecycle()
//     {
//         if (_isUpdatable)
//         {
//             UpdateManager.AddItem(this);
//         }
//     }

//     void IPoolItem.OnInstantiate()
//     {

//     }

//     void IPoolItem.OnDestroy()
//     {

//     }

//     protected virtual void OnProduce()
//     {

//     }


//     public void Recycle()
//     {
//         PoolManager.Recycle(gameObject);
//         UpdateManager.RemoveItem(this);
//     }

//     public void ManagedUpdate()
//     {

//     }

//     public void SetUpdateOrder(int order)
//     {
//         // UpdateManager.SetIndex(this);
//     }
// }
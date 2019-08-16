// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// // using System.Runtime.CompilerServices.DynamicAttribute;

// public class EffectManager : MonoBehaviour
// {
//     [SerializeField] private ManagedObject _target;
//     private List<Effect> _effects = new List<Effect>();

//     //PROPERTIES///////////////////////////////////////////////
//     public List<Effect> effects { get { return _effects; } }
//     public ManagedObject target { get { return _target; } }

//     //EVENTS///////////////////////////////////////////////////
//     public void Awake()
//     {
//         _target.onRecycle.AddListener(RemoveAllEffects);
//         dynamic target = _target;
//         try
//         {
//             foreach (Effect e in target.data.onSpawnEffects)
//             {
//                 AddEffect(e);
//             }
//         }
//         catch
//         {
//             Debug.LogError("target is nor Tower or Enemy. Or doesn't contain onSpawnEffects");
//         }
//     }

//     //PUBLIC///////////////////////////////////////////////////
//     public void AddEffect(GameObject effectPrefab)
//     {
//         Effect effect = effectPrefab.GetCopy(_target.transform).GetComponent<Effect>();
//         if (effect == null)
//         {
//             Debug.LogError("Effect not found on the given prefab.");
//             return;
//         }
//         AddEffect(effect);
//     }

//     public void AddEffect(Effect effect)
//     {

//         foreach (Effect e in _effects)
//         {
//             if (e == effect)
//             {
//                 e.Stack();
//                 return;
//             }
//         }
//         effect.Initiate(this);
//         _effects.Add(effect);
//     }

//     public void RemoveEffect(Effect effect)
//     {
//         effect.Remove();
//     }

//     private void RemoveAllEffects()
//     {
//         _effects.ForEach(e => e.Remove());
//     }
// }


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Effect : ManagedObject
// {
//     [SerializeField] protected AudioSource _audioSource;
//     [SerializeField] protected ParticleSystem _particleEffect;
//     [SerializeField] protected int _lifetime;
//     [SerializeField] protected int _interval;
//     [SerializeField] protected int _coolDown;
//     [SerializeField] protected int _amount;
//     [SerializeField] protected int _maxStacks;
//     [SerializeField] protected bool _ongoing;
//     protected EffectManager _effectManager;
//     protected int _stacks;
//     protected int _intervalCounter;
//     protected int _coolDownCounter;
//     protected int _lifetimeCounter;
//     protected bool _onCoolDown = false;

//     //PUBLIC///////////////////////////////////////////////////

//     public virtual void Initiate(EffectManager effectManager)
//     {
//         _effectManager = effectManager;
//         _intervalCounter = _interval;
//         _coolDownCounter = _coolDown;
//         _lifetimeCounter = _lifetime;

//         if (_ongoing)
//         {
//             ManagedUpdate();
//         }
//         else
//         {
//             Apply();
//         }
//     }

//     protected override void ManagedUpdate()
//     {
//         if (_lifetimeCounter > 0)
//         {
//             if (_onCoolDown)
//             {
//                 if (_coolDownCounter > 0)
//                 {
//                     _coolDownCounter--;
//                     if (_coolDownCounter <= 0)
//                     {
//                         ToggleCoolDown();
//                     }
//                 }
//             }
//             else if (_intervalCounter > 0)
//             {
//                 _intervalCounter--;
//                 if (_intervalCounter <= 0)
//                 {
//                     Apply();
//                     ResetIntervalCounter();
//                 }
//             }
//         }
//         _lifetimeCounter--;
//         if (_lifetimeCounter <= 0)
//         {
//             Remove();
//         }
//     }

//     public virtual void Stack()
//     {
//         if (_maxStacks != 0)
//         {
//             if (_stacks < _maxStacks)
//             {
//                 _stacks++;
//             }
//         }
//     }

//     public virtual void Remove()
//     {
//         Recycle();
//     }

//     //PROTECTED////////////////////////////////////////////////

//     protected virtual void Apply()
//     {

//     }

//     protected void ToggleCoolDown()
//     {
//         _onCoolDown = !_onCoolDown;
//         _coolDownCounter = _coolDown;
//     }

//     protected void ResetIntervalCounter()
//     {
//         _intervalCounter = _interval;
//     }
// }


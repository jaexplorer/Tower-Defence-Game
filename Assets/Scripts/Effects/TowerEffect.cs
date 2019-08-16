// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TowerEffect : Effect
// {
//     protected Tower _tower;

//     public override void Initiate(EffectManager manager)
//     {
//         base.Initiate(manager);
//         if (manager.target is Tower)
//         {
//             _tower = (Tower)manager.target;
//         }
//         else
//         {
//             Debug.LogError("Wrong effect target type.");
//         }
//     }
// }
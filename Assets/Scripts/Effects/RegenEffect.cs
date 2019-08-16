// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RegenEffect : EnemyEffect
// {
//     protected override void OnProduce()
//     {
//         _enemy.onDamage.AddListener(ToggleCoolDown);
//     }

//     protected override void Apply()
//     {
//         if (_amount + _enemy.health > _enemy.data.health) // if healed above max health
//         {
//             _enemy.Heal(_amount + _enemy.health - _enemy.data.health); // find the difference and add it
//         }
//         else
//         {
//             _enemy.Heal(_amount); // else heal amount
//         }
//     }
// }

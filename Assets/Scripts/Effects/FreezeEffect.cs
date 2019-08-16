// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class FreezeEffect : EnemyEffect
// {
//     protected override void Apply()
//     {
//         if (_enemy.currentTile.fog != null) // If the enemy is in the fog
//         {
//             _enemy.frostFogEffect.Play(); // play frostfog effect
//             if (_stacks < _maxStacks) 
//             {
//                 _stacks++;
//                 if (_stacks > _maxStacks)
//                 {
//                     _stacks = _maxStacks;
//                 }
//             }
//         }
//         else if (_stacks > 0)
//         {
//             _enemy.AlterSpeed(_amount * _stacks); // unfreeze the enemy
//             _stacks--;
//             if (_stacks < 0)
//             {
//                 _stacks = 0;
//             }
//             if (_stacks == 0)
//             {
//                 _enemy.frostFogEffect.Stop(); // Stop the effect
//                 Remove();
//             }
//         }

//         if (_stacks != 0)
//         {
//             _enemy.AlterSpeed(-_amount * _stacks); // freeze the enemy by the stack level
//         }
//     }
// }

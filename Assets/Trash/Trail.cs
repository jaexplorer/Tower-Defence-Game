// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Trail : MonoBehaviour
// {
//     [SerializeField]
//     private TrailRenderer _trailRenderer;
//     private Tile _tile;

//     private void FixedUpdate()
//     {
//         transform.position = _tile.transform.position;
//         if (_tile.next != null)
//         {
//             _tile = _tile.next;
//         }
//     }

//     public void Hide()
//     {
//         _trailRenderer.time = 0.0f;
//         Invoke("Recycle", 1.5f);
//     }

//     public void Recycle()
//     {
//         Pool.Recycle(gameObject);
//     }

//     public void Show()
//     {
//         _trailRenderer.time = 99999f;
//         _tile = Level.instance.spawner.tile;
//         transform.position = _tile.transform.position;
//         _trailRenderer.enabled = true;
//     }
// }

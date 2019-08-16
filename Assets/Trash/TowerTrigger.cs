// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [System.SerializableAttribute]
// public class TowerTrigger// : MonoBehaviour
// {
//     [SerializeField]
//     private Tower _tower;
//     private TowerTriggerData _data;
//     private List<Tile> _tiles = new List<Tile>(24);
//     private int _enemyCount;

//     public List<Tile> tiles
//     {
//         get { return _tiles; }
//     }

//     // public void OnBuild()
//     // {
//     //     for (int i = 0; i < _data.markers.Count; i++)
//     //     {
//     //         _tiles.Add(Level.current.GetTile(_data.markers[i]));
//     //     }
//     // } 

//     public void MarkTiles()
//     {
//         _tiles.Clear();
//         for (int i = 0; i < _data.markers.Count; i++)
//         {
//             Tile tile = Level.current.GetTile(_data.markers[i]);
//             if (tile != null)
//             {
//                 tile.AddTowerTrigger(this);
//                 _tiles.Add(Level.current.GetTile(_data.markers[i]));
//                 //tile.onEnemyEnter.AddListener(OnEnemyEnter);
//             }
//         }
//         _tiles.Sort(delegate (Tile a, Tile b)
//         {
//             if (a.order > b.order)
//                 return -1;
//             else
//                 return 1;
//         });
//         // for (int i = 0; i < _tiles.Count; i++)
//         // {
//         //     // Find first tile;

//         //     _tiles[0].onEnemyEnter.AddListener(this);
//         //     for (int j = 0; j < _tiles.Count; j++)
//         //     {

//         //     }
//         // }
//     }

//     public void OnEnemyEnter(Enemy enemy)
//     {
//         _enemyCount++;
//         _tower.OnEnemyEnter(enemy);
//     }

//     public void OnEnemyExit(Enemy enemy)
//     {
//         _enemyCount--;
//         _tower.OnEnemyExit(enemy);
//     }

//     // public void GetLastTarget()
//     // {

//     // }
// }

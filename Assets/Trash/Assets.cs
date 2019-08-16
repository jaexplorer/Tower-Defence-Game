// using System.Collections;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using UnityEngine;

// [CreateAssetMenu(fileName = "AssetProvider", menuName = "Data/AssetProvider", order = 1)]
// public class Assets : ScriptableObject
// {
//     [SerializeField] private GameObject _tilePrefab;
//     [SerializeField] private LevelList _levelList;
//     private ReadOnlyCollection<LevelData> _levelDataReadonly;

//     //PROPERTIES///////////////////////////////////////////////
//     private static Assets instance { get; set; }
//     public static GameObject tilePrefab { get { return instance._tilePrefab; } }
//     public static ReadOnlyCollection<LevelData> levels { get { return instance._levelDataReadonly; } }

//     //PUBLIC///////////////////////////////////////////////////
//     public void Initialize()
//     {
//         instance = this;
//         _levelDataReadonly = new ReadOnlyCollection<LevelData>(_levelList.levelData);
//     }
// }

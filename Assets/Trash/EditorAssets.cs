// using System.Collections;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using UnityEngine;

// [CreateAssetMenu(fileName = "EditorAssetProvider", menuName = "Data/EditorAssetProvider", order = 1)]
// public class EditorAssets : ScriptableObject
// {
//     [SerializeField] private List<TileTopData> _tileTopData;
//     [SerializeField] private List<TileBottomData> _tileBottomData;
//     [SerializeField] private List<TileObjectData> _tileObjectData;
//     [SerializeField] private List<TowerData> _towerData;
//     [SerializeField] private EditorData _editorData;
//     [SerializeField] private GameObject _gridSprite;

//     private ReadOnlyCollection<TileTopData> _tileTopDataReadonly;
//     private ReadOnlyCollection<TileBottomData> _tileBottomDataReadonly;
//     private ReadOnlyCollection<TileObjectData> _tileObjectDataReadonly;
//     private ReadOnlyCollection<TowerData> _towerDataReadonly;

//     //PROPERTIES///////////////////////////////////////////////
//     private static EditorAssets instance { get; set; }
//     public static ReadOnlyCollection<TileTopData> tileTopData { get { return instance._tileTopDataReadonly; } }
//     public static ReadOnlyCollection<TileBottomData> tileBottomData { get { return instance._tileBottomDataReadonly; } }
//     public static ReadOnlyCollection<TileObjectData> tileObjectData { get { return instance._tileObjectDataReadonly; } }
//     public static ReadOnlyCollection<TowerData> towerData { get { return instance._towerDataReadonly; } }
//     public static EditorData editorData { get { return instance._editorData; } }

//     //PUBLIC///////////////////////////////////////////////////
//     public void Initialize()
//     {
//         instance = this;
//         FindAssets();
//     }

//     public static void FindAssets()
//     {
// #if UNITY_EDITOR
//         // Find all TileData assets.
//         string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ScriptableObject", new string[] { "Assets/Data" });
//         foreach (string guid in guids)
//         {
//             ScriptableObject so = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
//             if (so is TileTopData)
//             {
//                 TileTopData data = (TileTopData)so;
//                 if (data != null)
//                 {
//                     instance._tileTopData.Add(data);
//                 }
//             }
//             else if (so is TileBottomData)
//             {
//                 TileBottomData data = (TileBottomData)so;
//                 if (data != null)
//                 {
//                     instance._tileBottomData.Add(data);
//                 }
//             }
//             else if (so is TileObjectData)
//             {
//                 TileObjectData data = (TileObjectData)so;
//                 if (data != null)
//                 {
//                     instance._tileObjectData.Add(data);
//                 }
//             }
//             else if (so is TowerData)
//             {
//                 TowerData data = (TowerData)so;
//                 if (data != null)
//                 {
//                     instance._towerData.Add(data);
//                 }
//             }
//         }
//         instance._tileTopDataReadonly = new ReadOnlyCollection<TileTopData>(instance._tileTopData);
//         instance._tileBottomDataReadonly = new ReadOnlyCollection<TileBottomData>(instance._tileBottomData);
//         instance._tileObjectDataReadonly = new ReadOnlyCollection<TileObjectData>(instance._tileObjectData);
//         instance._towerDataReadonly = new ReadOnlyCollection<TowerData>(instance._towerData);
// #endif
//     }
// }

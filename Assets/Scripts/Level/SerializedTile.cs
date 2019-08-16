// using UnityEngine;

// /// <summary>
// /// Information about the tile
// /// </summary>

// [System.Serializable]
// public class SerializedTile : IEditableTile
// {
//     public static readonly float levelHeight = 0.5f;
//     public static readonly float elevationHeight = 0.25f;

//     [SerializeField] public TileTopData _topData;
//     [SerializeField] public TileBottomData _bottomData;
//     [SerializeField] public TileObjectData _objectData;
//     [SerializeField] public TowerData _towerData;
//     [SerializeField] public Direction _direction;
//     [SerializeField] public int _level;
//     [SerializeField] public bool _elevated;
//     // [SerializeField] public bool _flipX;
//     // [SerializeField] public bool _flipZ;

//     //PROPERTIES///////////////////////////////////////////////
//     public TileTopData topData { get { return _topData; } }
//     public TileBottomData bottomData { get { return _bottomData; } }
//     public TileObjectData objectData { get { return _objectData; } }
//     public TowerData towerData { get { return _towerData; } }
//     public Direction direction { get { return _direction; } }
//     public int level { get { return _level; } }
//     public bool elevated { get { return _elevated; } }
//     // public bool flipX { get { return _flipX; } }
//     // public bool flipZ { get { return _flipZ; } }

//     TileTopData IEditableTile.topData { get { return _topData; } set { _topData = value; } }
//     TileBottomData IEditableTile.bottomData { get { return _bottomData; } set { _bottomData = value; } }
//     TileObjectData IEditableTile.objectData { get { return _objectData; } set { _objectData = value; } }
//     TowerData IEditableTile.towerData { get { return _towerData; } set { _towerData = value; } }
//     Direction IEditableTile.direction { get { return _direction; } set { _direction = value; } }
//     int IEditableTile.level { get { return _level; } set { _level = value; } }
//     bool IEditableTile.elevated { get { return _elevated; } set { _elevated = value; } }
//     // bool IEditableTile.flipX { get { return _flipX; } set { _flipX = value; } }
//     // bool IEditableTile.flipZ { get { return _flipZ; } set { _flipZ = value; } }
// }

// public interface IEditableTile
// {
//     TileTopData topData { get; set; }
//     TileBottomData bottomData { get; set; }
//     TileObjectData objectData { get; set; }
//     TowerData towerData { get; set; }
//     Direction direction { get; set; }
//     int level { get; set; }
//     bool elevated { get; set; }
//     // bool flipX { get; set; }
//     // bool flipZ { get; set; }
// }
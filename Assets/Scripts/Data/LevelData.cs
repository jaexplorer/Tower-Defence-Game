// using UnityEngine;

// [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 1)]
// public class LevelData : ScriptableObject, IEditableLevelData
// {
//     [SerializeField] private SerializedTile[] _tiles = new SerializedTile[256];
//     [SerializeField] private int _sizeX = 16;
//     [SerializeField] private int _sizeZ = 16;

//     [SerializeField] private int _startingEnergy;
//     [SerializeField] private SpawnWave[] _spawnWaves;

//     //PROPERTIES///////////////////////////////////////////////
//     public int sizeX { get { return _sizeX; } }
//     public int sizeZ { get { return _sizeZ; } }
//     public int startingEnergy { get { return _startingEnergy; } }
//     public SpawnWave[] spawnWaves { get { return _spawnWaves; } }

//     //PUBLIC///////////////////////////////////////////////////
//     void IEditableLevelData.MoveBorders(int left, int right, int forward, int back)
//     {
//         // Finding new array size.
//         int newSizeX = _sizeX + right + left;
//         int newSizeZ = _sizeZ + forward + back;

//         // Checking if size is correct.
//         if (newSizeX > 0 && newSizeZ > 0)
//         {
//             // Creating new array.
//             SerializedTile[] newTiles = new SerializedTile[newSizeX * newSizeZ];

//             // Finding overlapping range which will be copied to the new array.
//             int overlapXMin = Mathf.Max(-left, 0);
//             int overlapXMax = Mathf.Min(_sizeX + right, _sizeX);
//             int overlapZMin = Mathf.Max(-back, 0);
//             int overlapZMax = Mathf.Min(_sizeZ + forward, _sizeZ);

//             // Copying overlapping tiles to the new array.
//             for (int x = overlapXMin, newX = x + left; x < overlapXMax; x++, newX = x + left)
//             {
//                 for (int z = overlapZMin, newZ = z + back; z < overlapZMax; z++, newZ = z + back)
//                 {
//                     newTiles[IndexFromMapPosition(newX, newZ, newSizeX)] = _tiles[IndexFromMapPosition(x, z, _sizeX)];
//                 }
//             }

//             // Applying changes.
//             _tiles = newTiles;
//             _sizeX = newSizeX;
//             _sizeZ = newSizeZ;
//         }
//     }

//     public SerializedTile GetTile(Vector3 worldVector)
//     {
//         return GetTile((int)(worldVector.x + 0.5f), (int)(worldVector.z + 0.5f));
//     }

//     public SerializedTile GetTile(TilePosition mapPosition)
//     {
//         return GetTile(mapPosition.x, mapPosition.z);
//     }

//     public SerializedTile GetTile(int x, int z)
//     {
//         if (x >= 0 && x < _sizeX && z >= 0 && z < _sizeZ)
//         {
//             return _tiles[IndexFromMapPosition(x, z, sizeX)];
//         }
//         else
//         {
//             return null;
//         }
//     }

//     public int IndexFromMapPosition(int x, int z, int sizeX)
//     {
//         return z * sizeX + x;
//     }

//     //PRIVATE//////////////////////////////////////////////////
//     private TilePosition IndexToMapPosition(int index)
//     {
//         return new TilePosition(index % _sizeX, index / _sizeX);
//     }
// }

// public interface IEditableLevelData
// {
//     void MoveBorders(int left, int right, int forward, int back);
// }
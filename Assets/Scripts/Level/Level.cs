using UnityEngine;
using System.Collections.Generic;

public class Level : CustomBehaviour
{
    // [SerializeField] private SerializedTile[] _serializedTiles = new SerializedTile[256];
    [SerializeField] private int _sizeX = 16;
    [SerializeField] private int _sizeZ = 16;
    [SerializeField] private int _startingEnergy;
    [SerializeField] private SpawnWave[] _spawnWaves;
    [SerializeField] private CompoundMesh _compoundMesh;
    [SerializeField] private GameObject _tilePrefab;

    private Tile[,] _tiles;
    private List<Teleport> _teleports = new List<Teleport>(8);
    private Transform _objectContainerTransform;
    private Spawner _spawner;
    private Portal _portal;

    //PROPERTIES///////////////////////////////////////////////
    public int sizeX { get { return _sizeX; } }
    public int sizeZ { get { return _sizeZ; } }
    public int startingEnergy { get { return _startingEnergy; } }
    public SpawnWave[] spawnWaves { get { return _spawnWaves; } }
    public static Level instance { get; private set; }
    public CompoundMesh compoundMesh { get { return _compoundMesh; } }
    public Spawner spawner { get { return _spawner; } }
    public Portal portal { get { return _portal; } }
    public int wavesCount { get { return spawnWaves.Length; } }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        instance = this;
    }

    protected override void OnWaveSave()
    {
        List<Tile.SaveData> tileSaveData = ProfileManager.instance.saveData.tileSaveData;
        tileSaveData.Clear();
        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                Tile.SaveData data = null;
                Tile tile = GetTile(x, z);
                if (tile != null)
                {
                    data = tile.GetSaveData();
                }
                tileSaveData.Add(data);
            }
        }
    }

    protected override void OnWaveLoad()
    {
        List<Tile.SaveData> tileSaveData = ProfileManager.instance.saveData.tileSaveData;
        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                Tile.SaveData data = tileSaveData[IndexFromMapPosition(x, z, _sizeX)];
                if (data != null)
                {
                    _tiles[x, z].LoadSaveData(data);
                }
            }
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Load()
    {
        // = levelData;
        GameObject objectsContainer = GameObject.Find(Names.levelObjects);
        if (objectsContainer != null)
        {
            DestroyImmediate(objectsContainer);
        }
        objectsContainer = new GameObject(Names.levelObjects);
        objectsContainer.hideFlags = HideFlags.DontSaveInBuild;
        _objectContainerTransform = objectsContainer.transform;
        _objectContainerTransform.hideFlags = HideFlags.DontSave;

        // Loading tiles.
        _tiles = new Tile[_sizeX, _sizeZ];
        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                Tile tile = GetTile(x, z);
                if (tile.topData != null || tile.objectData != null)
                {
                    // PoolManager.Produce(_tilePrefab, _objectContainerTransform, new Vector3(x, 0f, z)).GetComponent<Tile>();
                    tile.Load(tile, x, z, compoundMesh, _teleports);
                    _tiles[x, z] = tile;
                    if (tile.isPortal)
                    {
                        _portal = tile.GetComponentInChildren<Portal>();
                        _portal.Reset();
                    }
                    if (tile.isSpawner)
                    {
                        _spawner = tile.GetComponentInChildren<Spawner>();
                    }
                }
            }
        }
        _teleports.Clear();

        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                Tile tile = GetTile(x, z);
                if (tile != null)
                {
                    tile.LoadPrebuiltTower();
                }
            }
        }
    }

    public void LoadInEditor()
    {
        // = levelData;

        // Clearing and creating object container.
        GameObject objectsContainer = GameObject.Find(Names.levelObjects);
        if (objectsContainer != null)
        {
            DestroyImmediate(objectsContainer);
        }
        objectsContainer = new GameObject(Names.levelObjects);
        objectsContainer.hideFlags = HideFlags.DontSaveInBuild;
        _objectContainerTransform = objectsContainer.transform;

        // Loading tiles.
        _tiles = new Tile[_sizeX, _sizeZ];
        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                SerializedTile serializedTile = GetTile(x, z);
                if (serializedTile != null && (serializedTile.topData != null || serializedTile.objectData != null))
                {
                    Tile tile = Instantiate(_tilePrefab, new Vector3(x, 0f, z), Quaternion.identity, _objectContainerTransform).GetComponent<Tile>();
                    _tiles[x, z] = tile;
                    tile.LoadInEditor(serializedTile, x, z);
                }
            }
        }
    }

    public void ReloadInEditor()
    {
        LoadInEditor(data);
    }

    public void GetAllTowers(List<Tower> list)
    {
        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                Tile tile = GetTile(x, z);
                if (tile != null && tile.tower != null)
                {
                    list.Add(tile.tower);
                }
            }
        }
    }

    public Tile GetTile(Vector3 worldVector)
    {
        return GetTile((int)(worldVector.x + 0.5f), (int)(worldVector.z + 0.5f));
    }

    public Tile GetTile(TilePosition mapPosition)
    {
        return GetTile(mapPosition.x, mapPosition.z);
    }

    public Tile GetTile(int x, int z)
    {
        if (x >= 0 && x < _sizeX && z >= 0 && z < _sizeZ)
        {
            return _tiles[x, z];
        }
        else
        {
            return null;
        }
    }

    public void RemoveTile(int x, int z)
    {
        if (x >= 0 && x < _sizeX && z >= 0 && z < _sizeZ)
        {
            if (_tiles[x, z])
            {
                DestroyImmediate(_tiles[x, z].gameObject);
            }
        }
    }

    public void CreateTile(int x, int z, TileTopData topData, TileBottomData bottomData, TileObjectData objectData)
    {
        if (x >= 0 && x < _sizeX && z >= 0 && z < _sizeZ)
        {
            if (GetTile(x, z) == null)
            {
                SerializedTile serializedTile = GetTile(x, z);
                if (serializedTile == null)
                {
                    serializedTile = new SerializedTile();
                }
                IEditableTile editableTileData = (IEditableTile)serializedTile;
                editableTileData.topData = topData;
                editableTileData.bottomData = bottomData;
                editableTileData.objectData = objectData;
                Tile tile = Instantiate(_tilePrefab, new Vector3(x, 0f, z), Quaternion.identity, _objectContainerTransform).GetComponent<Tile>();
                _tiles[x, z] = tile;
                tile.LoadInEditor(serializedTile, x, z);
            }
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    void MoveBorders(int left, int right, int forward, int back)
    {
        // Finding new array size.
        int newSizeX = _sizeX + right + left;
        int newSizeZ = _sizeZ + forward + back;

        // Checking if size is correct.
        if (newSizeX > 0 && newSizeZ > 0)
        {
            // Creating new array.
            SerializedTile[] newTiles = new SerializedTile[newSizeX * newSizeZ];

            // Finding overlapping range which will be copied to the new array.
            int overlapXMin = Mathf.Max(-left, 0);
            int overlapXMax = Mathf.Min(_sizeX + right, _sizeX);
            int overlapZMin = Mathf.Max(-back, 0);
            int overlapZMax = Mathf.Min(_sizeZ + forward, _sizeZ);

            // Copying overlapping tiles to the new array.
            for (int x = overlapXMin, newX = x + left; x < overlapXMax; x++, newX = x + left)
            {
                for (int z = overlapZMin, newZ = z + back; z < overlapZMax; z++, newZ = z + back)
                {
                    newTiles[IndexFromMapPosition(newX, newZ, newSizeX)] = _tiles[IndexFromMapPosition(x, z, _sizeX)];
                }
            }

            // Applying changes.
            _tiles = newTiles;
            _sizeX = newSizeX;
            _sizeZ = newSizeZ;
        }
    }

    // public SerializedTile GetSerializedTile(Vector3 worldVector)
    // {
    //     return GetSerializedTile((int)(worldVector.x + 0.5f), (int)(worldVector.z + 0.5f));
    // }

    // public SerializedTile GetSerializedTile(TilePosition mapPosition)
    // {
    //     return GetSerializedTile(mapPosition.x, mapPosition.z);
    // }

    // public SerializedTile GetSerializedTile(int x, int z)
    // {
    //     if (x >= 0 && x < _sizeX && z >= 0 && z < _sizeZ)
    //     {
    //         return _serializedTiles[IndexFromMapPosition(x, z, sizeX)];
    //     }
    //     else
    //     {
    //         return null;
    //     }
    // }

    public int IndexFromMapPosition(int x, int z, int sizeX)
    {
        return z * sizeX + x;
    }

    //PRIVATE//////////////////////////////////////////////////
    private TilePosition IndexToMapPosition(int index)
    {
        return new TilePosition(index % _sizeX, index / _sizeX);
    }
}
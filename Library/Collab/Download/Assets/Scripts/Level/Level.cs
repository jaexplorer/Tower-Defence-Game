using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// creates level from level data
/// </summary>

public class Level : MonoBehaviour
{
    [SerializeField] private CompoundMesh _compoundMesh;
    [SerializeField] private GameObject _tilePrefab;

    private Tile[,] _tiles;
    private List<Teleport> _teleports = new List<Teleport>(8);
    private LevelData _data;
    private Transform _objectContainerTransform;
    private Spawner _spawner;
    private Portal _portal;

    //PROPERTIES///////////////////////////////////////////////
    public static Level instance { get; private set; }
    public LevelData data { get { return _data; } }
    public CompoundMesh compoundMesh { get { return _compoundMesh; } }
    public Spawner spawner { get { return _spawner; } }
    public Portal portal { get { return _portal; } }
    public int wavesCount { get { return _data.spawnWaves.Length; } }
    public int sizeX { get { return _data.sizeX; } }
    public int sizeZ { get { return _data.sizeZ; } }
    public int startingEnergy { get { return _data.startingEnergy; } }

    //EVENTS///////////////////////////////////////////////////
    public void Awake()
    {
        instance = this;
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Load(LevelData levelData)
    {
        _data = levelData;
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
        _tiles = new Tile[_data.sizeX, _data.sizeZ];
        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
            {
                SerializedTile serializedTile = _data.GetTile(x, z);
                if (serializedTile.topData != null || serializedTile.objectData != null)
                {
                    Tile tile = PoolManager.Produce(_tilePrefab, _objectContainerTransform, new Vector3(x, 0f, z)).GetComponent<Tile>();
                    tile.Load(serializedTile, x, z, compoundMesh, _teleports);
                    _tiles[x, z] = tile;
                    if (tile.isPortal)
                    {
                        // Debug.Log("portal");
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

        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
            {
                Tile tile = GetTile(x, z);
                if (tile != null)
                {
                    tile.LoadPrebuiltTower();
                }
            }
        }
    }

    public void LoadInEditor(LevelData levelData)
    {
        _data = levelData;
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
        _tiles = new Tile[_data.sizeX, _data.sizeZ];
        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
            {
                SerializedTile serializedTile = _data.GetTile(x, z);
                if (serializedTile != null && (serializedTile.topData != null || serializedTile.objectData != null))
                {
                    Tile tile = Instantiate(_tilePrefab, new Vector3(x, 0f, z), Quaternion.identity, _objectContainerTransform).GetComponent<Tile>();
                    _tiles[x, z] = tile;
                    tile.LoadInEditor(serializedTile, x, z);
                    // Debug.Log(GetTile(x, z));
                }
            }
        }
    }

    public void ReloadInEditor()
    {
        LoadInEditor(data);
    }

    public void OnWaveSave()
    {
        spawner.OnWaveSave();
        portal.OnWaveSave();
        List<Tile.SaveData> tileSaveData = ProfileManager.instance.saveData.tileSaveData;
        tileSaveData.Clear();
        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
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

    public void OnWaveLoad()
    {
        spawner.OnWaveLoad();
        portal.OnWaveLoad();
        List<Tile.SaveData> tileSaveData = ProfileManager.instance.saveData.tileSaveData;

        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
            {
                Tile.SaveData data = tileSaveData[_data.IndexFromMapPosition(x, z, sizeX)];
                if (data != null)
                {
                    _tiles[x, z].LoadSaveData(data);
                }
            }
        }
    }

    public void OnWaveStart()
    {
        _spawner.OnWaveStart();
        int order = 1;//TODO:
        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
            {
                Tile tile = GetTile(x, z);
                if (tile != null && tile.tower != null && !(tile.tower is WallTower))
                {
                    tile.tower.OnWaveStart(order);
                    order++;
                }
                else if (tile != null && tile.wall != null)
                {
                    tile.wall.Reset(order);
                    order++;
                }

            }
        }
    }

    public void GetAllTowers(List<Tower> list)
    {
        for (int z = 0; z < _data.sizeZ; z++)
        {
            for (int x = 0; x < _data.sizeX; x++)
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
        if (x >= 0 && x < _data.sizeX && z >= 0 && z < _data.sizeZ)
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
        if (x >= 0 && x < _data.sizeX && z >= 0 && z < _data.sizeZ)
        {
            if (_tiles[x, z])
            {
                DestroyImmediate(_tiles[x, z].gameObject);
            }
        }
    }

    public void CreateTile(int x, int z, TileTopData topData, TileBottomData bottomData, TileObjectData objectData)
    {
        if (x >= 0 && x < _data.sizeX && z >= 0 && z < _data.sizeZ)
        {
            if (GetTile(x, z) == null)
            {
                SerializedTile serializedTile = _data.GetTile(x, z);
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
}
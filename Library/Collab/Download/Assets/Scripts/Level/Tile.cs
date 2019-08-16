using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;

public class Tile : ManagedObject
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;

    private List<Enemy> _enemies = new List<Enemy>(4);
    private EnemyEvent _onEnemyEnter = new EnemyEvent();
    private EnemyEvent _onEnemyExit = new EnemyEvent();
    private Tower _tower;
    private Frost _fog;
    private Tile _next;
    private Tile _previous;
    private TileCurve _curve;
    private Wall _wall;
    private TilePosition _position;
    private Tweener _moveTweener;
    private Teleport _teleport;
    private Direction _pathDirection;
    private int _pathOrder;
    private int _index;
    private bool _elevated;
    private bool _isPath;
    private SerializedTile _serializedTile;
    private CompoundMesh _compoundMesh;

    private const float _elevationPerLevel = 0.5f;

    //PROPERTIES///////////////////////////////////////////////
    new public GameObject gameObject { get { return _gameObject; } }
    new public Transform transform { get { return _transform; } }
    public MeshFilter meshFilter { get { return _meshFilter; } }
    public MeshRenderer meshRenderer { get { return _meshRenderer; } }
    public bool elevated { get { return _elevated; } }
    public TilePosition position { get { return _position; } }
    public List<Enemy> enemies { get { return _enemies; } }
    public EnemyEvent onEnemyEnter { get { return _onEnemyEnter; } }
    public EnemyEvent onEnemyExit { get { return _onEnemyExit; } }
    public Teleport teleport { get { return _teleport; } }
    public SerializedTile serializedTile { get { return _serializedTile; } }
    public bool walkable { get { return _serializedTile.topData && _serializedTile.topData.walkable && !_elevated && _serializedTile.level == 0 && (!_serializedTile.objectData || _serializedTile.objectData.walkable); } }
    public bool elevatable { get { return _serializedTile.topData && _serializedTile.topData.elevatable; } }
    public bool isPortal { get { return _serializedTile.objectData && _serializedTile.objectData.type == TileObjectData.Type.Portal; } }
    public bool isTeleport { get { return _serializedTile.objectData && _serializedTile.objectData.type == TileObjectData.Type.Teleport; } }
    public bool isSpawner { get { return _serializedTile.objectData && _serializedTile.objectData.type == TileObjectData.Type.Spawner; } }
    public bool hasSocket { get { return _serializedTile.topData.hasSocket; } }
    public Tower tower { get { return _tower; } set { _tower = value; } }
    public Wall wall { get { return _wall; } set { _wall = value; } }
    public TileCurve curve { get { return _curve; } set { _curve = value; } }
    public Tile next { get { return _next; } set { _next = value; } }
    public Tile previous { get { return _previous; } set { _previous = value; } }
    public int pathOrder { get { return _pathOrder; } set { _pathOrder = value; } }
    public bool isPath { get { return _isPath; } set { _isPath = value; } }
    public Frost fog { get { return _fog; } set { _fog = value; } }
    public Direction pathDirection { get { return _pathDirection; } set { _pathDirection = value; } }

    //PUBLIC///////////////////////////////////////////////////
    public void Load(SerializedTile serializedTile, int x, int z, CompoundMesh compoundMesh, List<Teleport> teleports)
    {
        // Setup.
        _compoundMesh = compoundMesh;
        _serializedTile = serializedTile;
        _position = new TilePosition(x, z);
        SetElevation(serializedTile.elevated);
        _transform.position += new Vector3(0f, _serializedTile.level * _elevationPerLevel, 0f);

        // This is ugly, probably needs to bechanged.
        Events.onLevelClear.RemoveListener(Recycle);

        // Spawn prefabs.
        if (serializedTile.objectData && serializedTile.objectData.prefab)
        {
            PoolManager.Produce(serializedTile.objectData.prefab, transform);
        }
        if (serializedTile.bottomData && serializedTile.bottomData.prefab)
        {
            serializedTile.bottomData.prefab.GetCopy(transform);
        }

        // Connect teleports.
        if (isTeleport)
        {
            _teleport = gameObject.GetComponentInChildren<Teleport>();
            for (int i = 0; i < teleports.Count; i++)
            {
                if (teleports[i].tile._serializedTile.objectData == _teleport.tile._serializedTile.objectData)
                {
                    _teleport.pair = teleports[i];
                    // _teleports.RemoveAt(i);
                    teleports[i].pair = _teleport;
                }
            }
            if (_teleport.pair == null)
            {
                teleports.Add(_teleport);
            }
        }

        //TODO: This code is necessary for optimization, gonna need to implement it in GenerateMesh method.
        // Tile tileDiagonal = Level.instance.GetTile(_position.x - 1, _position.z - 1);
        // Tile tileLeft = Level.instance.GetTile(_position.x - 1, _position.z);
        // Tile tileRight = Level.instance.GetTile(_position.x, _position.z - 1);
        // bool leftOpen = true;
        // bool leftTopOpen = false;
        // bool rightOpen = true;
        // bool rightTopOpen = false;
        // if (tileLeft != null)
        // {
        //     leftOpen = false;
        //     if (elevatable || (serializedTile.elevated && (tileLeft.elevatable || !tileLeft.IsBlockingView())))
        //     {
        //         leftTopOpen = true;
        //     }
        // }
        // if (tileRight != null)
        // {
        //     rightOpen = false;
        //     if (elevatable || (serializedTile.elevated && (tileRight.elevatable || !tileRight.IsBlockingView())))
        //     {
        //         rightTopOpen = true;
        //     }
        // }
        // if (tileRight != null && tileLeft != null && tileDiagonal == null)
        // {
        //     rightTopOpen = false;
        //     leftOpen = false;
        //     rightOpen = true;
        // }
        GenerateMesh();
        Level.instance.compoundMesh.AddMesh(_meshFilter);
        _meshRenderer.enabled = false;
    }

    public void LoadInEditor(SerializedTile serializedTile, int x, int z)
    {
        // _gameObject.hideFlags = HideFlags.DontSave;
        _serializedTile = serializedTile;
        _position = new TilePosition(x, z);
        SetElevation(serializedTile.elevated);
        GenerateMesh();
        _transform.position += new Vector3(0f, _serializedTile.level * 0.5f, 0f);
        if (tower != null)
        {
            DestroyImmediate(tower.gameObject);
        }
        if (_serializedTile.towerData != null)
        {
            Tower newTower = GameObject.Instantiate(_serializedTile.towerData.towerPrefab).GetComponent<Tower>();
            tower = newTower;
            newTower.transform.parent = _transform;
            newTower.transform.localPosition = Vector3.zero;
        }
    }

    public void ReloadInEditor()
    {
        LoadInEditor(_serializedTile, _position.x, _position.z);
    }

    public void LoadPrebuiltTower()
    {
        if (_serializedTile.towerData)
        {
            BuildTower(_serializedTile.towerData);
        }
    }

    public void OnEnemyEnter(Enemy enemy)
    {
        _enemies.Add(enemy);
        _onEnemyEnter.Invoke(enemy);
    }

    public void OnEnemyExit(Enemy enemy)
    {
        _enemies.Remove(enemy);
        _onEnemyExit.Invoke(enemy);
    }

    public void Elevate()
    {
        if (!_elevated)
        {
            Move(0.25f);
            _elevated = true;
        }
    }

    public void Lower()
    {
        if (_elevated)
        {
            Move(0.0f);
            _elevated = false;
        }
    }

    private void Move(float y)
    {
        _moveTweener.Kill();
        Vector3 localPosition = transform.localPosition;
        _moveTweener = _transform.DOMove(new Vector3(localPosition.x, y, localPosition.z), 1f).SetSpeedBased().OnComplete(OnStop);
        _compoundMesh.HideMesh(_meshFilter);
        _meshRenderer.enabled = true;
    }

    public void OnStop()
    {
        _compoundMesh.UpdatePositionAndShow(_meshFilter);
        _meshRenderer.enabled = false;
    }

    public void SetElevation(bool elevated)
    {
        if (elevated)
        {
            _transform.localPosition = new Vector3(_position.x, 0.25f, _position.z);
        }
        else
        {
            _transform.localPosition = new Vector3(_position.x, 0f, _position.z);
        }
        _elevated = elevated;
    }

    public void ToggleElevation()
    {
        if (!_elevated)
        {
            Elevate();
        }
        else
        {
            Lower();
        }
    }

    public void ClearPath()
    {
        _isPath = false;
        _onEnemyEnter.RemoveAllListeners();
        _onEnemyExit.RemoveAllListeners();
    }

    public bool IsOpen()
    {
        return walkable; //TODO: add more checks.
    }

    public bool IsBlockingView()
    {
        return !_serializedTile.topData.elevatable && _elevated;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.elevated = _elevated;
        if (_tower)
        {
            saveData.towerData = _tower.data;
            return saveData;
        }
        if (elevatable)
        {
            saveData.elevated = _elevated;
            return saveData;
        }
        return null;
    }

    public void LoadSaveData(SaveData saveData)
    {
        _tower = null;
        if (saveData.towerData)
        {
            BuildTower(saveData.towerData);
        }
        else
        {
            SetElevation(saveData.elevated);
        }
    }

    private void BuildTower(TowerData towerData)
    {
        if (!elevated)
        {
            SetElevation(true);
        }
        Tower newTower = PoolManager.Produce(towerData.towerPrefab, null, _transform.position).GetComponent<Tower>();
        _tower = newTower;
    }

    private static List<Vector3> _vertices = new List<Vector3>(1000);
    private static List<Vector3> _normals = new List<Vector3>(1000);
    private static List<Vector2> _uv = new List<Vector2>(1000);
    private static List<int> _triangles = new List<int>(1000 * 3);
    private static List<Mesh> _originalMeshes = new List<Mesh>(6);
    private static List<bool> _flipFlags = new List<bool>(6);

    private void GenerateMesh()
    {
        if (_meshFilter.sharedMesh == null)
        {
            _meshFilter.sharedMesh = new Mesh();
        }
        Mesh mesh = _meshFilter.sharedMesh;
        mesh.Clear();
        _vertices.Clear();
        _normals.Clear();
        _uv.Clear();
        _triangles.Clear();
        _originalMeshes.Clear();
        _flipFlags.Clear();
        Quaternion rotation = DirectionToRotation(_serializedTile.direction);

        if (serializedTile.topData != null && serializedTile.topData.mesh != null)
        {
            _originalMeshes.Add(serializedTile.topData.mesh);
        }
        if (serializedTile.bottomData != null && serializedTile.bottomData.mesh != null)
        {
            _originalMeshes.Add(serializedTile.bottomData.mesh);
        }
        if (serializedTile.objectData != null && serializedTile.objectData.mesh != null)
        {
            _originalMeshes.Add(serializedTile.objectData.mesh);
        }

        for (int i = 0; i < _originalMeshes.Count; i++)
        {
            int vertexIndex = _vertices.Count;
            int triangleIndex = _triangles.Count;
            Mesh currentMesh = _originalMeshes[i];
            _vertices.AddRange(currentMesh.vertices);

            _normals.AddRange(currentMesh.normals);
            _triangles.AddRange(currentMesh.triangles);
            for (int j = 0; j < _vertices.Count; j++)
            {
                _vertices[j] = rotation * _vertices[j];
            }
            for (int j = 0; j < _normals.Count; j++)
            {
                _normals[j] = rotation * _normals[j];
            }
            for (int j = triangleIndex; j < _triangles.Count; j++)
            {
                _triangles[j] = _triangles[j] + vertexIndex;
            }
            _uv.AddRange(currentMesh.uv);
        }
        mesh.SetVertices(_vertices);
        mesh.SetTriangles(_triangles, 0);
        mesh.SetUVs(0, _uv);
        mesh.RecalculateNormals();
    }

    private Quaternion DirectionToRotation(Direction direction)
    {
        if (direction == Direction.Left)
        {
            return Quaternion.Euler(0f, -90f, 0f);
        }
        if (direction == Direction.Right)
        {
            return Quaternion.Euler(0f, 90f, 0f);
        }
        if (direction == Direction.Backward)
        {
            return Quaternion.Euler(0f, 180f, 0f);
        }
        return Quaternion.identity;
    }

    public class SaveData
    {
        [SerializeField] public bool elevated;
        [SerializeField] public TowerData towerData;
        [SerializeField] public short index;
    }
}
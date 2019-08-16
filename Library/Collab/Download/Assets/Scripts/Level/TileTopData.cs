using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TileData", menuName = "Data/TileData", order = 1)]
public class TileData : ScriptableObject
{
    [SerializeField] private List<Collection> _collections;
    [SerializeField] private TileType _tileType;
    [SerializeField] private bool _liftable;
    [SerializeField] private bool _walkable;
    [SerializeField] private bool _isStatic;
    [SerializeField] private TowerData _prebuiltTower;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<Mesh> _meshesTop;
    [SerializeField] private List<Mesh> _meshesSideOpenLeft;
    [SerializeField] private List<Mesh> _meshesSideOpenRight;
    [SerializeField] private Mesh _meshSideUpper;

    private List<Vector3> _vertices = new List<Vector3>(1000);
    private List<Vector3> _verticesFlipping = new List<Vector3>(1000);
    private List<Vector3> _normals = new List<Vector3>(1000);
    private List<Vector2> _uv = new List<Vector2>(1000);
    private List<int> _triangles = new List<int>(1000 * 3);
    private List<Mesh> _originalMeshes = new List<Mesh>(3);
    private List<bool> _flipFlags = new List<bool>(3);
    private Quaternion _flipRotation = Quaternion.Euler(0f, 90f, 0f);
    private Vector3 _flipScale = new Vector3(-1f, 1f, 1f);
    private Vector3 _normalFlipScale = new Vector3(1f, 1f, 1f);

    //PROPERTIES///////////////////////////////////////////////
    public bool walkable
    {
        get { return _walkable; }
    }

    public bool elevatable
    {
        get { return _liftable; }
    }

    public GameObject prefab
    {
        get { return _prefab; }
    }

    public bool isPortal
    {
        get { return _tileType == TileType.Portal; }
    }

    public bool isTeleport
    {
        get { return _tileType == TileType.Teleport; }
    }

    public bool isSpawner
    {
        get { return _tileType == TileType.Spawner; }
    }

    // public bool canBuild
    // {
    //     get { return _canBuild; }
    // }

    public TowerData tower
    {
        get { return _prebuiltTower; }
    }

    public TileType tileType
    {
        get { return _tileType; }
    }

    //PUBLIC///////////////////////////////////////////////////
    public Mesh GetMesh(byte variation, bool upperLeft, bool upperRight, bool openLeft, bool openRight)
    {
        Mesh mesh = new Mesh();
        _vertices.Clear();
        _normals.Clear();
        _uv.Clear();
        _triangles.Clear();
        _originalMeshes.Clear();
        _flipFlags.Clear();
        _originalMeshes.Add(_meshesTop[variation]);
        _flipFlags.Add(false);

        if (upperLeft)
        {
            _originalMeshes.Add(_meshSideUpper);
            _flipFlags.Add(true);
        }
        else if (openLeft)
        {
            _originalMeshes.Add(_meshesSideOpenLeft[variation]);
            _flipFlags.Add(true);
        }
        if (upperRight)
        {
            _originalMeshes.Add(_meshSideUpper);
            _flipFlags.Add(false);
        }
        else if (openRight)
        {
            _originalMeshes.Add(_meshesSideOpenRight[variation]);
            _flipFlags.Add(false);
        }
        for (int i = 0; i < _originalMeshes.Count; i++)
        {
            int vertexIndex = _vertices.Count;
            int triangleIndex = _triangles.Count;
            Mesh currentMesh = _originalMeshes[i];
            _vertices.AddRange(currentMesh.vertices);
            _normals.AddRange(currentMesh.normals);
            _triangles.AddRange(currentMesh.triangles);
            for (int j = triangleIndex; j < _triangles.Count; j++)
            {
                _triangles[j] = _triangles[j] + vertexIndex;
            }
            if (_flipFlags[i])
            {
                for (int j = vertexIndex; j < _vertices.Count; j++)
                {
                    _vertices[j] = _flipRotation * Vector3.Scale(_vertices[j], _flipScale);
                }

                for (int j = triangleIndex; j < _triangles.Count; j += 3)
                {
                    int t0 = _triangles[j];
                    int t3 = _triangles[j + 2];
                    _triangles[j] = t3;
                    _triangles[j + 2] = t0;
                }
            }
            _uv.AddRange(currentMesh.uv);
        }
        mesh.SetVertices(_vertices);
        mesh.SetTriangles(_triangles, 0);
        mesh.SetUVs(0, _uv);
        mesh.RecalculateNormals();
        return mesh;
    }

    public bool IsInCollection(Collection collection)
    {
        return collection == Collection.Any || _collections.Contains(collection);
    }

    public bool CompareTileType(TileType type)
    {
        return type == TileType.Any || _tileType == type;
    }

    //ENUMS//////////////////////////////////////////////////////
    public enum Collection
    {
        Any,
        Green,
        Autumn,
        Dark
    }

    public enum TileType
    {
        Any,
        Tile,
        Foundation,
        Portal,
        Spawner,
        Teleport,
        Decor,
    }
}
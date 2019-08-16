using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FogTower : Tower
{
    [SerializeField] private MeshFilter _fogMeshFilter;
    [SerializeField] private MeshRenderer _fogMeshRenderer;
    [SerializeField] private Mesh _meshTop;
    [SerializeField] private Mesh _meshRight;
    [SerializeField] private Mesh _meshLeft;
    [SerializeField] private FogTowerData _fogData;

    private DynamicMesh _dynamicMesh;
    private Tile _targetTile;
    private List<Fog> _fog = new List<Fog>(7);
    private List<int> _distances = new List<int>(7);

    private static Vector3[] _tileVectors = new Vector3[] { new Vector3(0, 0f, 1), new Vector3(1, 0f, 0), new Vector3(0, 0f, -1), new Vector3(-1, 0f, 0) };

    public int freezeRate
    {
        get { return _data.debuff; }
    }

    public int freezeMax
    {
        get { return _data.debuffMax; }
    }

    public FogTowerData fogData
    {
        get { return _fogData; }
    }

    private List<Tile> _tileQueue = new List<Tile>(16);
    private List<Tile> _newTileQueue = new List<Tile>(16);
    private List<Tile> _blockedTileQueue = new List<Tile>(16);
    //PUBLIC///////////////////////////////////////////////////
    public override void GetTiles(TilePosition tilePosition, List<Tile> pathTiles, List<Tile> validTiles = null, List<Tile> blockedTiles = null)
    {
        Tile targetTile = null;
        for (int i = 0; i < _tileVectors.Length; i++)
        {
            Tile tile = Level.instance.GetTile(tilePosition.ToVector3() + _tileVectors[i]);
            if ((targetTile == null || tile != null && tile.pathOrder < targetTile.pathOrder))
            {
                if (IsTileValid(tile))
                {
                    targetTile = tile;
                }
            }
        }
        if (targetTile != null)
        {
            if (validTiles != null)
            {
                validTiles.Add(targetTile);
            }

            _tileQueue.Clear();
            _newTileQueue.Clear();
            _blockedTileQueue.Clear();
            _distances.Clear();
            _distances.Add(1);
            float tileCount = 1;
            int distance = 1;
            _tileQueue.Add(targetTile);

            for (int i = 0; i < _tileQueue.Count; i++)
            {
                Tile tile = _tileQueue[i];
                for (int j = 0; j < _tileVectors.Length; j++)
                {
                    Tile adjacentTile = Level.instance.GetTile(tile.position.ToVector3() + _tileVectors[j]);
                    if (adjacentTile != null && !_tileQueue.Contains(adjacentTile) && !_newTileQueue.Contains(adjacentTile))
                    {
                        if (IsTileValid(adjacentTile) && !_blockedTileQueue.Contains(tile))
                        {
                            if (validTiles != null)
                            {
                                validTiles.Add(adjacentTile);
                                _distances.Add(distance);
                            }
                            if (adjacentTile.isPath && pathTiles != null)
                            {
                                pathTiles.Add(adjacentTile);
                            }
                            _newTileQueue.Add(adjacentTile);
                            tileCount++;
                        }
                        else if (adjacentTile.IsWalkable())
                        {
                            _newTileQueue.Add(adjacentTile);
                            _blockedTileQueue.Add(adjacentTile);
                            if (blockedTiles != null)
                            {
                                blockedTiles.Add(adjacentTile);
                            }
                            tileCount++;
                            break;
                        }
                    }
                }
                if (i == _tileQueue.Count - 1)
                {
                    if (tileCount >= _data.range)
                    {
                        break;
                    }
                    else
                    {
                        distance++;
                        _tileQueue.AddRange(_newTileQueue);
                        _newTileQueue.Clear();
                    }
                }
            }
        }
    }

    public bool IsTileValid(Tile tile)
    {
        if (tile != null && tile.IsWalkable())
        {
            int openTilesCount = 0;
            for (int j = 0; j < _tileVectors.Length; j++)
            {
                Tile adjacentTile = Level.instance.GetTile(tile.position.ToVector3() + _tileVectors[j]);
                if (adjacentTile != null && adjacentTile.IsOpen())
                {
                    openTilesCount++;
                }
            }
            return openTilesCount <= 4; //TODO
        }
        else
        {
            return false;
        }
    }

    public override void OnEnemyEnter(Enemy enemy)
    {

    }

    //PROTECTED////////////////////////////////////////////////
    protected override void MarkTiles()
    {
        if (_tiles == null)
        {
            _tiles = new List<Tile>(5);
        }
        UnmarkTiles();

        // Marking fog.
        GetTiles(_tile.position, null, _tiles);
        for (int i = 0; i < _tiles.Count; i++)
        {
            Tile tile = _tiles[i];
            if (!tile.fog)
            {
                tile.fog = Pool.Produce(_fogData.fogPrefab, null, tile.position.ToVector3()).GetComponent<Fog>();
                tile.fog.tile = tile;
            }
            tile.fog.AddTower(this, _distances[i]);
        }

        // _tileQueue.Clear();
        // _newTileQueue.Clear();
        // _tileQueue.Add(_tile);
        // for (int i = 0; i < _tiles.Count; i++)
        // {

        //     Tile tile = _tiles[i];
        //     if (!tile.fog.isShown)
        //     {
        //         _tileQueue.Add(_tile);

        //         tile.fog = PoolManager.Produce(_fogData.fogPrefab, null, tile.position.ToVector3()).GetComponent<Fog>();
        //         tile.fog.tile = tile;
        //     }
        //     tile.fog.AddTower(this, i);
        // }
    }

    protected override void UnmarkTiles()
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (_tiles[i].fog != null)
            {
                _tiles[i].fog.RemoveTower(this);
            }
        }
        _tiles.Clear();
    }

    protected override void OnProduce()
    {
        base.OnProduce();
        EventManager.onLevelChange.AddListener(OnLevelChange);
        MarkTiles();
    }

    protected override void OnRecycle()
    {
        UnmarkTiles();
        EventManager.onLevelChange.RemoveListener(OnLevelChange);
    }

    //PRIVATE//////////////////////////////////////////////////
    private void OnLevelChange()
    {
        MarkTiles();
    }
}

public class DynamicMesh
{
    private Mesh _mesh;
    private List<Vector3> _vertices;
    private List<Vector3> _normals;
    private List<Vector2> _uv;
    private List<int> _triangles;

    public Mesh mesh
    {
        get { return _mesh; }
    }

    public List<Vector3> vertices
    {
        get { return _vertices; }
    }

    public List<Vector3> normals
    {
        get { return _normals; }
    }

    public List<Vector2> uv
    {
        get { return _uv; }
    }

    public List<int> triangles
    {
        get { return _triangles; }
    }

    public void Clear()
    {
        _vertices.Clear();
        _normals.Clear();
        _uv.Clear();
        _triangles.Clear();
        _mesh.Clear();
    }

    public DynamicMesh(int capacity)
    {
        _vertices = new List<Vector3>(capacity);
        _normals = new List<Vector3>(capacity);
        _uv = new List<Vector2>(capacity);
        _triangles = new List<int>(capacity * 3);
        _mesh = new Mesh();
        _mesh.SetVertices(_vertices);
        _mesh.SetTriangles(_triangles, 0);
        _mesh.SetUVs(0, _uv);
        _mesh.SetNormals(_normals);
        _mesh.MarkDynamic();
    }

    public void SetMesh(Mesh mesh)
    {

    }

    public void AddMesh(Mesh mesh, Vector3 position)
    {
        int vertexIndex = _vertices.Count;
        _vertices.AddRange(mesh.vertices);
        for (int i = vertexIndex; i < _vertices.Count; i++)
        {
            _vertices[i] += position;
        }
        _normals.AddRange(mesh.normals);
        _uv.AddRange(mesh.uv);
        int triangleIndex = _triangles.Count;
        _triangles.AddRange(mesh.triangles);
        for (int i = triangleIndex; i < _triangles.Count; i++)
        {
            _triangles[i] += vertexIndex;
        }
        _mesh.SetVertices(_vertices);
        _mesh.SetTriangles(_triangles, 0);
        _mesh.SetUVs(0, _uv);
        _mesh.SetNormals(_normals);
    }
}


// private List<Tile> _tileQueue = new List<Tile>(16);
// private List<Tile> _newTileQueue = new List<Tile>(16);
// public void GetTilesOld(TilePosition tilePosition, List<Tile> pathTiles, List<Tile> validTiles = null, List<Tile> blockedTiles = null)
// {

// _tileQueue.Clear();
// _newTileQueue.Clear();
// float tileCount = 0;
// Tile chosenTile = null;
// for (int i = 0; i < _data.markers.Length; i++)
// {
//     Tile tile = Level.current.GetTile(tilePosition.ToVector3() + _data.markers[i]);
//     if (tile != null && tile.IsWalkable() && tile.isPath && (chosenTile == null || tile.pathOrder < chosenTile.pathOrder))
//     {
//         chosenTile = tile;
//         // _tileQueue.Add(tile);
//         tileCount++;
//     }
// }
// _tileQueue.Add(chosenTile);
// Tile _towerTile = Level.current.GetTile(tilePosition);
// for (int i = 0; i < _data.markers.Length; i++)
// {
//     Tile tile = Level.current.GetTile(tilePosition.ToVector3() + _data.markers[i]);
//     if (tile != null && tile.IsWalkable())
//     {
//         _tileQueue.Add(tile);
//         tileCount++;
//     }
// }
// for (int i = 0; i < _tileQueue.Count; i++)
// {
//     Tile tile = _tileQueue[i];
//     if (tile != null && tile.IsWalkable())
//     {
//         if (validTiles != null)
//         {
//             validTiles.Add(tile);
//         }
//         if (tile.isPath && pathTiles != null)
//         {
//             pathTiles.Add(tile);
//         }
//     }
//     for (int j = 0; j < tileVectors.Length; j++)
//     {
//         Tile adjacentTile = Level.current.GetTile(tile.position.ToVector3() + tileVectors[j]);
//         if (!_tileQueue.Contains(adjacentTile) && !_newTileQueue.Contains(adjacentTile))
//         {
//             if (adjacentTile != null && adjacentTile.IsWalkable())
//             {
//                 _newTileQueue.Add(adjacentTile);
//                 tileCount++;
//             }
//             else if (adjacentTile == null || adjacentTile.IsEmpty())
//             {
//                 tileCount++;
//             }
//         }
//     }
//     if (i == _tileQueue.Count - 1)
//     {
//         if (tileCount > _data.range)
//         {
//             break;
//         }
//         else
//         {
//             _tileQueue.AddRange(_newTileQueue);
//             _newTileQueue.Clear();
//         }
//     }
// }
//     // _tileQueue.Clear();
//     // _newTileQueue.Clear();
//     // float tileCount = 0;
//     // Tile _towerTile = Level.current.GetTile(tilePosition);
//     // for (int i = 0; i < _data.markers.Length; i++)
//     // {
//     //     Tile tile = Level.current.GetTile(tilePosition.ToVector3() + _data.markers[i]);
//     //     if (tile != null && tile.IsWalkable())
//     //     {
//     //         _tileQueue.Add(tile);
//     //         tileCount++;
//     //     }
//     // }
//     // for (int i = 0; i < _tileQueue.Count; i++)
//     // {
//     //     Tile tile = _tileQueue[i];
//     //     if (tile != null && tile.IsWalkable())
//     //     {
//     //         if (validTiles != null)
//     //         {
//     //             validTiles.Add(tile);
//     //         }
//     //         if (tile.isPath && pathTiles != null)
//     //         {
//     //             pathTiles.Add(tile);
//     //         }
//     //     }
//     //     for (int j = 0; j < tileVectors.Length; j++)
//     //     {
//     //         Tile adjacentTile = Level.current.GetTile(tile.position.ToVector3() + tileVectors[j]);
//     //         if (!_tileQueue.Contains(adjacentTile) && !_newTileQueue.Contains(adjacentTile))
//     //         {
//     //             if (adjacentTile != null && adjacentTile.IsWalkable())
//     //             {
//     //                 _newTileQueue.Add(adjacentTile);
//     //                 tileCount++;
//     //             }
//     //             else if (adjacentTile == null || adjacentTile.IsEmpty())
//     //             {
//     //                 tileCount++;
//     //             }
//     //         }
//     //     }
//     //     if (i == _tileQueue.Count - 1)
//     //     {
//     //         if (tileCount > _data.range)
//     //         {
//     //             break;
//     //         }
//     //         else
//     //         {
//     //             _tileQueue.AddRange(_newTileQueue);
//     //             _newTileQueue.Clear();
//     //         }
//     //     }
//     // }
// }

// public struct FogDistanceData
// {
//     public FogTower tower;
//     public FogTower distance;
// }
using UnityEngine;
using System.Collections.Generic;

public class LineTowerTrigger : MonoBehaviour
{
    [SerializeField] private int _xMultiplier;
    [SerializeField] private int _zMultiplier;
    [SerializeField] private LineTower _tower;
    private List<Tile> _tiles;
    private List<Enemy> _enemies = new List<Enemy>(8);
    private MapVector _launchVector;
    private int _range;

    //PROPERTIES///////////////////////////////////////////////
    public List<Enemy> enemies { get { return _enemies; } }
    public MapVector launchVector { get { return _launchVector; } }
    public int range { get { return _range; } }
    public int xMultiplier { get { return _xMultiplier; } }
    public int zMultiplier { get { return _zMultiplier; } }

    //PUBLIC///////////////////////////////////////////////////
    public void OnWaveStarted()
    {
        _enemies.Clear();
        MarkTiles();
    }

    public void MarkTiles()
    {
        if (_tiles == null)
        {
            _tiles = new List<Tile>(_tower.data.range);
        }
        else
        {
            UnmarkTiles();
        }
        GetTiles(_tower.tile.position, null, _tiles);
        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].onEnemyEnter.AddListener(OnEnemyEnter);
            _tiles[i].onEnemyExit.AddListener(OnEnemyExit);
            _range = (i + 1) * MapVector.resolution;
        }
        _launchVector = new MapVector(_tower.tile.position);
        _launchVector.x += (MapVector.resolution / 2) * _xMultiplier;
        _launchVector.z += (MapVector.resolution / 2) * _zMultiplier;
    }

    public void GetTiles(TilePosition position, List<Tile> pathTiles, List<Tile> validTiles = null, List<Tile> blockedTiles = null)
    {
        bool blocked = false;
        for (int i = 1; i <= _tower.data.range; i++)
        {
            Tile tile = Level.instance.GetTile(position.x + i * _xMultiplier, position.z + i * _zMultiplier);
            if (!blocked && tile != null && tile.walkable)
            {
                if (tile.isPath && pathTiles != null)
                {
                    pathTiles.Add(tile);
                }
                else if (validTiles != null)
                {
                    validTiles.Add(tile);
                }
            }
            else
            {
                if (blockedTiles != null)
                {
                    blockedTiles.Add(tile);
                }
                blocked = true;
            }
        }
    }

    public void UnmarkTiles()
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].onEnemyEnter.RemoveListener(OnEnemyEnter);
            _tiles[i].onEnemyExit.RemoveListener(OnEnemyExit);
        }
        _range = 0;
        _tiles.Clear();
    }

    // public void OnRecycle()
    // {
    //     for (int i = 0; i < _tiles.Count; i++)
    //     {
    //         _tiles[i].onEnemyEnter.RemoveListener(OnEnemyEnter);
    //         _tiles[i].onEnemyExit.RemoveListener(OnEnemyExit);
    //     }
    // }

    public void OnEnemyEnter(Enemy enemy)
    {
        _enemies.Add(enemy);
        _tower.OnEnemyEnter(enemy);
    }

    public void OnEnemyExit(Enemy enemy)
    {
        _enemies.Remove(enemy);
        _tower.OnEnemyExit(enemy);
    }
}
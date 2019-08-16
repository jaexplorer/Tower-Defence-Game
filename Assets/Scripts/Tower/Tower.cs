using UnityEngine;
using System.Collections.Generic;

public class Tower : ManagedObject
{
    [SerializeField] protected Transform _transform;
    [SerializeField] protected TowerData _data;
    [SerializeField] protected MeshRenderer _meshRenderer;
    [SerializeField] protected MeshFilter _meshFilter;
    [SerializeField] protected AudioSource _audioSource;

    protected List<Tile> _tiles;
    protected Tile _tile;
    protected int _enemyCount;
    protected int _cooldown;

    //PROPERTIES///////////////////////////////////////////////
    public TowerData data { get { return _data; } }
    public Tile tile { get { return _tile; } }
    public new int updatePriority { get { return _tile.priority; } }

    //EVENTS///////////////////////////////////////////////////
    protected override void OnProduce()
    {
        _tile = Level.instance.GetTile(_transform.position);
    }

    protected override void ManagedUpdate()
    {
        if (_cooldown > 0)
        {
            _cooldown--;
            if (_cooldown <= 0 && _enemyCount > 0)
            {
                Shoot();
            }
        }
    }

    protected override void OnWaveStart()
    {
        _cooldown = 0;
        _enemyCount = 0;
        MarkTiles();
    }

    public virtual void OnEnemyEnter(Enemy enemy)
    {
        _enemyCount++;
        if (_enemyCount == 1 && _cooldown <= 0)
        {
            Shoot();
        }
    }

    public virtual void OnEnemyExit(Enemy enemy)
    {
        _enemyCount--;
    }

    //PUBLIC///////////////////////////////////////////////////
    public virtual void GetTiles(TilePosition tilePosition, List<Tile> pathTiles, List<Tile> validTiles = null, List<Tile> blockedTiles = null)
    {
        for (int i = 0; i < _data.markers.Length; i++)
        {
            Tile tile = Level.instance.GetTile(tilePosition.ToVector3() + _data.markers[i]);
            if (tile != null && tile.walkable)
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
            else if (blockedTiles != null)
            {
                blockedTiles.Add(tile);
            }
        }
    }

    public void Cooldown()
    {
        _cooldown = _data.cooldown;
    }

    //PROTECTED////////////////////////////////////////////////
    protected virtual void MarkTiles()
    {
        if (_tiles == null)
        {
            _tiles = new List<Tile>(_data.markers.Length);
        }
        else if (_tiles.Count > 0)
        {
            UnmarkTiles();
        }
        GetTiles(_tile.position, _tiles);
        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].onEnemyEnter.AddListener(OnEnemyEnter);
            _tiles[i].onEnemyExit.AddListener(OnEnemyExit);
        }
        _tiles.Sort(delegate (Tile a, Tile b)
        {
            if (a.pathOrder > b.pathOrder)
                return 1;
            else
                return -1;
        });
    }

    protected virtual void UnmarkTiles()
    {
        if (_tiles != null)
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                _tiles[i].onEnemyEnter.RemoveListener(OnEnemyEnter);
                _tiles[i].onEnemyExit.RemoveListener(OnEnemyExit);
            }
            _tiles.Clear();
        }
    }

    protected virtual void Shoot()
    {
        ProduceProjectile().Launch(GetFirstTarget());
        Cooldown();
    }

    protected Projectile ProduceProjectile()
    {
        GameObject projectilePrefab;
        // return (Instantiate(_data.projectilePrefab, _transform.position + _data.launcherPoint, Quaternion.identity) as GameObject).GetComponent<Projectile>();
        return _data.projectilePrefab.GetCopy(null, _transform.position + _data.launcherPoint).GetComponent<Projectile>();

        Projectile projectile = projectilePrefab.GetCopy(null, _transform.position + _data.launcherPoint).GetComponent<Projectile>();
        projectile.gameObject.Recycle();
    }

    protected Enemy GetLastTarget()
    {
        Enemy enemy = null;
        for (int i = 0; i < _tiles.Count; i++)
        {
            Tile tile = _tiles[i];
            if (tile.enemies.Count > 0)
            {
                return tile.enemies[tile.enemies.Count - 1];
            }
        }
        Debug.LogError("LAST ENEMY NOT FOUND");
        return enemy;
    }

    protected Enemy GetFirstTarget()
    {
        Enemy enemy = null;
        for (int i = _tiles.Count - 1; i > -1; i--)
        {
            Tile tile = _tiles[i];
            if (tile.enemies.Count > 0)
            {
                return tile.enemies[tile.enemies.Count - 1];
            }
        }
        Debug.LogError("FIRST ENEMY NOT FOUND");
        return enemy;
    }

    protected override void OnRecycle()
    {
        UnmarkTiles();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTower : Tower
{
    private static Vector2[] multipliers = new Vector2[] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

    //EVENTS///////////////////////////////////////////////////
    private void OnTileChange()
    {
        UpdateWalls(false);
    }

    //PUBLIC///////////////////////////////////////////////////
    public override void GetTiles(TilePosition tilePosition, List<Tile> pathTiles, List<Tile> validTiles = null, List<Tile> blockedTiles = null)
    {
        for (int i = 0; i < multipliers.Length; i++)
        {
            int xMultiplier = (int)multipliers[i].x;
            int zMultiplier = (int)multipliers[i].y;
            Tile markTile = Level.instance.GetTile(tilePosition.x + (1 * xMultiplier), tilePosition.z + (1 * zMultiplier));
            if (markTile != null && markTile.walkable)
            {
                if (validTiles != null)
                {
                    validTiles.Add(markTile);
                }
                if (pathTiles != null)
                {
                    Tile towerTile = Level.instance.GetTile(tilePosition.x + (2 * xMultiplier), tilePosition.z + (2 * zMultiplier));
                    if (towerTile != null && towerTile.tower && towerTile.tower is WallTower)
                    {
                        pathTiles.Add(markTile);
                    }
                }
            }
            else if (blockedTiles != null)
            {
                blockedTiles.Add(markTile);
            }
        }
    }

    //PROTECTED////////////////////////////////////////////////
    protected override void OnProduce()
    {
        base.OnProduce();
        MarkTiles();
        UpdateWalls(false);
    }

    protected override void OnRecycle()
    {
        UnmarkTiles();
        UpdateWalls(true);
    }

    protected override void MarkTiles()
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
        // for (int i = 0; i < _tiles.Count; i++)
        // {
        //     if (_tiles[i].data.elevatable)
        //     {
        //         _tiles[i].onChange.AddListener(OnTileChange);
        //     }
        // }
    }

    protected override void UnmarkTiles()
    {
        // if (_tiles != null)
        // {
        //     for (int i = 0; i < _tiles.Count; i++)
        //     {
        //         _tiles[i].onChange.RemoveListener(OnTileChange);
        //     }
        // }
        // _tiles.Clear();
    }

    //PRIVATE//////////////////////////////////////////////////
    private void UpdateWalls(bool recycle)
    {
        for (int i = 0; i < multipliers.Length; i++)
        {
            int xMultiplier = (int)multipliers[i].x;
            int zMultiplier = (int)multipliers[i].y;
            Tile wallTile = Level.instance.GetTile(_tile.position.x + (1 * xMultiplier), _tile.position.z + (1 * zMultiplier));
            Tile towerTile = Level.instance.GetTile(_tile.position.x + (2 * xMultiplier), _tile.position.z + (2 * zMultiplier));
            if (wallTile != null && towerTile != null)
            {
                WallTower wallTower = null;
                if (towerTile.tower && towerTile.tower is WallTower)
                {
                    wallTower = Level.instance.GetTile(_tile.position.x + (2 * xMultiplier), _tile.position.z + (2 * zMultiplier)).tower as WallTower;
                }
                if (wallTile.wall && wallTower && (recycle || !wallTile.walkable))
                {
                    wallTile.wall.Recycle();
                    wallTile.wall = null;
                }
                else if (wallTower && !wallTile.wall && wallTile.walkable)
                {
                    Quaternion rotation = Quaternion.identity;
                    if (zMultiplier != 0)
                    {
                        rotation = Quaternion.Euler(0f, 90f, 0f); //TODO: Rotation 
                    }
                    wallTile.wall = PoolManager.Produce(_data.projectilePrefab, null, new Vector3(_tile.position.x + (1 * xMultiplier), 0f, _tile.position.z + (1 * zMultiplier)), rotation).GetComponent<Wall>();
                    wallTile.wall.OnBuild(wallTile, this, wallTower);
                }
            }
        }
    }
}
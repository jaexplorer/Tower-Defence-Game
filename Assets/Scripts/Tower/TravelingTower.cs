using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingTower : Tower
{
    private static TravelingTower _currentTowerBase;
    private static Transform _towerHeadTransform;
    private static int _smallestTileOrder = 999;
    private static int _sharedCooldown;
    private static Enemy _firstTarget;

    protected Enemy _target;

    //PUBLIC///////////////////////////////////////////////////
    protected override void OnProduce()
    {
        base.OnProduce();
        if (_towerHeadTransform == null)
        {
            _towerHeadTransform = PoolManager.Produce(_data.additionalPrefab).transform;
        }
    }

    protected override void ManagedUpdate()
    {
        if (_currentTowerBase == this)
        {
            if (_sharedCooldown > 0)
            {
                _sharedCooldown--;
                if (_sharedCooldown <= 0 && _enemyCount > 0)
                {
                    Shoot();
                }
            }
        }
    }

    protected override void OnWaveStart()
    {
        base.OnWaveStart();

        int smallestOrder = 999;
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (_tiles[i].isPath && _tiles[i].pathOrder < smallestOrder)
            {
                smallestOrder = _tiles[i].pathOrder;
            }
        }
        if (smallestOrder < _smallestTileOrder)
        {
            _smallestTileOrder = smallestOrder;
            SetAsCurrentTowerBase();
        }
    }

    public override void OnEnemyEnter(Enemy enemy)
    {
        _enemyCount++;
        if (_target == null || enemy.spawnIndex < _target.spawnIndex)
        {
            _target = enemy;
            if (_currentTowerBase._target == null || _target.spawnIndex < _currentTowerBase._target.spawnIndex)
            {
                SetAsCurrentTowerBase();
            }
        }
        if (_currentTowerBase == this && _sharedCooldown <= 0 && _enemyCount > 0)
        {
            Shoot();
        }
    }

    public override void OnEnemyExit(Enemy enemy)
    {
        _enemyCount--;
        if (enemy == _target)
        {
            if (_enemyCount > 0)
            {
                _target = GetFirstTarget();
                if (_currentTowerBase._target == null || _target.spawnIndex < _currentTowerBase._target.spawnIndex)
                {
                    SetAsCurrentTowerBase();
                }
            }
            else
            {
                _target = null;
            }
        }
    }

    //PROTECTED////////////////////////////////////////////////
    protected override void Shoot()
    {
        ProduceProjectile().Launch(_target);
        _sharedCooldown = _data.cooldown;
    }

    //PRIVATE//////////////////////////////////////////////////
    private void SetAsCurrentTowerBase()
    {
        _currentTowerBase = this;
        // Debug.Log(_currentTowerBase);
        _towerHeadTransform.position = tile.position.ToVector3();
    }
}

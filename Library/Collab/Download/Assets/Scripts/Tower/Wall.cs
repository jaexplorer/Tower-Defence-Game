using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ManagedObject
{
    [SerializeField] private ParticleSystem _particleSystem;
    private WallTower _tower1;
    private WallTower _tower2;
    private Tile _tile;
    private Enemy _targetEnemy;

    public void Reset(int updateOrder)
    {
        SetUpdateOrder(updateOrder);
        _tile.onEnemyEnter.RemoveListener(OnEnemyEnter);
        _tile.onEnemyEnter.AddListener(OnEnemyEnter);
        _targetEnemy = null;
    }

    public void OnBuild(Tile tile, WallTower tower1, WallTower tower2)
    {
        _tile = tile;
        _tower1 = tower1;
        _tower2 = tower2;
    }

    protected override void ManagedUpdate()
    {
        if (_targetEnemy && _targetEnemy.currentTile == _tile)
        {
            if (_targetEnemy.movementProgress > _tile.curve.segmentCount / 2 - 10)
            {

            }
            if (_targetEnemy.movementProgress > _tile.curve.segmentCount / 2)
            {
                _particleSystem.Emit(30);
                _targetEnemy.Damage(_tower1.data.projectileDamage + _tower2.data.projectileDamage);
                _targetEnemy = _targetEnemy.next;
            }
        }
    }

    public void OnEnemyEnter(Enemy enemy)
    {
        if (_targetEnemy == null)
        {
            _targetEnemy = enemy;
        }
    }

    protected override void OnProduce()
    {
        // EventManager.onLevelChange.AddListener(OnMapChange);
    }

    protected override void OnRecycle()
    {
        // Debug.Log("recwall");
        _tile.onEnemyEnter.RemoveListener(OnEnemyEnter);
        // EventManager.onLevelChange.RemoveListener(OnMapChange);
    }

    //PRIVATE//////////////////////////////////////////////////
    // private void OnMapChange()
    // {
    //     transform.LookAt(_tile.next.transform);
    //     if (_tile.elevated)
    //     {
    //         Recycle();
    //     }
    // }

    public void UpdateRotation()
    {
        transform.LookAt(_tile.next.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultitargetTower : Tower
{
    private List<Enemy> _enemies = new List<Enemy>(20);

    //PUBLIC///////////////////////////////////////////////////
    public override void OnEnemyEnter(Enemy enemy)
    {
        _enemyCount++;
        _enemies.Add(enemy);
        if (_enemyCount == 1 && _cooldown <= 0)
        { 
            Shoot();
        }
    }

    public override void OnEnemyExit(Enemy enemy)
    {
        _enemyCount--;
        _enemies.Remove(enemy);
    }

    //PRIVATE//////////////////////////////////////////////////
    protected override void Shoot()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            ProduceProjectile().Launch(_enemies[i]);
        }
        Cooldown();
    }
}

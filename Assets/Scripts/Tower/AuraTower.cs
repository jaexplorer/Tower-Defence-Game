using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not implimented

public class AuraTower : Tower
{
    [SerializeField]
    private GameObject _auraObject;

    private List<Enemy> _enemies = new List<Enemy>(20);

    protected override void ManagedUpdate()
    {
        if (_enemies.Count > 0)
        {
            if (!_auraObject.activeSelf)
            {
                _auraObject.SetActive(true);
            }
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Damage(_data.projectileDamage);
            }
        }
        else
        {
            if (_auraObject.activeSelf)
            {
                _auraObject.SetActive(false);
            }
        }
    }

    public override void OnEnemyEnter(Enemy enemy)
    {
        _enemies.Add(enemy);
        // if (_enemyCount == 1 && _cooldown <= 0)
        // {
        //     // Debug.Log("ec" + _cooldown);
        //     Shoot();
        // }
    }

    public override void OnEnemyExit(Enemy enemy)
    {
        _enemies.Remove(enemy);
        // if (_enemyCount > 0)
        // {
        //     _enemyCount--;
        // }
    }
}

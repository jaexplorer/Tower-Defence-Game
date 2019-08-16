using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _healthBarPrefab;

    public static HealthBarManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        instance = this;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        HealthBar healthBar = PoolManager.Produce(_healthBarPrefab, transform).GetComponent<HealthBar>();
        healthBar.Reset(enemy);
    }
}

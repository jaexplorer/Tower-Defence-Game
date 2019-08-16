using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shared static data of their reset values of enemies with the same type
/// </summary>

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    // [SerializeField] private List<EnemyEffect> _onSpawnEffects;
    [SerializeField] private Vector3 _healthBarPoint;
    [SerializeField] private Vector3 _center;
    [SerializeField] private int _portalDamage;
    [SerializeField] private int _bounty;
    [SerializeField] private int _health;
    [SerializeField] private int _armor;
    [SerializeField] private int _speed;
    [SerializeField] private int _spawnInterval;
    [SerializeField] private int _acceleration;

    //PROPERTIES///////////////////////////////////////////////
    public GameObject prefab { get { return _prefab; } }
    // public List<EnemyEffect> onSpawnEffects { get { return _onSpawnEffects; } }
    public Vector3 healthBarPoint { get { return _healthBarPoint; } }
    public int portalDamage { get { return _portalDamage; } }
    public int bounty { get { return _bounty; } }
    public int speed { get { return _speed; } }
    public int health { get { return _health; } }
    public int armor { get { return _armor; } }
    public int spawnInterval { get { return _spawnInterval; } }
    public Vector3 center { get { return _center; } }
}

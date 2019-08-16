using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Vector3 _healthBarPoint;
    [SerializeField]
    private Vector3 _center;
    [SerializeField]
    private int _portalDamage;
    [SerializeField]
    private int _reward;
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private int _speed;
    [SerializeField]
    private int _spawnInterval;
    [SerializeField]
    private int _acceleration;

    //PROPERTIES///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public GameObject prefab
    {
        get { return _prefab; }
    }

    public Vector3 healthBarPoint
    {
        get { return _healthBarPoint; }
    }

    public int portalDamage
    {
        get { return _portalDamage; }
    }

    public int reward
    {
        get { return _reward; }
    }


    public int speed
    {
        get { return _speed; }
    }

    public int health
    {
        get { return _health; }
    }

    public int spawnInterval
    {
        get { return _spawnInterval; }
    }

    public Vector3 center
    {
        get { return _center; }
    }
}

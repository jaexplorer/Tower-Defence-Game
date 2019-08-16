using UnityEngine;

[System.Serializable]
public class SpawnWave
{
    [SerializeField]
    private EnemyData _enemyData;
    [SerializeField]
    private int _count;
    [SerializeField]
    private int _spawnInterval;

    public EnemyData enemyData
    {
        get { return _enemyData; }
    }

    public int count
    {
        get { return _count; }
    }

    public int spawnInterval
    {
        get { return _spawnInterval; }
    }
}
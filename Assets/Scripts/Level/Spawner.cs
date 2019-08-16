using UnityEngine;
using System.Collections.Generic;

public class Spawner : ManagedObject
{
    [SerializeField] private Transform _transform;
    [SerializeField] private AudioSource _audioSource;

    private Tile _tile;
    private SpawnWave[] _spawnWaves;
    private SpawnWave _currentWave;
    private int _cooldown;
    private int _currentWaveIndex;
    private int _enemyIndex;
    private Enemy _lastSpawnedEnemy;
    private Enemy _firstEnemy;
    private int _enemiesLeft;

    //PROPERTIES///////////////////////////////////////////////
    public Tile tile { get { return _tile; } }
    public SpawnWave[] spawnWaves { get { return _spawnWaves; } }
    public SpawnWave currentWave { get { return _currentWave; } }
    public Enemy firstEnemy { get { return _firstEnemy; } }
    public new int updatePriority { get { return _tile.priority; } }
    public int waveIndex { get { return _currentWaveIndex; } set { _currentWaveIndex = value; } }

    //EVENTS///////////////////////////////////////////////////
    protected override void OnProduce()
    {
        _spawnWaves = Level.instance.data.spawnWaves;
        _tile = Level.instance.GetTile(transform.position);//TODO
        enabled = false;
    }

    protected override void ManagedUpdate()
    {
        if (enabled)
        {
            _cooldown--;
            if (_cooldown <= 0 && _enemyIndex < _currentWave.count)
            {
                _tile = Level.instance.GetTile(transform.position);//TODO
                Enemy enemy = PoolManager.Produce(_currentWave.enemyData.prefab, null, _tile.transform.position).GetComponent<Enemy>();
                if (_enemyIndex != 0)
                {
                    enemy.previous = _lastSpawnedEnemy;
                    _lastSpawnedEnemy.next = enemy;
                }
                else
                {
                    _firstEnemy = enemy;
                }
                _lastSpawnedEnemy = enemy;
                _cooldown = _currentWave.enemyData.spawnInterval;
                enemy.OnSpawn(_tile, _enemyIndex);
                AudioSource.PlayClipAtPoint(_audioSource.clip, _transform.position, 1f);
                _enemyIndex++;
            }
        }
    }

    protected override void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.Wave)
        {
            enabled = true;
            _currentWave = _spawnWaves[_currentWaveIndex];
            _enemyIndex = 0;
            _cooldown = 20;
            _enemiesLeft = _currentWave.count;
        }
        else
        {
            enabled = false;
        }
    }

    protected override void OnWaveLoad()
    {
        _currentWaveIndex = ProfileManager.instance.saveData.currentWaveIndex;
    }

    protected override void OnWaveSave()
    {
        ProfileManager.instance.saveData.currentWaveIndex = _currentWaveIndex;
    }

    protected override void OnEnemyDeath(Enemy enemy)
    {
        _enemiesLeft--;
        if (_enemiesLeft == 0)
        {
            _currentWaveIndex++;
            EventManager.onWaveComplete.Invoke();
        }
    }
}
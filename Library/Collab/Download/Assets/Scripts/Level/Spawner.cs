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
    // private List<Enemy> _enemies = new List<Enemy>(64);
    private int _enemiesLeft;

    //PROPERTIES///////////////////////////////////////////////
    public Tile tile { get { return _tile; } }
    public SpawnWave[] spawnWaves { get { return _spawnWaves; } }
    public SpawnWave currentWave { get { return _currentWave; } }
    public int waveIndex { get { return _currentWaveIndex; } set { _currentWaveIndex = value; } }
    public int enemiesLeft { get { return _enemiesLeft; } set { _enemiesLeft = value; } }
    public Enemy firstEnemy { get { return _firstEnemy; } }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        Events.onEnemyDeath.AddListener(OnEnemyDeath);
    }

    //PUBLIC///////////////////////////////////////////////////
    protected override void OnProduce()
    {
        SetUpdateOrder(0);
        Events.onLevelClear.RemoveListener(Recycle);
        _spawnWaves = Level.instance.data.spawnWaves;
        _tile = Level.instance.GetTile(transform.position);// unreliable
    }

    protected override void ManagedUpdate()
    {
        if (GameManager.inWaveMode)
        {
            _cooldown--;
            if (_cooldown <= 0 && _enemyIndex < _currentWave.count)
            {
                _tile = Level.instance.GetTile(transform.position);//
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
                // _audioSource.Play();
                _enemyIndex++;
            }
        }
    }

    public void OnWaveStart()
    {
        _currentWave = _spawnWaves[_currentWaveIndex];
        _enemyIndex = 0;
        _cooldown = 20;
        _enemiesLeft = _currentWave.count;
    }

    public void OnWaveLoad()
    {
        _currentWaveIndex = ProfileManager.instance.saveData.currentWaveIndex;
        Hud.instance.SetWaves(_currentWaveIndex + 1, _spawnWaves.Length);
    }

    public void OnWaveSave()
    {
        ProfileManager.instance.saveData.currentWaveIndex = _currentWaveIndex;
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        _enemiesLeft--;
        if (_enemiesLeft == 0)
        {
            _currentWaveIndex++;
            Hud.instance.SetWaves(_currentWaveIndex + 1, _spawnWaves.Length);
            GameManager.instance.CompleteWave();
        }
    }
}
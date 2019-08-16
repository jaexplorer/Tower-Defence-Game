using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Enemy : ManagedObject
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private ParticleSystem _teleportEffectBurst;
    [SerializeField] private ParticleSystem _teleportEffectLinger;
    [SerializeField] private ParticleSystem _frostFogEffect;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private EnemyData _data;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private AudioSource _damageAudioSource;

    private Enemy _next;
    private Enemy _previous;
    private UnityEvent _onDeath = new UnityEvent();
    private UnityEvent _onDamage = new UnityEvent();
    private Tile _currentTile;
    private Direction _movementDirection;
    private MapVector _position;
    private Material _material;
    // private EffectManager _effects;
    private int _speed;
    private int _armor;
    private int _health;
    private int _spawnIndex;
    private int _movementProgress;
    private int _lifetime;
    private bool _teleporting;

    //PROPERTIES/////////////////////////////////////////////////////////////////////////
    public new GameObject gameObject { get { return _gameObject; } }
    public new Transform transform { get { return _transform; } }
    public new Rigidbody rigidbody { get { return _rigidbody; } }
    public ParticleSystem frostFogEffect { get { return _frostFogEffect; } }
    public EnemyData data { get { return _data; } }
    public Tile currentTile { get { return _currentTile; } }
    public UnityEvent onDeath { get { return _onDeath; } }
    public UnityEvent onDamage { get { return _onDamage; } }
    public Vector3 centerPosition { get { return _transform.position + _data.center; } }
    public int spawnIndex { get { return _spawnIndex; } }
    // public EffectManager effects { get { return _effects; } set { _effects = value; } }
    public int health { get { return _health; } }
    public int armor { get { return _armor; } }
    public int speed { get { return _speed; } }
    public MapVector position { get { return _position; } }
    public int movementProgress { get { return _movementProgress; } }
    public Enemy next { get { return _next; } set { _next = value; } }
    public Enemy previous { get { return _previous; } set { _previous = value; } }

    //PUBLIC/////////////////////////////////////////////////////////////////////////////
    public void OnSpawn(Tile tile, int spawnIndex)
    {
        TeleprtBlink();
        _currentTile = tile;
        _movementProgress = 500;
        _transform.LookAt(tile.next.transform);
        _speed = _data.speed;
        _health = _data.health;
        _position = new MapVector(tile.position);
        _currentTile.OnEnemyEnter(this);
        _spawnIndex = spawnIndex;
        _lifetime = 0;
        _armor = _data.armor;
        HealthBarManager.instance.RegisterEnemy(this);
    }

    public void Damage(int amount, int armorPiercing = 0)
    {
        _onDamage.Invoke();
        DebugManager.ContributeToChecksum(amount);
        Blink();
        if (_health > 0)
        {
            _health -= (amount - _armor);
            if (_health <= 0)
            {
                Kill();
            }
        }
        // _damageAudioSource.PlayDelayed(0.02f);
        AudioSource.PlayClipAtPoint(_damageAudioSource.clip, _transform.position);
    }

    public void Heal(int amount)
    {
        _health += amount;
    }

    public void AlterSpeed(int amount)
    {
        if (_speed + amount >= 0) // Ensuring speed doesn't go below 0
        {
            _speed += amount;
        }
    }

    //EVENTS///////////////////////////////////////////////////
    protected override void ManagedUpdate()
    {
        // Moving.
        _movementProgress += _speed / UpdateManager.framesPerSecond;

        // Reaching middle.
        if (_currentTile.isPortal)
        {
            if (_movementProgress >= _currentTile.curve.segmentCount / 2)
            {
                ReachPortal();
            }
        }
        if (_currentTile.isTeleport && _currentTile.teleport.isEntrence)
        {
            if (_movementProgress >= _currentTile.curve.segmentCount / 2)
            {
                Teleport();
            }
            if (_movementProgress >= _currentTile.curve.segmentCount / 2 - (_speed / 4) && _movementProgress < _currentTile.curve.segmentCount / 2 - (_speed / 4) + _speed / UpdateManager.framesPerSecond)
            {
                // _teleportParticles.Play();
                DOTween.To((x) => _material.SetFloat("_TeleportEmission", x), 0f, 2f, 0.7f);
                // Teleport();
            }
        }

        // Entering new tiles.
        if (_movementProgress >= _currentTile.curve.segmentCount)
        {
            _movementProgress = _movementProgress - _currentTile.curve.segmentCount;
            _currentTile.OnEnemyExit(this);
            _currentTile = _currentTile.next;
            _currentTile.OnEnemyEnter(this);
        }

        // Moving transform.
        MapVector localMapVector = _currentTile.curve.GetPrecalculatedMapVector(_movementProgress);
        _position = new MapVector(localMapVector.x + _currentTile.position.x * 1000, localMapVector.z + _currentTile.position.z * 1000);
        _rigidbody.MovePosition(_currentTile.curve.GetPrecalculatedPoint(_movementProgress) + _currentTile.transform.position);
        _rigidbody.MoveRotation(Quaternion.LookRotation(_currentTile.curve.GetPrecalculatedDirection(_movementProgress)));
        _lifetime++;
    }

    protected void Awake()
    {
        _material = _meshRenderer.material;
    }

    protected override void OnRecycle()
    {
        _currentTile.OnEnemyExit(this);
        _onDeath.Invoke();
        _onDeath.RemoveAllListeners();
    }

    //PRIVATE//////////////////////////////////////////////////
    private void Kill()
    {
        EventManager.onEnemyDeath.Invoke(this);
        Recycle();
    }

    private void ReachPortal()
    {
        Level.instance.portal.Damage(_data.portalDamage);
        Kill();
    }

    private void Blink()
    {
        DOTween.To((x) => _material.SetFloat("_BlinkEmission", x), 1f, 0f, 0.2f);
    }

    private void TeleprtBlink()
    {
        DOTween.To((x) => _material.SetFloat("_TeleportEmission", x), 2f, 0f, 0.7f);
    }

    private void Teleport()
    {
        // _teleportEffectBurst.randomSeed = (uint)Random.Range(0, 10000000);

        _teleportEffectBurst.Emit(25);
        _teleportEffectBurst.transform.Rotate(0f, Random.Range(0, 360), 0f);
        _currentTile = _currentTile.teleport.pair.tile;
        _transform.position = _currentTile.teleport.pair.tile.curve.GetPrecalculatedPoint(_movementProgress) + _currentTile.transform.position;
        _teleportEffectBurst.Emit(25);
        _currentTile.teleport.OnEnemyEnter();
        TeleprtBlink();
    }
}
using UnityEngine;

public class RayProjectile : Projectile, IUpdatable
{
    [SerializeField] private ParticleSystem _targetParticleSystem;
    [SerializeField] private Transform _targetParticleSystemTransform;
    [SerializeField] private GameObject _targetParticleSystemGameObject;
    [SerializeField] private ParticleSystem _beamParticleSystem;
    [SerializeField] private MeshRenderer _meshRenderer;
    private float _lifetime;
    private Vector3 _launchPoint;
    private float _scaleMultiplier;
    private int _damage;
    private Tower _tower;
    private Enemy _target;
    private float _sizeMin = 0.7f;
    private float _sizeMax = 1f;
    private float _currentSize = 1f;
    private int _cooldown;
    // private bool _active;

    //EVENTS///////////////////////////////////////////////////
    private void Update()
    {
        if (!pendingRecycle)
        {
            Vector3 targetPosition = _target.transform.position + _target.data.center;
            _transform.localScale = new Vector3(_currentSize, _currentSize, Vector3.Distance(_launchPoint, targetPosition) - 0.1f);
            _transform.rotation = Quaternion.LookRotation(targetPosition - _transform.position);
            if (_currentSize < _sizeMax)
            {
                _currentSize += 0.1f * Time.deltaTime;
            }
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    protected override void ManagedUpdate()
    {
        if (!pendingRecycle)
        {
            _cooldown--;
            if (_cooldown <= 0)
            {
                _damage += _towerData.projectileDamage;
                _target.Damage(_damage);
                _cooldown = 1 * UpdateManager.framesPerSecond;
            }
            Vector3 enemyTileLocalPosition = _target.currentTile.position.ToVector3() - _tower.tile.position.ToVector3();
            if (enemyTileLocalPosition.x > 1.5f || enemyTileLocalPosition.x < -1.5f || enemyTileLocalPosition.z > 1.5f || enemyTileLocalPosition.z < -1.5f)
            {
                Stop();
            }
        }
    }

    public override void Launch(Enemy target, Tower tower)
    {
        _cooldown = 0;
        _currentSize = _sizeMin;
        _tower = tower;
        _target = target;
        _damage = 0;
        _launchPoint = _transform.position;
        _meshRenderer.enabled = true;
        _target.onDeath.AddListener(Stop);
        Update();
        _targetParticleSystem.Emit(15);
        _targetParticleSystem.Play();
    }

    public void Stop()
    {
        _tower.Cooldown();
        Recycle(40);
        _target.onDeath.RemoveListener(Stop);
        _meshRenderer.enabled = false;
        _beamParticleSystem.Emit(40);
        _targetParticleSystem.Stop();
        // _target = null;
        // PoolManager.Recycle();
    }

    protected override void OnRecycle()
    {
    }
}
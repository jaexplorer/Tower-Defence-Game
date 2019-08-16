using UnityEngine;

///Not implimented

public class ChainProjectile : Projectile, IUpdatable
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _targetParticleSystemGameObject;
    [SerializeField] private Transform _targetParticleSystemTransform;
    [SerializeField] private ParticleSystem _targetParticleSystem;
    [SerializeField] private ParticleSystem _beamParticleSystem;

    // private Vector3 _launchPoint;
    // private Enemy _target;

    //EVENTS///////////////////////////////////////////////////
    private void Update()
    {
    }

    //PUBLIC///////////////////////////////////////////////////
    protected override void ManagedUpdate()
    {
    }

    public override void Launch(Enemy target)
    {
        // _target = target;
        // _launchPoint = _transform.position;
        _meshRenderer.enabled = true;
        // _target.onDeath.AddListener(Stop);
        Update();
        _targetParticleSystem.Emit(15);
        _targetParticleSystem.Play();
    }


    public void Stop()
    {
        Recycle(40);
        // _target.onDeath.RemoveListener(Stop);
        _meshRenderer.enabled = false;
        _beamParticleSystem.Emit(40);
        _targetParticleSystem.Stop();
    }

    protected override void OnRecycle()
    {
    }

    //PRIVATE//////////////////////////////////////////////////
    private void LaunchSecondary(Enemy target)
    {

    }
}
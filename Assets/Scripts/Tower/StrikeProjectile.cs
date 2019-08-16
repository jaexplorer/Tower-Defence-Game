using UnityEngine;

public class StrikeProjectile : Projectile
{
    private Enemy _target;
    // private Vector3 _direction;

    //EVENTS///////////////////////////////////////////////////
    // protected override void ManagedUpdate()
    private void Update()
    {
        // _transform.position += _direction;
        if (_target)
        {
            _transform.position = _target.transform.position;
        }
    }

    private void OnTargetDestroyed()
    {
        _target = null;
    }

    //PUBLIC///////////////////////////////////////////////////
    public override void Launch(Enemy target)
    {
        _target = target;
        // Debug.Log(_target);
        // Debug.Log(_target.transform);
        // Debug.Log(_towerData);
        //_direction = (_target.transform.position - _transform.position).normalized * _towerData.projectileVelocity / 1000;
        // _direction = _target.transform.forward * _towerData.projectileVelocity / 1000;
        _transform.position = _target.transform.position;// -_direction * 10f;
                                                         // _target = target;
        _target.onDeath.AddListener(OnTargetDestroyed);
        _target.Damage(_towerData.projectileDamage);
        Recycle(_towerData.projectileLifetime);
    }
}
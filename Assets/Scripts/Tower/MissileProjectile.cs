using UnityEngine;
// using DG.Tweening;

public class MissileProjectile : Projectile, IUpdatable
{
    [SerializeField]private TrailRenderer _trailRenderer;
    [SerializeField]private Rigidbody _rigidbody;

    private Enemy _target;
    private BezierCurve _curve = new BezierCurve();
    private float _progress;
    private float _progressRate;
    private float _lifetime; 

    //PUBLIC///////////////////////////////////////////////////
    protected override void ManagedUpdate()
    {
        _progress += _towerData.projectileVelocity;
        _curve.SetPoint(3, _target.centerPosition);
        _rigidbody.MovePosition(_curve.GetPoint(_progress * 0.001f));
        Vector3 lookVector = _curve.GetDirection(_progress * 0.001f);
        if (lookVector != Vector3.zero)
        {
            _rigidbody.MoveRotation(Quaternion.LookRotation(_curve.GetDirection(_progress * 0.001f)));
        }
        if (_progress >= 1000)
        {
            _target.Damage(_towerData.projectileDamage);
            Recycle();
        }
    }

    public override void Launch(Enemy target)
    {
        _trailRenderer.Clear();
        _progress = 0;
        _target = target;
        Vector3 targetCenter = target.centerPosition;
        Vector3 position = _transform.position;
        Vector3 v0 = position;
        Vector3 v1 = new Vector3(targetCenter.x, position.y, targetCenter.z);
        Vector3 v2 = targetCenter;
        v2 = targetCenter;
        _curve.Set(v0, v1, v2, targetCenter);
    }
}
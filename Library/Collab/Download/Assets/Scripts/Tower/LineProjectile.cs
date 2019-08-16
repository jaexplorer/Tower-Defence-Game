using UnityEngine;
using System.Collections.Generic;

public class LineProjectile : Projectile, IUpdatable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AudioClip _audioClip;

    private MapVector _position;
    private MapVector _targetVector;
    private List<Enemy> _enemiesHit = new List<Enemy>();
    private LineTowerTrigger _trigger;
    private int _travelDistance;

    //EVENTS///////////////////////////////////////////////////
    protected override void ManagedUpdate()
    {
        // Moving.
        _travelDistance += _towerData.projectileVelocity;
        _position.x += _towerData.projectileVelocity * _trigger.xMultiplier;
        _position.z += _towerData.projectileVelocity * _trigger.zMultiplier;
        if (_travelDistance > _trigger.range)
        {
            Recycle();
            return;
        }
        _rigidbody.MovePosition(_position.ToVector3() + new Vector3(0f, 0.2f, 0f));

        // Dealing damage.
        if (_trigger.enemies.Count > 0)
        {
            for (int i = _trigger.enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _trigger.enemies[i];
                if (MapVector.SquareDistance(enemy.position, _position) < _towerData.projectileRange && !_enemiesHit.Contains(enemy))
                {
                    _enemiesHit.Add(enemy);
                    enemy.Damage(_towerData.projectileDamage);
                    AudioSource.PlayClipAtPoint(_audioClip, _transform.position, 1);
                }
            }
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Launch(LineTowerTrigger trigger)
    {
        _trigger = trigger;
        // Debug.Log(_trigger.range);
        _position = trigger.launchVector;
        _travelDistance = 0;
        _enemiesHit.Clear();
        // Recycle(10);
    }
}
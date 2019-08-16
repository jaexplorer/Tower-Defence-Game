// using UnityEngine;

// public class ChargingTower : Tower
// {
//     private float _timeSinceLastShot;
//     private float _timeOfLastShot;

//     public override float GetTimeSinceLastSHot()
//     {
//         return _timeSinceLastShot;
//     }

//     // protected virtual void OnTriggerEnter(Collider other)
//     // {
//     //     Enemy enemy = other.GetComponent<Enemy>();

//     //     //DEBUG
//     //     if (!enemy) Debug.LogError("Not Enemy!" + other);

//     //     GameObject projectileObject = PoolManager.Produce(_data.projectilePrefab, null, _transform.position + _data.launcherPoint);
//     //     projectileObject.GetComponent<Projectile>().Launch(this);
//     //     _onCooldown = true;
//     //     // if (!_data.manualCooldown)
//     //     // {
//     //     //     Cooldown();
//     //     // }
//     //     // if (!_data.hasIntersectingTriggers || !_enemies.Contains(enemy))
//     //     // {
//     //     //     enemy.onDeath.AddListener(OnEnemyDeath);
//     //     //     _enemies.Add(enemy);
//     //     //     if (_enemies.Count == 1)
//     //     //     {
//     //     //         Shoot();
//     //     //     }
//     //     // }
//     // }

//     protected override void Shoot()
//     {
//         _timeSinceLastShot = Time.time - _timeOfLastShot;
//         _timeOfLastShot = Time.time;
//         base.Shoot();
//     }
// }
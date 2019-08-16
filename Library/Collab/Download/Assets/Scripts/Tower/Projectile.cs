using UnityEngine;

public class Projectile : ManagedObject
{
    [SerializeField]
    protected GameObject _gameObject;
    [SerializeField]
    protected Transform _transform;
    [SerializeField]
    protected TowerData _towerData;

    public virtual void Launch(Enemy enemy)
    {

    }

    public virtual void Launch(Enemy enemy, Tower tower)
    {

    }
}
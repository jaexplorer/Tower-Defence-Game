using UnityEngine;
using System.Collections;

/// Teleports the enemies, this is object, not implimented fully
public class Teleport : ManagedObject
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem _particleSystemOut;
    [SerializeField] private ParticleSystem _particleSystemIn;
    [SerializeField] private Transform _transform;
    private Teleport _pair;
    private Tile _tile;

    //PROPERTIES///////////////////////////////////////////////
    public new Transform transform { get { return _transform; } }
    public Tile tile { get { return _tile; } }
    public Teleport pair { get { return _pair; } set { _pair = value; } }
    public bool isEntrence { get { return _tile.pathOrder < _pair.tile.pathOrder; } }

    //PROTECTED////////////////////////////////////////////////
    protected override void OnProduce()
    {
        _tile = Level.instance.GetTile(_transform.position);
    }

    public void OnEnemyEnter()
    {
        // if (_tile.pathOrder < _pair.tile.pathOrder)
        // {
        // enemy.transform.position = _pair.transform.position;
        _pair.OnEnemyExit();
        _particleSystem.Play();
        // _particleSystemOut.Play();   
        // }
    }

    public void OnEnemyExit()
    {
        _particleSystem.Play();
    }
}

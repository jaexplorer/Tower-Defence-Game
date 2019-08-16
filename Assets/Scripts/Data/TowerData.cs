using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shared static data of their reset values of towers with the same type
/// </summary>

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/TowerData", order = 1)]
public class TowerData : ScriptableObject, ITileDataButton
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _additionalPrefab;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Sprite _buttonSprite;
    [SerializeField] private Vector3[] _markers;
    [SerializeField] private Vector3 _launcherPoint;
    // [SerializeField] private List<TowerEffect> _onSpawnEffects;
    [SerializeField] private int _cooldown;
    [SerializeField] private int _range;
    [SerializeField] private int _projectileDamage;
    [SerializeField] private int _projectileVelocity;
    [SerializeField] private int _projectileRange;
    [SerializeField] private int _projectileLifetime;
    [SerializeField] private int _debuff;
    [SerializeField] private int _debuffMax;

    //PROPERTIES///////////////////////////////////////////////
    public Color color { get { return _color; } }
    public GameObject towerPrefab { get { return _towerPrefab; } }
    public GameObject projectilePrefab { get { return _projectilePrefab; } }
    public GameObject additionalPrefab { get { return _additionalPrefab; } }
    public Mesh mesh { get { return _mesh; } }
    public Sprite buttonSprite { get { return _buttonSprite; } }
    public Vector3[] markers { get { return _markers; } }
    public Vector3 launcherPoint { get { return _launcherPoint; } }
    // public List<TowerEffect> onSpawnEffects { get { return _onSpawnEffects; } }
    public int cooldown { get { return _cooldown; } }
    public int projectileDamage { get { return _projectileDamage; } }
    public int projectileVelocity { get { return _projectileVelocity; } }
    public int projectileRange { get { return _projectileRange; } }
    public int projectileLifetime { get { return _projectileLifetime; } }
    public int range { get { return _range; } }
    public int debuff { get { return _debuff; } }
    public int debuffMax { get { return _debuffMax; } }
}
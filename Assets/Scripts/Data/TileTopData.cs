using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Information about the top of the tiles
/// </summary>

[CreateAssetMenu(fileName = "TileData", menuName = "Data/TileData", order = 1)]
public class TileTopData : ScriptableObject, ITileDataButton
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private bool _elevatable;
    [SerializeField] private bool _isTowerSocket;
    [SerializeField] private bool _walkable;
    [SerializeField] public bool test;

    //PROPERTIES///////////////////////////////////////////////
    public Color color { get { return _color; } }
    public GameObject prefab { get { return _prefab; } }
    public Mesh mesh { get { return _mesh; } }
    public bool walkable { get { return _walkable; } }
    public bool elevatable { get { return _elevatable; } }
    public bool hasSocket { get { return _isTowerSocket; } }

    //ENUMS//////////////////////////////////////////////////////
    public enum DataTypeFlag
    {
        Any,
        Green,
        Autumn,
        Dark
    }
}

public interface ITileDataButton
{
    Color color { get; }
    string name { get; }
}

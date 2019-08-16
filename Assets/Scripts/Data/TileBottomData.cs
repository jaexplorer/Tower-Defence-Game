using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Information about the bottom part of the tile
/// </summary>

[CreateAssetMenu(fileName = "TileBottomData", menuName = "Data/TileBottomData", order = 1)]
public class TileBottomData : ScriptableObject
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private bool _isDefault;

    //PROPERTIES///////////////////////////////////////////////
    public Color color { get { return _color; } }
    public GameObject prefab { get { return _prefab; } }
    public Mesh mesh { get { return _mesh; } }
    public bool isDefault { get { return _isDefault; } }
}

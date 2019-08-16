using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TileObjectData", menuName = "Data/TileObjectData", order = 1)]
public class TileObjectData : ScriptableObject, ITileDataButton
{
    [SerializeField] private Color _color;
    [SerializeField] private string _variationGroup;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Type _type;
    [SerializeField] private bool _walkable;

    //PROPERTIES///////////////////////////////////////////////
    public Color color { get { return _color; } }
    public GameObject prefab { get { return _prefab; } }
    public Mesh mesh { get { return _mesh; } }
    public Type type { get { return _type; } }
    public bool walkable { get { return _walkable; } }
    public string variationGroup { get { return variationGroup; } }

    //TYPES////////////////////////////////////////////////////
    public enum Type
    {
        Any,
        Tile,
        Foundation,
        Portal,
        Spawner,
        Teleport,
        Decor,
    }
}
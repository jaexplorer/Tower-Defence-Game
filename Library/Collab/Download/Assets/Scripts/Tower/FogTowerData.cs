using UnityEngine;

[CreateAssetMenu(fileName = "FogTowerData", menuName = "Data/FogTowerData", order = 1)]
public class FogTowerData : TowerData
{
    [SerializeField] private GameObject _fogPrefab;
    [SerializeField] private Mesh _fogMeshMid;
    [SerializeField] private Mesh _fogMeshFront;
    [SerializeField] private Mesh _fogMeshBack;
    [SerializeField] private Mesh _fogMeshLeft;
    [SerializeField] private Mesh _fogMeshRight;
    // [SerializeField] private Material _fogMaterial;

    public GameObject fogPrefab
    {
        get { return _fogPrefab; }
    }
    public Mesh fogMeshMid
    {
        get { return _fogMeshMid; }
    }
    public Mesh fogMeshFront
    {
        get { return _fogMeshFront; }
    }
    public Mesh fogMeshBack
    {
        get { return _fogMeshBack; }
    }
    public Mesh fogMeshLeft
    {
        get { return _fogMeshLeft; }
    }
    public Mesh fogMeshRight
    {
        get { return _fogMeshRight; }
    }
}
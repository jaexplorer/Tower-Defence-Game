using UnityEngine;
using System.Collections;

[System.Serializable]
public class PortalData
{
    [SerializeField]
    private int _pointsMax = 1000;
    [SerializeField]
    private TilePosition _mapPosition;
}

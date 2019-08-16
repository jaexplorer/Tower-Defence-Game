using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class TowerTriggerData
{
    private List<Vector3> _markers;
    private Vector3 _rotationVector;

    public List<Vector3> markers
    {
        get { return _markers; }
    }

    public Vector3 rotationVector
    {
        get { return _rotationVector; }
    }
}
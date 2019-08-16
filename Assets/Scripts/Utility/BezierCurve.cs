using UnityEngine;

/// <summary>
/// Not created by Vlad, library for curves such path curves
/// </summary>

[System.SerializableAttribute]
public class BezierCurve
{
    [SerializeField]
    private Vector3[] _points = new Vector3[4];

    public Vector3 GetPoint(float t)
    {
        return Bezier.GetPoint(_points[0], _points[1], _points[2], _points[3], t);
    }

    public Vector3 GetVelocity(float t)
    {
        return Bezier.GetFirstDerivative(_points[0], _points[1], _points[2], _points[3], t);
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void SetPoint(int index, Vector3 point)
    {
        _points[index] = point;
    }

    public void Set(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        _points[0] = v0;
        _points[1] = v1;
        _points[2] = v2;
        _points[3] = v3;
    }
}
using UnityEngine;
using System.Collections.Generic;

/// Keeps curve data, provides it
[System.SerializableAttribute]
public class TileCurve
{
    [SerializeField]
    private Vector3[] _points = new Vector3[4];
    [SerializeField]
    private Vector3[] _precalculatedPositions;
    [SerializeField]
    private Vector3[] _precalculatedDirections;
    [SerializeField]
    private int _segmentCount;
    [SerializeField]
    private MapVector[] _precalculatedMapVectors;

    float segmentLenght = 0.0001f;
    int sampleResolution = 1000000;

    public int segmentCount
    {
        get { return _segmentCount; }
    }

    public void PrecalculatePoints()
    {
        // Collecting raw data.
        Vector3[] rawPoints = new Vector3[sampleResolution];
        Vector3[] rawDirections = new Vector3[sampleResolution];
        MapVector[] rawMapVectors = new MapVector[sampleResolution];
        float[] distances = new float[sampleResolution];
        rawPoints[0] = GetPoint(0);
        rawDirections[0] = GetDirection(0);
        rawMapVectors[0] = new MapVector(rawPoints[0]);
        for (int i = 1; i < rawPoints.Length; i++)
        {
            rawPoints[i] = GetPoint((float)i / sampleResolution);
            rawDirections[i] = GetDirection((float)i / sampleResolution);
            rawMapVectors[i] = new MapVector(rawPoints[i]);
            distances[i] = Vector3.Distance(rawPoints[i], rawPoints[i - 1]);
        }

        // Collecting equidistant points. 
        List<Vector3> positions = new List<Vector3>(sampleResolution);
        List<Vector3> directions = new List<Vector3>(sampleResolution);
        List<MapVector> mapVectors = new List<MapVector>(sampleResolution);
        float distance = 0;
        for (int i = 0; i < rawPoints.Length; i++)
        {
            distance += distances[i];
            if (distance >= segmentLenght)
            {
                positions.Add(rawPoints[i]);
                Vector3 direction = rawDirections[i];
                if (direction == Vector3.zero)
                {
                    direction = rawDirections[i - 1];
                    Debug.Log(direction);
                }
                directions.Add(rawDirections[i]);
                mapVectors.Add(rawMapVectors[i]);
                distance = 0;
            }
        }

        // Storing results.
        _precalculatedPositions = positions.ToArray();
        _precalculatedDirections = directions.ToArray();
        _precalculatedMapVectors = mapVectors.ToArray();
        _segmentCount = _precalculatedPositions.Length;
    }

    public Vector3 GetPrecalculatedDirection(int index)
    {
        return _precalculatedDirections[index];
    }

    public Vector3 GetPrecalculatedPoint(int index)
    {
        return _precalculatedPositions[index];
    }

    public MapVector GetPrecalculatedMapVector(int index)
    {
        return _precalculatedMapVectors[index];
    }

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
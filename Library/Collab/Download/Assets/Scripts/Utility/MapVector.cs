using UnityEngine;

/// <summary>
/// Used to detect projectiles between enemies
/// </summary>

[System.SerializableAttribute]
public struct MapVector
{
    [SerializeField]
    public int x;
    [SerializeField]
    public int z;

    public const int resolution = 1000;
    public const float resolutionMultiplier = 1f / resolution;

    public MapVector(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public MapVector(TilePosition tilePosition)
    {
        this.x = tilePosition.x * resolution;
        this.z = tilePosition.z * resolution;
    }

    // public static int CircularDistance(MapVector v1, MapVector v2)
    // {
    //     return (Mathf.Max(Mathf.Abs(v1.x - v2.x), Mathf.Abs(v1.z - v2.z)));
    // }

    public static int SquareDistance(MapVector v1, MapVector v2)
    {
        return ((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
    }

    public MapVector(Vector3 vector)
    {
        this.x = (int)(vector.x * resolution);
        this.z = (int)(vector.z * resolution);
    }

    // public MapVector(float x, float z)
    // {
    //     this.x = (int)(x * 1000f);
    //     this.z = (int)(z * 1000f);
    // }

    // public static MapVector Raw()
    // {
    //     return new MapVector
    // }

    public Vector3 ToVector3()
    {
        return new Vector3(x * resolutionMultiplier, 0f, z * resolutionMultiplier);
    }
}

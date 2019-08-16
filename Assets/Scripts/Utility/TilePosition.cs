using UnityEngine;

[System.Serializable]
public struct TilePosition
{
    public int x;
    public int z;

    public TilePosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public TilePosition(Vector3 vector)
    {
        x = (int)(vector.x + 0.5f);
        z = (int)(vector.z + 0.5f);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, 0, z);
    }

    public void SetFromVector3(Vector3 vector)
    {
        x = (int)(vector.x + 0.5f);
        z = (int)(vector.z + 0.5f);
    }

    // Uncomment if gonna implement the GetHashCode() 

    // static public bool operator ==(TilePosition p1, TilePosition p2)
    // {
    //     return p1.x == p2.x && p1.z == p2.z;
    // }

    // public override bool Equals(object o)
    // {
    //     return (o is TilePosition && (TilePosition)o == this);
    // }

    // static public bool operator !=(TilePosition p1, TilePosition p2)
    // {
    //     return p1.x == p2.x && p1.z == p2.z;
    // }

    static public TilePosition operator +(TilePosition p1, TilePosition p2)
    {
        return new TilePosition(p1.x + p2.x, p1.z + p2.z);
    }

    static public TilePosition operator -(TilePosition p1, TilePosition p2)
    {
        return new TilePosition(p1.x - p2.x, p1.z - p2.z);
    }
}

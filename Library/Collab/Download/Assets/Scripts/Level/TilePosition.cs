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

    static public bool operator ==(TilePosition m1, TilePosition m2)
    {
        if (m1.x == m2.x && m1.z == m2.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    static public bool operator !=(TilePosition m1, TilePosition m2)
    {
        if (m1.x == m2.x && m1.z == m2.z)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    static public TilePosition operator +(TilePosition m1, TilePosition m2)
    {
        return new TilePosition(m1.x + m2.x, m1.z + m2.z);
    }

    static public TilePosition operator -(TilePosition m1, TilePosition m2)
    {
        return new TilePosition(m1.x - m2.x, m1.z - m2.z);
    }
}

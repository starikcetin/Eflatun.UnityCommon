using UnityEngine;

/// <summary>
/// Serialization helper class for Vector3 type.
/// </summary>
[System.Serializable]
public class Vector3Serializer
{
    public float x;
    public float y;
    public float z;

    public void Fill(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }

    public Vector3 V3
    {
        get
        {
            return new Vector3(x, y, z);
        }
        set
        {
            Fill(value);
        }
    }
}
using UnityEngine;

/// <summary>
/// Provides fastest methods to convert a Vector3 to Vector2 and a Vector2 to Vector3
/// </summary>
public static class VectorConversions
{
    /// <summary>
    /// Converts this Vector3 to Vector2 using manual initialization. This is the fastest way to convert.
    /// </summary>
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    /// <summary>
    /// Converts this Vector2 to Vector3 using manual initialization. This is the fastest way to convert.
    /// </summary>
    public static Vector3 ToVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
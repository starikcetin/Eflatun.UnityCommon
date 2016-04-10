using UnityEngine;

namespace UnityCSharpCommon.Utils.Common
{
    public static class Utilities2D
    {
        /// <summary>
        /// Returns the 2D position of given transform on XY plane. Uses manual conversion.
        /// </summary>
        public static Vector2 Position2D(this Transform transform)
        {
            Vector3 pos = transform.position;
            return new Vector2(pos.x, pos.y);
        }

        /// <summary>
        /// Returns the 2D local position of given transform on XY plane. Uses manual conversion.
        /// </summary>
        public static Vector2 LocalPosition2D(this Transform transform)
        {
            Vector3 pos = transform.localPosition;
            return new Vector2(pos.x, pos.y);
        }

        /// <summary>
        /// Moves the transform to a Vector3 created with given X and Y values of given Vector2 and 0 Z value.
        /// </summary>
        public static void SetPosition2D(this Transform transform, Vector2 newPosition)
        {
            transform.position = new Vector3(newPosition.x, newPosition.y, 0);
        }

        /// <summary>
        /// Moves transform in local space to a Vector3 created with X and Y values of given Vector2 and 0 Z value.
        /// </summary>
        public static void SetLocalPosition2D(this Transform transform, Vector2 newLocalPosition)
        {
            transform.localPosition = new Vector3(newLocalPosition.x, newLocalPosition.y, 0);
        }
    }
}
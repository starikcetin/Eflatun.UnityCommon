using UnityEngine;

namespace UnityCSharpCommon.Utils.Common.Serialization
{
    /// <summary>
    /// Serialization helper class for Vector2 type.
    /// </summary>
    [System.Serializable]
    public class Vector2Serializer
    {
        public float x;
        public float y;

        public void Fill(Vector2 v2)
        {
            x = v2.x;
            y = v2.y;
        }

        public Vector2 V2
        {
            get { return new Vector2(x, y); }
            set { Fill(value); }
        }

        public static implicit operator Vector2(Vector2Serializer serializer)
        {
            return serializer.V2;
        }

        public static implicit operator Vector2Serializer(Vector2 vector)
        {
            var serializer = new Vector2Serializer {V2 = vector};
            return serializer;
        }
    }
}
using UnityEngine;

namespace starikcetin.UnityCommon.Utils.Serialization.Helpers
{
    /// <summary>
    /// Serialization helper class for Vector2 type.
    /// </summary>
    [System.Serializable]
    public class Vector2Serializer
    {
        public float X;
        public float Y;

        public void Fill(Vector2 v2)
        {
            X = v2.x;
            Y = v2.y;
        }

        public Vector2 V2
        {
            get { return new Vector2(X, Y); }
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

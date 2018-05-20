using UnityEngine;

namespace Eflatun.UnityCommon.Utils.Serialization.Helpers
{
    /// <summary>
    /// Serialization helper class for Vector3 type.
    /// </summary>
    [System.Serializable]
    public class Vector3Serializer
    {
        public float X;
        public float Y;
        public float Z;

        public void Fill(Vector3 v3)
        {
            X = v3.x;
            Y = v3.y;
            Z = v3.z;
        }

        public Vector3 V3
        {
            get { return new Vector3(X, Y, Z); }
            set { Fill(value); }
        }

        public static implicit operator Vector3(Vector3Serializer serializer)
        {
            return serializer.V3;
        }

        public static implicit operator Vector3Serializer(Vector3 vector)
        {
            var serializer = new Vector3Serializer {V3 = vector};
            return serializer;
        }
    }
}

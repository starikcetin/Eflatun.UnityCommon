using UnityEngine;

namespace UnityCSharpCommon.Utils.Common.Serialization
{
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
            get { return new Vector3(x, y, z); }
            set { Fill(value); }
        }

        public static implicit operator Vector3(Vector3Serializer serializer)
        {
            return serializer.V3;
        }

        public static implicit operator Vector3Serializer(Vector3 vector)
        {
            var serializer = new Vector3Serializer { V3 = vector };
            return serializer;
        }
    }
}
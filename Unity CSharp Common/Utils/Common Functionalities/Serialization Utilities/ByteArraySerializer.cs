using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityCSCommon.Utils.Common.Serialization
{
    /// <summary>
    /// Utilities for serializing to byte arrays and deserializing from byte arrays.
    /// </summary>
    public static class ByteArraySerializer
    {
        /// <summary>
        /// Serializes given <paramref name="input"/> to a byte array.
        /// </summary>
        public static byte[] SerializeToByteArray<T> (this T input)
        {
            if (input == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, input);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes given <paramref name="byteArray"/> as the type <typeparamref name="T"/>.
        /// </summary>
        public static T Deserialize<T> (this byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteArray, 0, byteArray.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T) new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
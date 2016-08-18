using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityCSCommon.Utils.Common.Serialization
{
    /// <summary>
    /// Utilities for serializing to byte array and deserializing from byte array.
    /// </summary>
    public static class ByteArraySerializer
    {
        public static byte[] Serialize<T>(ref T input)
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

        public static T Deserialize<T>(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(input, 0, input.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T) new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
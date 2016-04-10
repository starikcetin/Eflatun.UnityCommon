using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityCSharpCommon.Utils.Common.Serialization
{
    /// <summary>
    /// Utilities for serializing to byte and deserializing from byte.
    /// </summary>
    public static class ByteSerializeTools
    {
        public static byte[] ToByte_Serialize<T>(ref T input)
        {
            if (input == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, input);
                return ms.ToArray();
            }
        }

        public static T FromByte_Deserialize<T>(byte[] input)
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
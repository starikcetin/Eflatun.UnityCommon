using System;

namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Provides utilities for strings.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Casts <see cref="input"/> to <see cref="T"/> type. <para/>
        /// Please note that this is not guaranteed for types other than primitives and enums.
        /// </summary>
        public static T Cast<T> (this string input)
        {
            Type wantedType = typeof (T);

            if (wantedType.IsPrimitive)
            {
                //If the wanted type is Boolean but the input is in Integer form, we need to cast to Integer first.
                if (wantedType == typeof (bool))
                {
                    int intInput;
                    if (int.TryParse (input, out intInput))
                    {
                        return (T)Convert.ChangeType (intInput, wantedType);
                    }
                }

                return (T)Convert.ChangeType (input, wantedType);
            }
            else if (wantedType.IsEnum)
            {
                try
                {
                    return (T)Enum.Parse (wantedType, input);
                }
                catch (Exception)
                {
                    //We cannot parse to wanted Enum directly, try to cast to Integer and get the value from array of values.
                    int intInput;
                    if (int.TryParse (input, out intInput))
                    {
                        Array allValues = Enum.GetValues (wantedType);

                        if (intInput < 0 || intInput > allValues.Length - 1)
                        {
                            throw new IndexOutOfRangeException ("The 'input' is out of the range of the target enum's array of values.");
                        }

                        return (T)allValues.GetValue (intInput);
                    }

                    throw;
                }
            }
            else
            {
                return (T)(object)input;
            }
        }
    }
}
using System;
using UnityEngine;

namespace UnityCSharpCommon.Utils.Common
{
    /// <summary>
    /// A class that includes standard mathematical functions for types that doesn't included in Unity's Mathf class; such as double and long.
    /// Also includes some other useful mathematical functions.
    /// </summary>
    public static class Mathf2
    {
        public const int LayerMaskAll = ~0;

        public static double ClampDouble(this double value, double min, double max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }

        /// <summary>
        /// Rounds the decimal points of given double to fit the given decimalCount. (I.E.: 1.14d.LimitDecimals(1) -> 1.1d)
        /// </summary>
        public static double LimitDecimals(this double value, int decimalCount)
        {
            double rounded = Math.Round(value, decimalCount, MidpointRounding.AwayFromZero);
            return rounded;
        }

        /// <summary>
        /// Rounds the decimal points of given float to fit the given decimalCount. (I.E.: 1.14f.LimitDecimals(1) -> 1.1f)
        /// </summary>
        public static float LimitDecimals(this float value, int decimalCount)
        {
            float rounded = (float)Math.Round(value, decimalCount, MidpointRounding.AwayFromZero);
            return rounded;
        }

        /// <summary>
        /// <para>WARNING: Math.Sqrt is up to 200 times faster than this method! Don't use this as an optimization!</para>
        /// <para>Returns the smallest integer that is greater than or equal to the square root of given integer.</para>
        /// </summary>
        public static int CeiledSquareRoot(this int number)
        {
            int i = 0;
            while (i*i < number) i++;
            return i;
        }

        /// <summary>
        /// <para>WARNING: Math.Sqrt is up to 200 times faster than this method! Don't use this as an optimization!</para>
        /// <para>Returns the smallest integer that is greater than or equal to the square root of given float.</para>
        /// </summary>
        public static int CeiledSquareRoot(this float number)
        {
            int i = 0;
            while (i*i < number) i++;
            return i;
        }

        /// <summary>
        /// <para>WARNING: Math.Sqrt is up to 200 times faster than this method! Don't use this as an optimization!</para>
        /// <para>Returns the biggest integer that is smaller than or equal to the square root of given integer.</para>
        /// </summary>
        /// <remarks>The logic is same with CeiledSquareRoot, but here we also increment the value when we hit the number,
        /// so when we subtract 1, we get the square root if the number is square root-able.</remarks>
        public static int FlooredSquareRoot (this int number)
        {
            int i = 0;
            while (i*i <= number) i++;
            return i - 1;
        }

        /// <summary>
        /// <para>WARNING: Math.Sqrt is up to 200 times faster than this method! Don't use this as an optimization!</para>
        /// <para>Returns the biggest integer that is smaller than or equal to the square root of given float.</para>
        /// </summary>
        /// <remarks>The logic is same with CeiledSquareRoot, but here we also increment the value when we hit the number,
        /// so when we subtract 1, we get the square root if the number is square root-able.</remarks>
        public static int FlooredSquareRoot(this float number)
        {
            int i = 0;
            while (i*i <= number) i++;
            return i - 1;
        }

        /// <summary>
        /// Returns the <paramref name="exp"/>'th power of <paramref name="baseN"/>.
        /// </summary>
        /// <param name="baseN">Base.</param>
        /// <param name="exp">Exponent (Power).</param>
        /// <remarks>Float raised to float = float</remarks>
        public static float Pow(this float baseN, float exp)
        {
            return (float)Math.Pow (baseN, exp);
        }

        /// <summary>
        /// Returns the <paramref name="exp"/>'th power of <paramref name="baseN"/>.
        /// </summary>
        /// <param name="baseN">Base.</param>
        /// <param name="exp">Exponent (Power).</param>
        /// <remarks>Integer raised to float = float</remarks>
        public static float Pow(this int baseN, float exp)
        {
            return (float)Math.Pow (baseN, exp);
        }

        /// <summary>
        /// Returns the <paramref name="exp"/>'th power of <paramref name="baseN"/>.
        /// </summary>
        /// <param name="baseN">Base.</param>
        /// <param name="exp">Exponent (Power).</param>
        /// <remarks>Integer raised to integer = integer</remarks>
        public static int Pow(this int baseN, int exp)
        {
            return (int)Math.Pow(baseN, exp);
        }

        /// <summary>
        /// Normalizes the angle to 0-360 range.
        /// </summary>
        public static float NormalizeAngle(this float angle)
        {
            angle = (angle + 360)%360;

            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }

        /// <summary>
        /// <para>Calculates the shortest rotation direction from this angle to the one given in paranthesis.</para>
        /// <para>If return value is positive, shortest angle is positive way; if return value is negative shortest angle is negative way.</para>
        /// <para>If return value is 0, this means angles are equal.</para>
        /// </summary>
        /// <param name="from">The angle to start from.</param>
        /// <param name="to"> The destination angle of rotation.</param>
        public static float CalcShortestRotTo(this float from, float to)
        {
            // If from or to is a negative, we have to recalculate them.
            // For an example, if from = -45 then from(-45) + 360 = 315.
            if (from < 0)
            {
                from += 360;
            }

            if (to < 0)
            {
                to += 360;
            }

            // Do not rotate if from == to.
            if (from == to ||
                from == 0 && to == 360 ||
                from == 360 && to == 0)
            {
                return 0;
            }

            // Pre-calculate left and right.
            float left = (360 - from) + to;
            float right = from - to;
            // If from < to, re-calculate left and right.
            if (from < to)
            {
                if (to > 0)
                {
                    left = to - from;
                    right = (360 - to) + from;
                }
                else
                {
                    left = (360 - to) + from;
                    right = to - from;
                }
            }

            // Determine the shortest direction.
            return ((left <= right) ? left : (right*-1));
        }

        /// <summary>
        /// Determines if given mask includes given layer. Layer parameter must not be bit-shifted, bit-shifting is being done inside this method.
        /// </summary>
        public static bool MaskIncludes(int mask, int layer)
        {
            int shifted = 1 << layer;
            return (mask & shifted) == shifted;
        }

        /// <summary>
        /// !!! WARNING: This method is just a reminder on how to use the function. DO PREFER inlining this method, because this method creates a Vector2 which causes stack usage. !!!
        /// </summary>
        public static Vector2 MultiplyPoint2d_3x4(Matrix4x4 matrix, Vector2 point)
        {
            // ----
            // Formulas used in transforming local to world have been directly taken from assmebly view of Matrix4x4.MultiplyPoint3x4.
            // I deleted everything related to Z axis, and this yielded a much much better performance.
            // ----
            // While using, don't even create a new vector, just assign X and Y directly when possible.
            // Because Vector2..ctor creates a serious impact on performance when used regularly.
            // ----

            Vector2 result;
            result.x = matrix.m00*point.x + matrix.m01*point.y + matrix.m03;
            result.y = matrix.m10*point.x + matrix.m11*point.y + matrix.m13;
            return result;
        }

        /// <summary>
        /// Mirrors the <paramref name="value"/> by <paramref name="origin"/>.
        /// </summary>
        public static int MirrorBy(this int value, int origin)
        {
            return origin + (origin - value);
        }

        /// <summary>
        /// Mirrors the <paramref name="value"/> by <paramref name="origin"/>.
        /// </summary>
        public static double MirrorBy(this double value, double origin)
        {
            return origin + (origin - value);
        }

        /// <summary>
        /// Mirrors the <paramref name="value"/> by <paramref name="origin"/>.
        /// </summary>
        public static float MirrorBy(this float value, float origin)
        {
            return origin + (origin - value);
        }
    }
}
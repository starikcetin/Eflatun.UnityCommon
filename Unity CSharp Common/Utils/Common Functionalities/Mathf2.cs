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
        /// Returns the smallest integer that is greater than or equal to the square root of given integer.
        /// </summary>
        public static int CeiledSquareRoot(this int number)
        {
            int i = 1;
            while (i*i < number) i++;
            return i;
        }

        /// <summary>
        /// Returns the smallest integer that is greater than or equal to the square root of given float.
        /// </summary>
        public static int CeiledSquareRoot(this float number)
        {
            int i = 1;
            while (i*i < number) i++;
            return i;
        }

        /// <summary>
        /// Returns the exp'th power of baseF.
        /// </summary>
        /// <param name="baseF">Base.</param>
        /// <param name="exp">Exponent (Power).</param>
        public static float Pow(this float baseF, float exp)
        {
            return Mathf.Pow(baseF, exp);
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
        /// Calculates the shortest rotatation direction from this float to the one given in paranthesis.
        /// If return value is positive, shortest angle is positive way; if return value is negative shortest angle is negative way.
        /// If return value is 0, this means angles are equal.
        /// </summary>
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
        /// Detemrines if given mask includes given layer. Layer parameter must not be bit-shifted, bit-shifting is being done inside this method.
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
    }
}
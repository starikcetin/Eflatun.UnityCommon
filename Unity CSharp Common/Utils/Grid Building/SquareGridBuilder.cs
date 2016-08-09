using System.Collections.Generic;
using UnityCSharpCommon.Utils.Common;
using UnityEngine;

namespace UnityCSharpCommon.Utils.GridBuilding
{
    /// <summary>
    /// Methods for generating symmetrical square grids.
    /// </summary>
    public static class SquareGridBuilder
    {
        /// <summary>
        /// Generates a grid inside a circle.
        /// </summary>
        /// <param name="origin">Origin of circle.</param>
        /// <param name="radius">Radius of circle.</param>
        /// <param name="nodeDistance">Distance between nodes.</param>
        public static List<Vector2> BuildGridInCircle (Vector2 origin, float radius, float nodeDistance)
        {
            float offset = radius % nodeDistance;

            Vector2 min = new Vector2(origin.x - radius, origin.y - radius);
            Vector2 max = new Vector2(origin.x + radius, origin.y + radius);

            var x = min.x + offset;
            var y = min.y + offset;

            List<Vector2> allNodes = new List<Vector2>();
            while (x <= max.x)
            {
                while (y <= max.y)
                {
                    var candidate = new Vector2(x, y);
                    if (candidate.TestDistanceMax(origin, radius))
                    {
                        allNodes.Add(candidate);
                    }

                    y += nodeDistance;
                }
                y = min.y + offset;
                x += nodeDistance;
            }

            return allNodes;
        }

        /// <summary>
        /// Generates a grid inside a rectangle.
        /// </summary>
        /// <param name="min">Minumun point of rectangle.</param>
        /// <param name="max">Maximum point of rectangle.</param>
        /// <param name="nodeDistance">Distance between nodes.</param>
        public static List<Vector2> BuildGridInRectangle (Vector2 min, Vector2 max, float nodeDistance)
        {
            float xOffset = ((max.x - min.x) / 2) % nodeDistance;
            float yOffset = ((max.y - min.y) / 2) % nodeDistance;

            var x = min.x + xOffset;
            var y = min.y + yOffset;

            List<Vector2> allNodes = new List<Vector2>();
            while (x <= max.x)
            {
                while (y <= max.y)
                {
                    allNodes.Add(new Vector2(x, y));
                    y += nodeDistance;
                }
                y = min.y + yOffset;
                x += nodeDistance;
            }

            return allNodes;
        }
    }
}
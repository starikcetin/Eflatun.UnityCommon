using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCSharpCommon.Utils.Common
{
    /// <summary>
    /// Includes 2D Geometry functions.
    /// </summary>
    public static class Geometry2D
    {
        /// <summary>
        /// Converts polar coordinates to cartesian coordinates.
        /// </summary>
        /// <param name="radius">Magnitude of position vector.</param>
        /// <param name="angle">Positive rotation of position vector from +x.</param>
        /// <returns> Cartesian equivelant of given polar coordinates. </returns>
        public static Vector2 PolarToCartesian(float radius, float angle)
        {
            var x = radius * Mathf.Cos(angle);
            var y = radius * Mathf.Sin(angle);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Converts cartesian coordinates to polar coordinates.
        /// </summary>
        /// <param name="cartesian">Carteisan coordinates.</param>
        /// <param name="radius">Magnitude of position vector.</param>
        /// <param name="angle">Positive rotation of position vector from +x.</param>
        public static void CartesianToPolar (this Vector2 cartesian, out float radius, out float angle)
        {
            radius = cartesian.magnitude;
            angle = Mathf.Atan2(cartesian.y, cartesian.x);
        }

        /// <summary>
        /// Generates a random Vector2 whose length is 1.
        /// </summary>
        public static Vector2 RandomUnitVector2()
        {
            Vector2 rnd = UnityEngine.Random.insideUnitCircle;
            return rnd.normalized;
        }

        /// <summary>
        /// Returns the positive angle in degrees of given Vector2. This method takes +X axis as 0 degrees.
        /// </summary>
        public static float Rotation(this Vector2 vector2)
        {
            return Mathf.Atan2(vector2.y, vector2.x)*Mathf.Rad2Deg;
        }

        /// <summary>
        /// Rotates the Vector2 by given angle.
        /// </summary>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees*Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees*Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos*tx) - (sin*ty);
            v.y = (sin*tx) + (cos*ty);
            return v;
        }

        /// <summary>
        /// Returns the squared distance between two Vector2s.
        /// </summary>
        public static float SqrDistance(Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude;
        }

        /// <summary>
        /// <para>Returns the weighted center of all the points given.</para>
        /// <para>If weighted is true, center point will be closer to the area that points are denser; if false, center will be the geometric exact center of bounding box of points.</para>
        /// </summary>
        public static Vector2 FindCenter(this List<Vector2> points, bool weighted)
        {
            if (points.Count == 0)
            {
                return Vector2.zero;
            }
            else if (points.Count == 1)
            {
                return points[0];
            }

            if (weighted)
            {
                Vector2 center = Vector2.zero;

                foreach (var point in points)
                {
                    center += point;
                }

                center /= points.Count;
                return center;
            }
            else
            {
                Bounds bound = new Bounds();
                bound.center = new Vector3(points[0].x, points[0].y, 0);

                foreach (var point in points)
                {
                    bound.Encapsulate(point);
                }

                return new Vector2(bound.center.x, bound.center.y);
            }
        }

        /// <summary>
        /// <para>Returns the weighted center of all the points given.</para>
        /// <para>If weighted is true, center point will be closer to the area that points are denser; if false, center will be the geometric exact center of bounding box of points.</para>
        /// </summary>
        public static Vector2 FindCenter2D(this List<GameObject> gameObjects, bool weighted)
        {
            if (gameObjects.Count == 0)
            {
                return Vector2.zero;
            }
            else if (gameObjects.Count == 1)
            {
                Vector3 pos = gameObjects[0].transform.position;
                return new Vector2(pos.x, pos.y);
            }

            if (weighted)
            {
                Vector2 center = Vector2.zero;

                foreach (var gameObject in gameObjects)
                {
                    Vector3 pos = gameObject.transform.position;
                    center += new Vector2(pos.x, pos.y);
                }

                center /= gameObjects.Count;
                return center;
            }
            else
            {
                Bounds bound = new Bounds();
                Vector3 pos = gameObjects[0].transform.position;
                bound.center = new Vector2(pos.x, pos.y);

                foreach (var gameObject in gameObjects)
                {
                    Vector3 posIte = gameObject.transform.position;
                    bound.Encapsulate(new Vector2(posIte.x, posIte.y));
                }

                return new Vector2(bound.center.x, bound.center.y);
            }
        }

        /// <summary>
        /// Determines if AABBs represented with given parameters are overlapping.
        /// </summary>
        public static bool TestOverlap(Vector2 min1, Vector2 max1, Vector2 min2, Vector2 max2)
        {
            //--- Step 1: Check for left. ---
            if (max1.x < min2.x) //if maximum x point of test is smaller than minimum x (falls to the left of test area)
            {
                return false;
            }

            //--- Step 2: Check for right. ---
            if (min1.x > max2.x) //if minimum x point of test is bigger than maximum x (falls to the right of test area)
            {
                return false;
            }

            //--- Step 3: Check for up. ---
            if (min1.y > max2.y) //if minimum y point of test is bigger than maximum y (falls to the up of test area)
            {
                return false;
            }

            //--- Step 4: Check for down. ---
            if (max1.y < min2.y) //if maximum y point of test is smaller than minimum y (falls to the down of test area)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// <para> Determines if given point is inside given polygon. Polygon must be convex. </para>
        /// <para> </para>
        /// <para> This method uses cross product to determine which side of the edge the point is. </para>
        /// </summary>
        public static bool IsInConvexPoly(this Vector2 point, Vector2[] verticesOfPolygon)
        {
            //Get shared variables.
            var vertexCount = verticesOfPolygon.Length;
            var px = point.x;
            var py = point.y;

            //Vertex count can't be less than 3.
            if (vertexCount < 3) return false;

            //n>2 Keep track of cross product sign changes
            var pos = 0;
            var neg = 0;

            for (var i = 0; i < vertexCount; i++)
            {
                //Form a segment between the i'th point
                var x1 = verticesOfPolygon[i].x;
                var y1 = verticesOfPolygon[i].y;

                //And the i+1'th, or if i is the last, with the first point
                var i2 = i < vertexCount - 1 ? i + 1 : 0;

                var x2 = verticesOfPolygon[i2].x;
                var y2 = verticesOfPolygon[i2].y;

                //Compute the cross product
                var d = (px - x1)*(y2 - y1) - (py - y1)*(x2 - x1);

                if (d > 0) pos++;
                if (d < 0) neg++;

                //If the sign changes, then point is outside
                if (pos > 0 && neg > 0)
                    return false;
            }

            //If no change in direction, then on same side of all segments, and thus inside
            return true;
        }

        /// <summary>
        /// <para> Determines if given point is inside given polygon. Polygon can be concave or convex. </para>
        /// <para> This method uses an algorithm that casts an imaginary ray counting amount of sides it intersects until it reaches the point. If the count is even, point is outside; if count is odd point is inside. </para>
        /// </summary>
        public static bool IsInPoly(this Vector2 point, Vector2[] verticesOfPolygon)
        {
            // Original raycast algorithm: http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            // C# conversion:              http://stackoverflow.com/a/16391873/5504706

            //Get shared variables.
            var vertexCount = verticesOfPolygon.Length;
            var px = point.x;
            var py = point.y;

            //Vertex count can't be less than 3.
            if (vertexCount < 3) return false;

            bool inside = false;
            for (int i = 0, j = vertexCount - 1; i < vertexCount; j = i++)
            {
                //Get points.
                var ix = verticesOfPolygon[i].x;
                var iy = verticesOfPolygon[i].y;
                var jy = verticesOfPolygon[j].y;
                var jx = verticesOfPolygon[j].x;

                //Cast the ray.
                if ((iy > py) != (jy > py) && px < (jx - ix)*(py - iy)/(jy - iy) + ix)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        /// <summary>
        /// Determines if the distance between given points are lower than <paramref name="maxDistance"/>. (Doesn't use square root.)
        /// </summary>
        public static bool TestDistanceMax(this Vector2 point1, Vector2 point2, float maxDistance)
        {
            Vector2 relativePos = (point2 - point1);
            float sqrDistance = (relativePos.x*relativePos.x) + (relativePos.y*relativePos.y); //square magnitude
            return sqrDistance < maxDistance * maxDistance;
        }

        /// <summary>
        /// Determines if the distance between given points are greater than or equal to <paramref name="minDistance"/>. (Doesn't use square root.)
        /// </summary>
        public static bool TestDistanceMin(this Vector2 point1, Vector2 point2, float minDistance)
        {
            Vector2 relativePos = (point2 - point1);
            float sqrDistance = (relativePos.x * relativePos.x) + (relativePos.y * relativePos.y); //square magnitude
            return sqrDistance >= minDistance * minDistance;
        }

        /// <summary>
        /// Determines if the distance between given points are greater than or equal to <paramref name="minDistance"/> and lower than <paramref name="maxDistance"/>. (Doesn't use square root.)
        /// </summary>
        public static bool TestDistanceMinMax(this Vector2 point1, Vector2 point2, float minDistance, float maxDistance)
        {
            Vector2 relativePos = (point2 - point1);
            float sqrDistance = (relativePos.x * relativePos.x) + (relativePos.y * relativePos.y); //square magnitude
            return sqrDistance < maxDistance * maxDistance && sqrDistance >= minDistance * minDistance;
        }

        #region Convex Hull Calculations
        /// <summary>
        /// A wrapper around list version. Converts input to list, makes calculations, converts output to array then returns.
        /// </summary>
        public static Vector2[] MakeConvexHull(this Vector2[] points)
        {
            return MakeConvexHull(points.ToList()).ToArray();
        }

        /// <summary>
        /// Return the points that make up a polygon's convex hull. This method leaves the points list unchanged.
        /// </summary>
        public static List<Vector2> MakeConvexHull(this List<Vector2> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest y value.
            // if (there's a tie, take the one with the smaller x value.
            Vector2 best_pt = points[0];
            foreach (Vector2 pt in points)
            {
                if ((pt.y < best_pt.y) ||
                    ((pt.y == best_pt.y) && (pt.x < best_pt.x)))
                {
                    best_pt = pt;
                }
            }

            // Move this point to the convex hull.
            List<Vector2> hull = new List<Vector2>();
            hull.Add(best_pt);
            points.Remove(best_pt);

            // Start wrapping up the other points.
            float sweep_angle = 0;
            for (;;)
            {
                // Find the point with smallest AngleValue
                // from the last point.
                float x = hull[hull.Count - 1].x;
                float y = hull[hull.Count - 1].y;
                best_pt = points[0];
                float best_angle = 3600;

                // Search the rest of the points.
                foreach (Vector2 pt in points)
                {
                    float test_angle = AngleValue(x, y, pt.x, pt.y);
                    if ((test_angle >= sweep_angle) &&
                        (best_angle > test_angle))
                    {
                        best_angle = test_angle;
                        best_pt = pt;
                    }
                }

                // See if the first point is better.
                // If so, we are done.
                float first_angle = AngleValue(x, y, hull[0].x, hull[0].y);
                if ((first_angle >= sweep_angle) &&
                    (best_angle >= first_angle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(best_pt);
                points.Remove(best_pt);

                sweep_angle = best_angle;

                // If all of the points are on the hull, we're done.
                if (points.Count == 0) break;
            }

            return hull;
        }

        /// <summary>
        /// Find the points nearest the upper left, upper right, lower left, and lower right corners.
        /// </summary>
        private static void GetMinMaxCorners(List<Vector2> points, ref Vector2 ul, ref Vector2 ur, ref Vector2 ll, ref Vector2 lr)
        {
            // Start with the first point as the solution.
            ul = points[0];
            ur = ul;
            ll = ul;
            lr = ul;

            // Search the other points.
            foreach (Vector2 pt in points)
            {
                if (-pt.x - pt.y > -ul.x - ul.y) ul = pt;
                if (pt.x - pt.y > ur.x - ur.y) ur = pt;
                if (-pt.x + pt.y > -ll.x + ll.y) ll = pt;
                if (pt.x + pt.y > lr.x + lr.y) lr = pt;
            }
        }

        /// <summary>
        /// Find a box that fits inside the MinMax quadrilateral.
        /// </summary>
        private static Rect GetMinMaxBox(List<Vector2> points)
        {
            // Find the MinMax quadrilateral.
            Vector2 ul = new Vector2(0, 0), ur = ul, ll = ul, lr = ul;
            GetMinMaxCorners(points, ref ul, ref ur, ref ll, ref lr);

            // Get the coordinates of a box that lies inside this quadrilateral.
            float xmin, xmax, ymin, ymax;
            xmin = ul.x;
            ymin = ul.y;

            xmax = ur.x;
            if (ymin < ur.y) ymin = ur.y;

            if (xmax > lr.x) xmax = lr.x;
            ymax = lr.y;

            if (xmin < ll.x) xmin = ll.x;
            if (ymax > ll.y) ymax = ll.y;

            Rect result = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);

            return result;
        }

        /// <summary>
        /// Cull points out of the convex hull that lie inside the trapezoid defined by the vertices with smallest and largest x and y coordinates. Return the points that are not culled.
        /// </summary>
        private static List<Vector2> HullCull(List<Vector2> points)
        {
            // Find a culling box.
            Rect culling_box = GetMinMaxBox(points);

            // Cull the points.
            List<Vector2> results = new List<Vector2>();
            foreach (Vector2 pt in points)
            {
                // See if (this point lies outside of the culling box.
                if (pt.x <= culling_box.xMin ||
                    pt.x >= culling_box.xMax ||
                    pt.y <= culling_box.yMin ||
                    pt.y >= culling_box.yMax)
                {
                    // This point cannot be culled.
                    // Add it to the results.
                    results.Add(pt);
                }
            }

            return results;
        }

        /// <summary>
        /// Return a number that gives the ordering of angles WRST horizontal from the point (x1, y1) to (x2, y2).
        /// Look inside the method for detailed description.
        /// </summary>
        private static float AngleValue(float x1, float y1, float x2, float y2)
        {
            // Return a number that gives the ordering of angles
            // WRST horizontal from the point (x1, y1) to (x2, y2).
            // In other words, AngleValue(x1, y1, x2, y2) is not
            // the angle, but if:
            //   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
            // then
            //   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
            // this angle is greater than the angle for another set
            // of points,) this number for
            //
            // This function is dy / (dy + dx).

            float dx, dy, ax, ay, t;

            dx = x2 - x1;
            ax = Mathf.Abs(dx);
            dy = y2 - y1;
            ay = Mathf.Abs(dy);
            if (ax + ay == 0)
            {
                // if (the two points are the same, return 360.
                t = 360f/9f;
            }
            else
            {
                t = dy/(ax + ay);
            }
            if (dx < 0)
            {
                t = 2 - t;
            }
            else if (dy < 0)
            {
                t = 4 + t;
            }
            return t*90;
        }
        #endregion
    }
}
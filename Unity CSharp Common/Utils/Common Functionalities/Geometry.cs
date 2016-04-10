using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCSharpCommon.Utils.Common
{
    public static class Geometry
    {
        /// <summary>
        /// Finds the closest Transform in allTargets.
        /// </summary>
        public static Transform FindClosest(this Transform origin, Transform[] allTargets)
        {
            if (!origin)
            {
                throw new System.ArgumentNullException("origin");
            }

            int targetCount = allTargets.Count();
            if (targetCount == 0)
            {
                return null;
            }
            else if (targetCount == 1)
            {
                return allTargets[0];
            }

            Vector2 originPos = origin.Position2D();

            float closestDistance = Mathf.Infinity;
            Transform closest = null;

            foreach (var iteratingTarget in allTargets)
            {
                Vector2 relPosVector = iteratingTarget.Position2D() - originPos;
                float distanceSqr = relPosVector.sqrMagnitude;

                if (distanceSqr < closestDistance)
                {
                    closestDistance = distanceSqr;
                    closest = iteratingTarget;
                }
            }

            return closest;
        }

        /// <summary>
        /// Finds the closest GameObject in allTargets.
        /// </summary>
        public static GameObject FindClosest(this GameObject origin, GameObject[] allTargets)
        {
            if (!origin)
            {
                throw new System.ArgumentNullException("origin");
            }

            int targetCount = allTargets.Count();
            if (targetCount == 0)
            {
                return null;
            }
            else if (targetCount == 1)
            {
                return allTargets[0];
            }

            Vector2 originPos = origin.transform.Position2D();

            float closestDistance = Mathf.Infinity;
            GameObject closest = null;

            foreach (var iteratingTarget in allTargets)
            {
                Vector2 relPosVector = iteratingTarget.transform.Position2D() - originPos;
                float distanceSqr = relPosVector.sqrMagnitude;

                if (distanceSqr < closestDistance)
                {
                    closestDistance = distanceSqr;
                    closest = iteratingTarget;
                }
            }

            return closest;
        }

        /// <summary>
        /// <para>Returns the weighted center of all the points given.</para>
        /// <para>If weighted is true, center point will be closer to the area that points are denser; if false, center will be the geometric exact center of bounding box of points.</para>
        /// </summary>
        public static Vector3 FindCenter(this List<Vector3> points, bool weighted)
        {
            if (points.Count == 0)
            {
                return Vector3.zero;
            }
            else if (points.Count == 1)
            {
                return points[0];
            }

            if (weighted)
            {
                Vector3 center = Vector3.zero;

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
                bound.center = points[0];

                foreach (var point in points)
                {
                    bound.Encapsulate(point);
                }

                return bound.center;
            }
        }

        /// <summary>
        /// <para>Returns the weighted center of all the points given.</para>
        /// <para>If weighted is true, center point will be closer to the area that points are denser; if false, center will be the geometric exact center of bounding box of points.</para>
        /// </summary>
        public static Vector3 FindCenter(this List<GameObject> gameObjects, bool weighted)
        {
            if (gameObjects.Count == 0)
            {
                return Vector3.zero;
            }
            else if (gameObjects.Count == 1)
            {
                return gameObjects[0].transform.position;
            }

            if (weighted)
            {
                Vector3 center = Vector3.zero;

                foreach (var gameObject in gameObjects)
                {
                    center += gameObject.transform.position;
                }

                center /= gameObjects.Count;
                return center;
            }
            else
            {
                Bounds bound = new Bounds();
                bound.center = gameObjects[0].transform.position;

                foreach (var gameObject in gameObjects)
                {
                    bound.Encapsulate(gameObject.transform.position);
                }

                return bound.center;
            }
        }
    }
}
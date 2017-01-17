using System.Collections.Generic;
using UnityCSCommon.Utils.CodePatterns;
using UnityCSCommon.Utils.Common;
using UnityEngine;

namespace UnityCSCommon.Utils.ManualTracking2D
{
    public class ObjectTracker : GlobalSingleton<ObjectTracker>
    {
        protected ObjectTracker() { } //prevent init

        #region Record Keeping
        public void RegisterObject(GameObject toRegister, GameObject prefab)
        {
            var renderer = toRegister.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                var localConvexHull = ConvexHullDatabase.Instance.GetLocalConvexHull(prefab);

                var newData = new TrackedObjectData(toRegister, renderer, localConvexHull);
                ObjectTrackerDatabase.Add(toRegister, newData);
            }
        }

        public void UnregisterObject(GameObject toUnregister)
        {
            ObjectTrackerDatabase.Remove(toUnregister);
        }
        #endregion

        #region Functionalities (Multi Returners)
        /// <summary>
        /// Similar to RaycastAll.
        /// </summary>
        public List<GameObject> ByPoint_All(Vector2 point, int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;

            //--- Preparation: Determine if we need to check for layers.  ---
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;

            //--- Iterate: Test all datas. ---
            var results = new List<GameObject>();

            int count = allDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = allDatas[i];

                //--- Test: Run the point test. ---
                if (Test_ByPoint(item, point, layerMask, checkForLayers))
                {
                    results.Add(item.GameObject);
                }
            }

            //--- Finally: Return all. ---
            return results;
        }

        /// <summary>
        /// Similar to OverlapAreaAll.
        /// </summary>
        public List<GameObject> ByBox_All(Vector2 startPoint, Vector2 endPoint, int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;

            //--- Preparation: Determine if we need to check for layers.  ---
            //---              Determine max and min points of test area. ---
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;

            Vector2 min = new Vector2();
            Vector2 max = new Vector2();

            if (startPoint.x > endPoint.x)
            {
                //start point's X is bigger.
                max.x = startPoint.x;
                min.x = endPoint.x;
            }
            else
            {
                //end point's X is bigger or equal.
                max.x = endPoint.x;
                min.x = startPoint.x;
            }

            if (startPoint.y > endPoint.y)
            {
                //start point's Y is bigger.
                max.y = startPoint.y;
                min.y = endPoint.y;
            }
            else
            {
                //end point's Y is bigger or equal.
                max.y = endPoint.y;
                min.y = startPoint.y;
            }

            //--- Iterate: Test all datas. ---
            List<GameObject> results = new List<GameObject>();

            int count = allDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = allDatas[i];

                //--- Test: Run the box test. ---
                if (Test_ByBox(item, min, max, layerMask, checkForLayers))
                {
                    results.Add(item.GameObject);
                }
            }

            //--- Finally: Return all. ---
            return results;
        }

        /// <summary>
        /// Similar to OverlapCircleAll.
        /// </summary>
        public List<GameObject> ByCircle_All(Vector2 center, float radius, int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;

            //--- Preparation: Determine if we need to check for layers.  ---
            //---              Calculate squared radius.                  ---
            //---              Determine max and min points of test area. ---
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
            float sqrRadius = radius*radius;
            Vector2 min = new Vector2(center.x - radius, center.y - radius);
            Vector2 max = new Vector2(center.x + radius, center.y + radius);

            //--- Iterate: Test all datas. ---
            List<GameObject> results = new List<GameObject>();

            int count = allDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = allDatas[i];

                //--- Test: Run the circle test. ---
                if (Test_ByCircle(item, center, sqrRadius, min, max, layerMask, checkForLayers))
                {
                    results.Add(item.GameObject);
                }
            }

            //--- Finally: Return all. ---
            return results;
        }

        #endregion

        #region Functionalities (Single Returners)

        /// <summary>
        /// Similar to Raycast.
        /// </summary>
        public GameObject ByPoint_Single(Vector2 point, int layermask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;
            var pickedData = Pick_ByPoint(allDatas, point, layermask);
            return GetGameObjectOrNull(pickedData);
        }

        /// <summary>
        /// Similar to OverlapArea.
        /// </summary>
        public GameObject ByBox_Single(Vector2 startPoint, Vector2 endPoint, int layerMask = Mathf2.LayerMaskAll)
        {
            //--- Preparation: Determine max and min points of test area. ---
            Vector2 min = new Vector2();
            Vector2 max = new Vector2();

            if (startPoint.x > endPoint.x)
            {
                //start point's X is bigger.
                max.x = startPoint.x;
                min.x = endPoint.x;
            }
            else
            {
                //end point's X is bigger or equal.
                max.x = endPoint.x;
                min.x = startPoint.x;
            }

            if (startPoint.y > endPoint.y)
            {
                //start point's Y is bigger.
                max.y = startPoint.y;
                min.y = endPoint.y;
            }
            else
            {
                //end point's Y is bigger or equal.
                max.y = endPoint.y;
                min.y = startPoint.y;
            }

            //--- Pick: Pick by box. ---
            var allDatas = ObjectTrackerDatabase.AllDatas;
            var pickedData = Pick_ByBox(allDatas, min, max, layerMask);
            return GetGameObjectOrNull(pickedData);
        }

        /// <summary>
        /// Similar to OverlapCircle.
        /// </summary>
        public GameObject ByCircle_Single(Vector2 center, float radius, int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;
            var pickedData = Pick_ByCircle(allDatas, center, radius, layerMask);
            return GetGameObjectOrNull(pickedData);
        }

        #endregion

        #region Functionalities (Non-Alloc Multi Returners)

        /// <summary>
        /// Similar to RaycastNonAlloc
        /// </summary>
        public int ByPoint_AllNonAlloc(GameObject[] outputArray, Vector2 point, int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;

            //--- Preparation: Determine if we need to check for layers.  ---
            //---              Get output capacity.                       ---
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
            int outputCapacity = outputArray.Length;

            //--- Iterate: Test all datas. ---
            int resultCount = 0;
            int count = allDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = allDatas[i];

                //--- Test: Run the point test. ---
                if (Test_ByPoint(item, point, layerMask, checkForLayers))
                {
                    //--- Finally: Assign to current index. ---
                    outputArray[resultCount] = item.GameObject;
                    resultCount++;

                    // Check if we hit the capacity of output array.
                    if (resultCount == outputCapacity)
                    {
                        break;
                    }
                }
            }

            //--- Finally: Return result count. ---
            return resultCount;
        }

        /// <summary>
        /// Similar to OverlapAreaNonAlloc
        /// </summary>
        public int ByBox_AllNonAlloc(GameObject[] outputArray, Vector2 startPoint, Vector2 endPoint,
            int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;

            //--- Preparation: Determine if we need to check for layers.  ---
            //---              Get output capacity.                       ---
            //---              Determine max and min points of test area. ---
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
            int outputCapacity = outputArray.Length;

            Vector2 min = new Vector2();
            Vector2 max = new Vector2();

            if (startPoint.x > endPoint.x)
            {
                //start point's X is bigger.
                max.x = startPoint.x;
                min.x = endPoint.x;
            }
            else
            {
                //end point's X is bigger or equal.
                max.x = endPoint.x;
                min.x = startPoint.x;
            }

            if (startPoint.y > endPoint.y)
            {
                //start point's Y is bigger.
                max.y = startPoint.y;
                min.y = endPoint.y;
            }
            else
            {
                //end point's Y is bigger or equal.
                max.y = endPoint.y;
                min.y = startPoint.y;
            }

            //--- Iterate: Test all datas. ---
            int resultCount = 0;
            int count = allDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = allDatas[i];

                //--- Test: Run the box test. ---
                if (Test_ByBox(item, min, max, layerMask, checkForLayers))
                {
                    //--- Finally: Assign to current index. ---
                    outputArray[resultCount] = item.GameObject;
                    resultCount++;

                    // Check if we hit the capacity of output array.
                    if (resultCount == outputCapacity)
                    {
                        break;
                    }
                }
            }

            //--- Finally: Return result count. ---
            return resultCount;
        }

        /// <summary>
        /// Similar to OverlapCircleNonAlloc
        /// </summary>
        public int ByCircle_AllNonAlloc(GameObject[] outputArray, Vector2 center, float radius,
            int layerMask = Mathf2.LayerMaskAll)
        {
            var allDatas = ObjectTrackerDatabase.AllDatas;

            //--- Preparation: Determine if we need to check for layers.  ---
            //---              Calculate squared radius.                  ---
            //---              Get output capacity.                       ---
            //---              Determine max and min points of test area. ---
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
            float sqrRadius = radius*radius;
            int outputCapacity = outputArray.Length;
            Vector2 min = new Vector2(center.x - radius, center.y - radius);
            Vector2 max = new Vector2(center.x + radius, center.y + radius);

            //--- Iterate: Test all datas. ---
            int resultCount = 0;
            int count = allDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = allDatas[i];

                //--- Test: Run the circle test. ---
                if (Test_ByCircle(item, center, sqrRadius, min, max, layerMask, checkForLayers))
                {
                    //--- Finally: Assign to current index. ---
                    outputArray[resultCount] = item.GameObject;
                    resultCount++;

                    // Check if we hit the capacity of output array.
                    if (resultCount == outputCapacity)
                    {
                        break;
                    }
                }
            }

            //--- Finally: Return result count. ---
            return resultCount;
        }

        #endregion

        #region Test Runners

        private bool Test_ByPoint(TrackedObjectData item, Vector2 point, int layerMask, bool checkForLayers)
        {
            if (checkForLayers)
            {
                //--- Step 1: Run the LayerMask test. ---
                if (!TestFor_Layer(item, layerMask))
                {
                    return false;
                }
            }

            //--- Step 2: Run the AABB test. ---
            if (!TestFor_AABB(item, point, point))
            {
                return false;
            }

            //--- Step 3: Run the PIP test. ---
            if (!TestFor_PIP(item, point))
            {
                return false;
            }

            //--- Finally: Return true (passed all tests). ---
            return true;
        }

        private bool Test_ByBox(TrackedObjectData item, Vector2 min, Vector2 max, int layerMask, bool checkForLayers)
        {
            if (checkForLayers)
            {
                //--- Step 1: Run the LayerMask test. ---
                if (!TestFor_Layer(item, layerMask))
                {
                    return false;
                }
            }

            //--- Step 2: Run the AABB test. ---
            if (!TestFor_AABB(item, min, max))
            {
                return false;
            }

            //--- Finally: Return true (passed all tests). ---
            return true;
        }

        private bool Test_ByCircle(TrackedObjectData item, Vector2 center, float sqrRadius, Vector2 aabbMin,
            Vector2 aabbMax, int layerMask, bool checkForLayers)
        {
            if (checkForLayers)
            {
                //--- Step 1: Run the LayerMask test. ---
                if (!TestFor_Layer(item, layerMask))
                {
                    return false;
                }
            }

            //--- Step 2: Run the AABB test. ---
            if (!TestFor_AABB(item, aabbMin, aabbMax))
            {
                return false;
            }

            //--- Step 3: Run the Distance test. ---
            if (!TestFor_Distance(item, center, sqrRadius))
            {
                return false;
            }

            //--- Finally: Return true (passed all tests). ---
            return true;
        }

        #endregion

        #region Tests

        private static bool TestFor_Layer(TrackedObjectData item, int layerMask)
        {
            int layerToTest = item.GameObject.layer;
            return layerMask.MaskIncludes(layerToTest);
        }

        private static bool TestFor_AABB(TrackedObjectData item, Vector2 min, Vector2 max)
        {
            Vector2 itemMin = item.AABB.Min;
            Vector2 itemMax = item.AABB.Max;
            return Geometry2D.IsOverlapping(itemMin, itemMax, min, max);
        }

        private static bool TestFor_PIP(TrackedObjectData item, Vector2 point)
        {
            IList<Vector2> hullPoints = item.WorldConvexHull;
            return point.IsInPoly(hullPoints);
        }

        private static bool TestFor_Distance(TrackedObjectData item, Vector2 center, float sqrRadius)
        {
            Vector2 testCenter = item.AABB.Center;
            return testCenter.TestDistanceMax(center, sqrRadius);
        }

        #endregion

        #region Pickers

        private TrackedObjectData Pick_ByPoint(List<TrackedObjectData> toPickFrom, Vector2 point, int layerMask)
        {
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
                //if layermask is 1, that means everything is included, so we don't need to check.

            int toPickFromCount = toPickFrom.Count;
            for (int i = 0; i < toPickFromCount; i++)
            {
                var item = toPickFrom[i];

                if (Test_ByPoint(item, point, layerMask, checkForLayers))
                {
                    return item;
                }
            }

            //--- Finally: Return null (nobody passed tests). ---
            return null;
        }

        private TrackedObjectData Pick_ByBox(List<TrackedObjectData> toPickFrom, Vector2 min, Vector2 max, int layerMask)
        {
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
                //if layermask is 1, that means everything is included, so we don't need to check.

            int toPickFromCount = toPickFrom.Count;
            for (int i = 0; i < toPickFromCount; i++)
            {
                var item = toPickFrom[i];

                if (Test_ByBox(item, min, max, layerMask, checkForLayers))
                {
                    return item;
                }
            }

            //--- Finally: Return null (nobody passed test). ---
            return null;
        }

        private TrackedObjectData Pick_ByCircle(List<TrackedObjectData> toPickFrom, Vector2 center, float radius,
            int layerMask)
        {
            bool checkForLayers = layerMask != Mathf2.LayerMaskAll;
                //if layermask is 1, that means everything is included, so we don't need to check.

            //--- Preparation: Determine max and min points of test area. ---
            //---              Calculate squared radius.                  ---
            Vector2 min = new Vector2(center.x - radius, center.y - radius);
            Vector2 max = new Vector2(center.x + radius, center.y + radius);
            float sqrRadius = radius*radius;

            int toPickFromCount = toPickFrom.Count;
            for (int i = 0; i < toPickFromCount; i++)
            {
                var item = toPickFrom[i];

                if (Test_ByCircle(item, center, sqrRadius, min, max, layerMask, checkForLayers))
                {
                    return item;
                }
            }

            //--- Finally: Return null (nobody passed tests). ---
            return null;
        }

        #endregion

        #region Updater

        void Update()
        {
            ObjectTrackerDatabase.UpdateAll();
        }

        #endregion

        #region Inner Methods

        private static GameObject GetGameObjectOrNull(TrackedObjectData data)
        {
            return data != null ? data.GameObject : null;
        }

        #endregion
    }
}
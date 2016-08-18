using System.Collections.Generic;
using UnityEngine;

namespace UnityCSCommon.Utils.ManualTracking2D
{
    /// <summary>
    /// Keeps records of all TrackedObjectDatas and their referances.
    /// </summary>
    public static class ObjectTrackerDatabase
    {
        #region Fields and Properties
        private static readonly List<TrackedObjectData> AllTrackedObjects = new List<TrackedObjectData>();
        private static readonly Dictionary<GameObject, TrackedObjectData> ObjectDataDictionary = new Dictionary<GameObject, TrackedObjectData>();
        private static readonly Dictionary<TrackedObjectData, GameObject> DataObjectDictionary = new Dictionary<TrackedObjectData, GameObject>();

        /// <summary>
        /// Gets all datas as read-only. DO NOT try to modify this by any meanings.
        /// </summary>
        public static List<TrackedObjectData> AllDatas
        {
            get { return AllTrackedObjects; }
        }
        #endregion

        #region Public Methods
        public static void Add(GameObject gameObject, TrackedObjectData data)
        {
            AllTrackedObjects.Add(data);
            ObjectDataDictionary.Add(gameObject, data);
            DataObjectDictionary.Add(data, gameObject);
        }

        public static void Remove(GameObject gameObject)
        {
            TrackedObjectData data = ObjectDataDictionary[gameObject];

            AllTrackedObjects.Remove(data);
            ObjectDataDictionary.Remove(gameObject);
            DataObjectDictionary.Remove(data);
        }

        public static void Remove(TrackedObjectData data)
        {
            GameObject gameObject = DataObjectDictionary[data];

            AllTrackedObjects.Remove(data);
            ObjectDataDictionary.Remove(gameObject);
            DataObjectDictionary.Remove(data);
        }

        public static void UpdateAll()
        {
            int count = AllTrackedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var data = AllTrackedObjects[i];

                if (data.GameObject.activeSelf)
                {
                    data.Update();
                }
            }
        }
        #endregion
    }
}
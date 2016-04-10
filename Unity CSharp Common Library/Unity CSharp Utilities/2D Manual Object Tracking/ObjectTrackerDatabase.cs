using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// Keeps records of all TrackedObjectDatas and their referances.
/// </summary>
public static class ObjectTrackerDatabase
{
    #region Fields and Properties
    private static List<TrackedObjectData> _allTrackedObjects = new List<TrackedObjectData>();
    private static Dictionary<GameObject, TrackedObjectData> _objectDataDictionary = new Dictionary<GameObject, TrackedObjectData>();
    private static Dictionary<TrackedObjectData, GameObject> _dataObjectDictionary = new Dictionary<TrackedObjectData, GameObject>();

    /// <summary>
    /// Gets all datas as read-only. DO NOT try to modify this by any meanings.
    /// </summary>
    public static List<TrackedObjectData> AllDatas
    {
        get
        {
            return _allTrackedObjects;
        }
    }
    #endregion

    #region Public Methods
    public static void Add(GameObject gameObject, TrackedObjectData data)
    {
        _allTrackedObjects.Add(data);
        _objectDataDictionary.Add(gameObject, data);
        _dataObjectDictionary.Add(data, gameObject);
    }

    public static void Remove(GameObject gameObject)
    {
        TrackedObjectData data = _objectDataDictionary[gameObject];

        _allTrackedObjects.Remove(data);
        _objectDataDictionary.Remove(gameObject);
        _dataObjectDictionary.Remove(data);
    }

    public static void Remove(TrackedObjectData data)
    {
        GameObject gameObject = _dataObjectDictionary[data];

        _allTrackedObjects.Remove(data);
        _objectDataDictionary.Remove(gameObject);
        _dataObjectDictionary.Remove(data);
    }

    public static void UpdateAll()
    {
        int count = _allTrackedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            var data = _allTrackedObjects[i];

            if (data.GameObject.activeSelf)
            {
                data.Update();
            }
        }
    }
    #endregion
}
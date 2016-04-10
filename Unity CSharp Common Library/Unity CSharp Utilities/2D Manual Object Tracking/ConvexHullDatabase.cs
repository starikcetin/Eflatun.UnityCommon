using UnityEngine;
using cSharpExpansions;

public class ConvexHullDatabase : Singleton<ConvexHullDatabase>
{
    protected ConvexHullDatabase() {}  //prevent init

    private OrderedDictionary<GameObject, Vector2[]> _localConvexHullDictionary = new OrderedDictionary<GameObject, Vector2[]>();

    /// <summary>
    /// Gets the local convex hull of the given prefab.
    /// </summary>
    public Vector2[] GetLocalConvexHull (GameObject prefab)
    {
        Vector2[] foundValue;
        bool isRecordedBefore = _localConvexHullDictionary.TryGetValue(prefab, out foundValue);

        if (isRecordedBefore)
        {
            return foundValue;
        }
        else
        {
            //get renderer
            var renderer = prefab.GetComponent<SpriteRenderer>();

            //get local vertices
            var localVertices = renderer.sprite.vertices;

            //calculate convex hull
            var localConvexHull = Geometry2D.MakeConvexHull(localVertices);

            //add to dictionary
            _localConvexHullDictionary.Add(prefab, localConvexHull);

            //return value
            return localConvexHull;
        }
    }
}

using UnityEngine;

namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Utilities and extension methods for <see cref="GameObject"/>s.
    /// </summary>
    public static class GameObjectUtilities
    {
        /// <summary>
        /// Sets the active state of this <paramref name="gameObject"/> and it's first level parent.
        /// </summary>
        public static void SetActiveWithParent (this GameObject gameObject, bool value)
        {
            gameObject.SetActive(value);
            gameObject.transform.parent.gameObject.SetActive(value);
        }

        /// <summary>
        /// Sets the active state of this <paramref name="gameObject"/> and it's first level children.
        /// </summary>
        public static void SetActiveWithChildren(this GameObject gameObject, bool value)
        {
            gameObject.SetActive(value);
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Sets the active state of this <paramref name="gameObject"/> and all of ancestors (parent, grandparent, parent of grandparent... etc).
        /// </summary>
        /// <remarks>
        /// This method loops through the hierarchy, thus eliminating recursive calls.
        /// </remarks>
        public static void SetActiveWithAncestors (this GameObject gameObject, bool value)
        {
            Transform t = gameObject.transform;
            while (t != null)
            {
                t.gameObject.SetActive (value);
                t = t.parent;
            }
        }

        /// <summary>
        /// Sets the active state of this <paramref name="gameObject"/> and all of it's children hierarchy (children, grandchildren, children of grandchildren... etc).
        /// <para>This method uses recursive calls, so be careful about *very* big hierarchies.</para>
        /// </summary>
        /// <remarks>
        /// This method tries to reduce the amount of recursive calls, by eliminating them for single-child hierarchies.
        /// But it is still dangerous for *very* big hierarchies.
        /// Test Result: 7-9 total calls for 20-30 gameObject (no single child hierarchy).
        /// </remarks>
        public static void SetActiveWithDescendants (this GameObject gameObject, bool value)
        {
            Transform level1 = SetActiveRTSCH (gameObject.transform, value);
            if (level1.childCount == 0) return;

            for (int i = 0; i < level1.childCount; i++)
            {
                Transform level2 = SetActiveRTSCH (level1.GetChild (i), value);
                if (level2.childCount == 0) continue;

                for (int j = 0; j < level2.childCount; j++)
                {
                    Transform level3 = SetActiveRTSCH (level2.GetChild (j), value);
                    if (level3.childCount == 0) continue;

                    foreach (Transform level4 in level3)
                    {
                        switch (level4.childCount)
                        {
                            case 0:
                                level4.gameObject.SetActive (value);
                                break;

                            case 1:
                                level4.gameObject.SetActive (value);
                                level4.GetChild (0).gameObject.SetActiveWithDescendants (value);
                                break;

                            default:
                                level3.gameObject.SetActiveWithDescendants (value);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// <para>SetActive Recursively Through Single Child Hierarchy</para>
        /// <para>'SetActive's transform; if transform has only one child, switches to child; repeats the process.
        /// Continues until switching a transform with no child or more than one child.
        /// Returns the transform it stopped.</para>
        /// </summary>
        private static Transform SetActiveRTSCH (Transform beginWith, bool value)
        {
            Transform t = beginWith;
            t.gameObject.SetActive(value);

            while (t.childCount == 1)
            {
                t = t.GetChild(0);
                t.gameObject.SetActive(value);
            }

            return t;
        }
    }
}
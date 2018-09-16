using UnityEngine;

namespace Eflatun.UnityCommon.Utils.Pooling
{
    /// <summary>
    /// Inspector representative for PrefabPool class.
    /// </summary>
    [System.Serializable]
    public sealed class PrefabPool_Inspector
    {
        public GameObject Prefab;
        public int PrePopulateAmount;
        public int AutoPopulateAmount;
    }
}

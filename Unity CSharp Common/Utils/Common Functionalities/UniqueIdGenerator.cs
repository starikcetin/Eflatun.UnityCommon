namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Generates a unique integer every time <see cref="Next"/> method is called unless application quits or <see cref="Reset"/> method is called.
    /// </summary>
    public static class UniqueIdGenerator
    {
        private static int _nextId;

        public static void Reset()
        {
            _nextId = 0;
        }

        public static int Next()
        {
            return _nextId++; //takes value first, then increments.
        }
    }
}
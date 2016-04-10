using UnityEngine;

/// <summary> 
/// A class including optimized debug functions. 
/// </summary>
public static class DebugOpt
{
    /// <summary> 
    /// A wrapper around Debug.Log method with a UNITY_EDITOR conditional attribute. 
    /// </summary>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogCon(string message)
    {
        Debug.Log(message);
    }
}
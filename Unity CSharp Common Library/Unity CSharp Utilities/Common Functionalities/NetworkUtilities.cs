using System.Net;

/// <summary>
/// Utilities for Networking.
/// </summary>
public static class NetworkUtilities
{
    /// <summary>
    /// Tries to open a web client to http://www.example.com; if no errors or exceptions arises, returns true.
    /// </summary>
    public static bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead("http://www.example.com"))
                {
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }
}
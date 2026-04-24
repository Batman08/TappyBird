using UnityEngine;

public static class Log
{
    public static bool EnableLogs = true;

    public static void Info(string msg)
    {
        if (EnableLogs) Debug.Log(msg);
    }

    public static void Warning(string msg)
    {
        if (EnableLogs) Debug.LogWarning(msg);
    }

    public static void Error(string msg)
    {
        Debug.LogError(msg);
    }
}
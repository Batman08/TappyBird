using UnityEngine;

public static class Log
{
#if UNITY_EDITOR
    public static bool EnableLogs = true;
#elif UNITY_ANDROID
    public static bool EnableLogs = false;
#endif


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
using UnityEngine;

public static class Log
{
    public static void Warning(object ms, string group = "main")
    {
        Debug.LogWarning($"[{group}]: {ms}");
    }
    public static void Ms(object ms, string group = "main")
    {
        Debug.Log($"[{group}]: {ms}");
    }
    public static void Error(object ms, string group = "main")
    {
        Debug.LogError($"[{group}]: {ms}");
    }
    public static T log<T>(this T obj)
    {
        Log.Ms(obj, "fs_logger");
        return (T)obj;
    }
}

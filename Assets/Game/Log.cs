using System.Collections;
using System.Collections.Generic;
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
}

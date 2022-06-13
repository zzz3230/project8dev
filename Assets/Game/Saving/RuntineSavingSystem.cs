using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct SaveAccessDataSet
{
    public ulong uid;
    public string key;
}

public static class RuntimeSavingSystem
{
    static Dictionary<SaveAccessDataSet, object> data = new Dictionary<SaveAccessDataSet, object>();

    public static void SaveObject(SaveAccessDataSet key, object value)
    {
        data[key] = value;
    }

    public static object LoadObject(SaveAccessDataSet key)
    {
        return data[key];
    }
    public static bool HasKey(SaveAccessDataSet key)
    {
        return (data.ContainsKey(key));
    }
}

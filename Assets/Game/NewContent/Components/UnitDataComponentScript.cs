using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataComponentScript : BaseSaveableComponentScript
{
    public bool HasKey(string key)
    {
        return HasSaveWithKey(key);
    }
    public void Set(string key, object value)
    {
        SaveObject(key, value);
    }
    public T Get<T>(string key)
    {
        return LoadObject<T>(key);
    }
}

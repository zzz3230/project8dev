using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SavingDataComponentScript))]
public class BaseSaveableComponentScript : BaseUnitComponentScript
{
    SavingDataComponentScript savingComponent;

    public override void GameAwake()
    {
        savingComponent = GetComponent<SavingDataComponentScript>();
    }

    protected void SaveObjects<T>(string key, T[] value)
    {
        savingComponent.SaveObjects(uid, key, value.Cast<object>().ToArray());
    }
    protected T[] LoadObjects<T>(string key)
    {
        return savingComponent.LoadObjects(uid,key).Cast<T>().ToArray();
    }
    protected bool HasSaveWithKey(string key)
    {
        return savingComponent.HasKey(uid, key);
    }

    protected void SaveObject(string key, object value)
    {
        savingComponent.SaveObjects(uid, key, new object[] { value });
    }
    protected T LoadObject<T>(string key)
    {
        return (T)savingComponent.LoadObjects(uid, key)[0];
    }

}

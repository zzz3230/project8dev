using UnityEngine;

public class BaseSyncArticle : MonoBehaviour
{
    public enum SyncType
    {
        ToAll = 1,
        ToOwner = 2,
        NotSync = 4
    }
    public SyncType syncType;
    protected object lastSendedValue;

    public virtual bool S_ValueUpdated() { return false; }
    public virtual object S_GetValue() { return null; }
    public virtual void C_SetValue(object value) { }
}

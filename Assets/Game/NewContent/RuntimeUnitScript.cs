using UnityEngine;
using System.Linq;

public class RuntimeUnitScript : MonoBehaviour
{
    public RuntimeUnitInfoScript info;

    void Start()
    {
        GameStart();
    }
    void Update()
    {
        GameUpdate();
    }
    private void FixedUpdate()
    {
        GameFixedUpdate();
    }

    public virtual void GameUpdate() { }
    public virtual void GameStart() { }
    public virtual void GameFixedUpdate() { }

    public ulong GetComponentUid(BaseUnitComponentScript comp)
    {
        var uids = info.CastMetadata<UnitMetadata>().componentsUids;
        for (int i = 0; i < uids.Count; i++)
        {
            if(uids[i].className == comp.GetType().Name && uids[i].engineId == comp.engineId)
            {
                return uids[i].uid;
            }
        }    
        var uid = new ComponentUidInfo();
        uid.uid = Utils.NextUlong();
        uid.className = comp.GetType().Name;
        uid.engineId = comp.engineId;

        info.CastMetadata<UnitMetadata>().componentsUids.Add(uid);

        return uid.uid;
    }
}

using UnityEngine;

public class BaseUnitComponentScript : MonoBehaviour
{
    public string uuid;
    public ulong uid;
    public string engineId = "";

    public BaseUnitComponentScript Init(RuntimeUnitScript unitScript)
    {
        uid = unitScript.GetComponentUid(this);
        return this;
    }

    public BaseUnitComponentScript Init(string engineId, RuntimeUnitScript unitScript) 
    {
        this.engineId = engineId;
        Init(unitScript);
        return this;
    }
    public BaseUnitComponentScript Init()
    {
        return this;
    }

    private void Start()
    {
        //uuid = System.Guid.NewGuid().ToString();
        //uid = 777000777;// Utils.NextUlong();
        if (engineId == string.Empty)
            Log.Warning("Component dont initialized");

        GameStart();
    }
    private void Update()
    {
        if (engineId == string.Empty)
            Log.Warning("Component dont initialized");

        GameUpdate();
    }
    private void Awake()
    {
        GameAwake();
    }
    public virtual void GameAwake() { }
    public virtual void GameStart() { }
    public virtual void GameUpdate() { }
}

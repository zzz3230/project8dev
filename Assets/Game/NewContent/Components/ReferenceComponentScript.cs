using UnityEngine;

public class ReferenceComponentScript : BaseUnitComponentScript
{
    [SerializeField]
    NewBasePlayerScript playerScript;
    RuntimeUnitInfoScript infoScript;
    public override void GameAwake()
    {
        infoScript = GetComponent<RuntimeUnitInfoScript>();
    }
    public void Init(NewBasePlayerScript _playerScript)
    {
        playerScript = _playerScript;
    }

    public NewBasePlayerScript GetPlayerScript()
    {
        return infoScript.playerScript;
    }
}

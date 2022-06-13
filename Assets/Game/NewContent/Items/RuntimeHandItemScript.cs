using UnityEngine;

[RequireComponent(typeof(RuntimeUnitInfoScript))]
public class RuntimeHandItemScript : RuntimeUnitScript
{
    protected void DurabilityUpdated()
    {
        info.playerScript.UpdateItemDurabilityInHUD(true);
    }
}

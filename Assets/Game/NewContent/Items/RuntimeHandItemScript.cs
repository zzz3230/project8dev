using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeHandItemScript : RuntimeUnitScript
{
    protected void DurabilityUpdated()
    {
        info.playerScript.UpdateItemDurabilityInHUD(true);
    }
}

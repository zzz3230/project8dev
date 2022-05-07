using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractiveHandItemScript : RuntimeHandItemScript
{
    [SerializeField] Light _light;
    public override void GameUpdate()
    {
        var md = info.CastMetadata<UnitMetadata>();
        if (md.durability <= 0)
        {
            _light.enabled = false;
        }
        else
        {
            _light.enabled = true;
            md.durability -= Time.deltaTime * 5;
            DurabilityUpdated();
        }
    }
}

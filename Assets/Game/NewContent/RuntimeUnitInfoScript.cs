using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeUnitInfoScript : MonoBehaviour
{
    public GameObject root;
    public ItemInstance itemInstance;
    public NewBasePlayerScript playerScript;

    public T CastMetadata<T>() where T : UnitMetadata
    {
        return (T)itemInstance.metadata;
    }

    internal void Setup(ItemInstance item, NewBasePlayerScript player)
    {
        itemInstance = item;
        playerScript = player;
        root = gameObject;
    }
}

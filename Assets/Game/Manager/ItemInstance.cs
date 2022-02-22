using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public enum SIID
{
    game_testitem_1
}

public static class StaticItemManager
{

}


public class ItemMetadata
{
    public float durability = 100;
    public float maxDurability = 100;
    public string uuid = "00000000-0000-0000-0000-000000000000";
}

[System.Serializable]
public class ItemInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public SIID id;
    public TEXT name;
    [JsonConverter(typeof(ResourceJsonConverter))]
    public Texture2D ico; 
    [JsonConverter(typeof(StringEnumConverter))]
    public BuilderID builderId; 
    [JsonConverter(typeof(PrefabJsonConverter))]
    public Prefab handObject; 
    [JsonConverter(typeof(PrefabJsonConverter))]
    public Prefab discardedObject;
}

public class ItemInstance
{
    public bool empty = false;
    public ItemMetadata metadata;
    public ItemInfo info;
    public int count;

    public static readonly ItemInstance Empty = new ItemInstance { empty = true };
}

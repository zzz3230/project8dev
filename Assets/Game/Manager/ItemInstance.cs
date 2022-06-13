using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using UnityEngine;



public static class StaticItemManager
{

}

[System.Serializable]
public struct ComponentUidInfo
{
    public string className;
    public string engineId;
    public ulong uid;
}

[System.Serializable]
public class UnitMetadata
{
    public float durability = 100;
    public float maxDurability = 100;
    public string uuid = "00000000-0000-0000-0000-000000000000";

    public List<ComponentUidInfo> componentsUids = new();

    public override string ToString()
    {
        return $"mtdat[dur={durability} maxDur={maxDurability} uuid='{uuid[..8]}...']";
    }

    public static bool operator ==(UnitMetadata a, UnitMetadata b)
    {
        //Log.Ms(a + "  ==  " + b);
        return (a is not null) && (b is not null) &&
            a.durability == b.durability &&
            a.maxDurability == b.maxDurability &&
            a.uuid == b.uuid;
    }
    public static bool operator !=(UnitMetadata a, UnitMetadata b)
    {
        if (b == null)
            throw new System.Exception("USE is not null");
        return !(a == b);//|| (a is null && b is not null) || (b is null && a is not null);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

[System.Serializable]
public class ItemInfo
{
    public ItemInfo()
    {
        //strId = id.ToString();
    }

    public SIID id;
    public string strId = "~SIID";
    public string[] tags;

    public bool stackable { get => stack > 1; }
    public int stack;

    public bool CompareType(ItemInfo other)
    {
        if (other == null)
            return false;

        return id == other.id;
    }
    public override string ToString()
    {
        var strTags = "[" + string.Join(", ", tags) + "]";
        return
            $"id={id}\n" +
            $"strId={strId}\n" +
            $"tags={strTags}\n" +
            $"stack={stack}\n" +
            $"name={name}\n" +
            $"builderId={builderId}\n" +
            $"discardedObjInfo={discardedObjectInfo.type}\n" +
            $"handItemIner={handItemInfo.interactive}\n" +
            $"placeable={buildingInfo.placeable}\n";
    }

    [JsonConverter(typeof(StringEnumConverter))]



    public TEXT name;
    [JsonConverter(typeof(ResourceJsonConverter))]
    public Texture2D ico;
    [JsonConverter(typeof(StringEnumConverter))]
    public BuilderID builderId;
    //[JsonConverter(typeof(PrefabJsonConverter))]
    [JsonConverter(typeof(PrefabJsonConverter))]
    //public Prefab discardedObject;
    public DiscardedObjectInfo discardedObjectInfo;
    public HandItemInfo handItemInfo;
    public PlaceBuildingInfo buildingInfo;

}
public enum DiscardedObjectType
{
    HandObject, InactiveHandObject, Prefab, Crate, Virtual
}
[System.Serializable]
public class DiscardedObjectInfo
{
    public DiscardedObjectType type;
    public Prefab prefab;
}

[System.Serializable]
public class HandItemInfo
{
    public Prefab interactiveHandObject;
    public Prefab staticHandObject;
    public float damageFactor;
    public DamageType damageType;
    public bool interactive;
}
[System.Serializable]
public class PlaceBuildingInfo
{
    public bool placeable;
    public Prefab placingPrefab;
    public Prefab previewPrefab;
}
[System.Serializable]
public class ItemInstance
{
    public bool empty = false;
    public UnitMetadata metadata;
    public ItemInfo info;
    public int count;

    public static ItemInstance Empty { get => new ItemInstance { empty = true }; } // empty item instance;

    internal bool CompareMetadata(ItemInstance other)
    {
        return metadata == other.metadata;
    }

    public override string ToString()
    {
        var nl = "null";
        return $"item[siid='{(info == null ? nl : info.id)}' count={count} metadata={(metadata == null ? nl : metadata)}]";
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabInfo
{
    public string uuid;
    public string name;
    [JsonConverter(typeof(PrefabJsonConverter))]
    public Prefab prefab;
}

public class PrefabJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((Prefab)value).uuid);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (objectType == typeof(Prefab))
            return PrefabManager.Instance.GetPrefabByUUID(((string)existingValue));
        throw new Exception("json reading error: not a prefab");
    }

    public override bool CanRead
    {
        get { return true; }
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Prefab);
    }
}

public class PrefabManager : MonoBehaviour
{
    public Prefab selectedPrefab;
    public List<PrefabInfo> prefabs = new List<PrefabInfo>();

    private void Awake()
    {
        Global.prefabManager = this;
        DontDestroyOnLoad(this);
    }

    public Prefab GetPrefabByUUID(string uuid)
        => prefabs.Find(x => x.uuid == uuid).prefab;

    public static PrefabManager Instance { get { return Global.prefabManager; } }
}

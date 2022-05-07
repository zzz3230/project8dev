using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System;


[System.Serializable]
public class ResourceInfo
{
    public string name;
    [JsonConverter(typeof(ResourceJsonConverter))]
    public UnityEngine.Object res;
    public string uuid;
}

public class ResourceJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((UnityEngine.Object)value).GetUUID());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (objectType == typeof(Prefab))
            return GameResourcesManager.Instance.GetResourceByUUID<UnityEngine.Object>(((string)existingValue));
        throw new Exception("json reading error: not a res");
    }

    public override bool CanRead
    {
        get { return true; }
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(UnityEngine.Object);
    }
}

[ExecuteAlways]
public class GameResourcesManager : MonoBehaviour
{
    public UnityEngine.Object selectedRes;
    public List<ResourceInfo> resources = new List<ResourceInfo>();

    private void Awake()
    {
        //print("Crated");
        Global.resourcesManager = this;
        //print("added");
    }
    private void Start()
    {
        if(Application.isPlaying)
            DontDestroyOnLoad(this);
    }

    public T GetResourceByUUID<T>(string uuid) where T : UnityEngine.Object
    {
        return (T)resources.Find(x => x.uuid == uuid).res;
    }

    public (string uuid, bool founded) GetUUIDOfResource(UnityEngine.Object obj, bool assert = true)
    {
        var foundedRes = resources.FirstOrDefault((x) => x.res == obj);

        if (foundedRes == null && assert)
            throw new System.Exception($"Resource '{obj.name}' not found, type '{obj.GetType().Name}'");

        if (!assert && foundedRes == null)
            return (System.Guid.Empty.ToString(), false);

        return (foundedRes.uuid, foundedRes != null);
    }

    public static GameResourcesManager Instance { get { return Global.resourcesManager; } }
}

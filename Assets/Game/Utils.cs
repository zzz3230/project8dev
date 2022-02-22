#if UNITY_EDITOR
#define debug
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System;
using UnityEditor;
using OldNetwork;

public static class RaycastHitX
{
    public static RaycastHit Cast(Vector3 origin, Vector3 direction, LayerMask layerMask, float maxDistance = 10f, bool debug = false)
    {
        var ray = new Ray(origin, direction);
        var hit = new RaycastHit();

        Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance, layerMask);

        if (debug)
        {
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
        }

        return hit;
    }
}

public static class Vector3X
{
    public static Vector3 IgnoreY(Vector3 value, float y = 0)
    {
        return new Vector3(value.x, y, value.z);
    }

    public static Vector3 IgnoreX(Vector3 value, float x = 0)
    {
        return new Vector3(x, value.y, value.z);
    }

    public static Vector3 IgnoreXZ(Vector3 value)
    {
        return new Vector3(0f, value.y, 0f);
    }
}


[Serializable]
public struct SerVector3
{
    public float x, y, z;
    public SerVector3(Vector3 orig)
    {
        x = orig.x;
        y = orig.y;
        z = orig.z;
    }
    public SerVector3(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
}

public class ReadOnlyAttribute : PropertyAttribute { }
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif

public enum DamageType
{
    Soft, Hard, Impulse, True
}
[System.Serializable]
public struct DamageFactor
{
    public DamageFactor(DamageType type, float factor)
    {
        this.type = type;
        this.factor = factor;
    }

//#if UNITY_EDITOR
    [ReadOnly]
//#endif
    public DamageType type;
    public float factor;
}

[Flags]
public enum Layers
{
    BuildingPreview = 1
}

public static class Utils
{
    public static T DeepCopy<T>(T other)
    {
        using MemoryStream ms = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(ms, other);
        ms.Position = 0;
        return (T)formatter.Deserialize(ms);
    }

    public static string GetUUID(this UnityEngine.Object obj, bool allowEmpty = false)
    {
        var info = GameResourcesManager.Instance.GetUUIDOfResource(obj, !allowEmpty);

        return info.founded ? info.uuid : Guid.Empty.ToString();
    }

    public static Prefab GetPrefab(this GameObject obj)
    {
        return obj.GetComponent<Prefab>();
    }
    public static string GetUUID(this GameObject obj)
    {
        return obj.GetComponent<Prefab>().uuid;
    }


    public const string LOG_PATH = "D:\\unity_project\\Project8\\LOG";

    public static void Assert(bool val, string msg = "")
    {
        if(!val)
            throw new Exception("[assertion failed] " + msg);
    }

    public static void SetKeyValue<T0, T1>(this Dictionary<T0, T1> d, T0 key, T1 value)
    {
        if (d.ContainsKey(key))
        {
            d[key] = value;
        }
        else
        {
            d.Add(key, value);
        }
    }

    public static void AddValueToListByKey<T0, T1>(this Dictionary<T0, List<T1>> d, T0 key, T1 value)
    {
        if (d.ContainsKey(key))
        {
            if (d[key] == null)
                d[key] = new List<T1> { value };
            else
                d[key].Add(value);
        }
        else
            d.Add(key, new List<T1> { value });
    }

    public static SerVector3 GetSerVector3(this Vector3 v)
    {
        return new SerVector3(v);
    }

    public static /*void*/int GetLayerMask(Layers layer)
    {
        
        List<string> names = new List<string> { }; 
        var layers = Enum.GetValues(typeof(Layers));
        foreach (Layers l in layers)
        {
            if ((layer & l) == l)
                names.Add(l.ToString());
        }

        return LayerMask.GetMask(names.ToArray());

    }

    public static int GetLayer(Layers layer)
    {
        return LayerMask.NameToLayer(GetLayerName(layer));
    }

    public static string GetLayerName(Layers layer)
    {
        return layer.ToString();
    }

    public static void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }

    static System.Random _random = new System.Random();

    public static T SpawnBuilder<T>(T builder) where T : BaseBuilder
    {
        return GameObject.Instantiate<T>(builder);
    }

    public static T SpawnWidget<T>(T widget) where T : WidgetBehaviour
    {
        return GameObject.Instantiate<T>(widget);
    }

    public static SlotWidgetScript SpawnSlotWidget(SlotWidgetScript original, GameObject parent, int index = -1)
    {
        var slot = SpawnWidget(original);
        slot.transform.SetParent(parent.transform);
        slot.transform.localScale = Vector3.one;
        slot.index = index;
        return slot;
    }

    /// <summary>
    /// Only for server
    /// </summary>
    /// <param name="prefab"></param>
    public static NetworkMonoBehaviour Spawn(NetworkPrefab prefab, ClientId owner = null)
    {
        
        ThrowErrorIf(NetworkManager.NetworkState.Client);
        var nmb = GameObject.Instantiate(prefab.gameObject).GetComponent<NetworkMonoBehaviour>();
        nmb.owner = owner;

        nmb.InitOnServer();
        return nmb;
    }
    static HashSet<ulong> _reged = new HashSet<ulong> { 0 };
    public static ulong RandomUlong()
    {
        ulong val;
        do
        {
            byte[] bytes = new byte[8];
            _random.NextBytes(bytes);
            val = BitConverter.ToUInt64(bytes, 0);
        }
        while (_reged.Contains(val));
        return val;
    }

    public static byte [] StructureToByteArray(object obj)
    {
        int len = Marshal.SizeOf(obj);

        byte [] arr = new byte[len];

        IntPtr ptr = Marshal.AllocHGlobal(len);

        Marshal.StructureToPtr(obj, ptr, true);

        Marshal.Copy(ptr, arr, 0, len);

        Marshal.FreeHGlobal(ptr);

        return arr;
    }

    public static void ByteArrayToStructure(byte [] bytearray, ref object obj)
    {
        int len = Marshal.SizeOf(obj);

        IntPtr i = Marshal.AllocHGlobal(len);

        Marshal.Copy(bytearray, 0, i, len);

        obj = Marshal.PtrToStructure(i, obj.GetType());

        Marshal.FreeHGlobal(i);
    }
    public static byte[] ObjectToByteArray(object source)
    {
        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, source);
            return stream.ToArray();
        }
    }
    public static object ByteArrayToObject(byte[] data)
    {
        var formatter = new BinaryFormatter();
        using (var stream = new MemoryStream(data))
        {
            return formatter.Deserialize(stream);
        }
    }

    public static void ThrowErrorIf(NetworkManager.NetworkState side, string m = ""){
        if(Global.networkManager.state == side){
            var ex = $"Bad side: {side}" +(m != "" ?  " | " + m : "");
            LogError(ex);
            throw new Exception(ex);
            //return true;
            }
        //return false;
    }
    public static void ThrowDoIt(string m = "")
    {
        throw new Exception("Do It " + m);
    }
    public static void LoadAsset()
    {
        //Global.networkService.CallOnServer(nameof(Log), "Hello");
    }
    public enum LogType{
        Console, Screen, File
    }
    public static void LogSide(NetworkMonoBehaviour sender, LogType logType = LogType.Screen){
        var m = (sender.isServer ? 'S' : 'C');
        switch (logType){
            case LogType.Console:
                Debug.Log(m);
                break;
        }
    }
    public static void LogError(string m){
        Log(m, color: Color.red);
    }

    public static void SetMaterial(GameObject obj, Material mat)
    {
        var renderers = obj.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
        {
            r.material = mat;
        }
    }

    public static void DisableAllScripts(GameObject obj, List<Type> exclude = null)
    {
        _AllScripts(obj, (s) => s.enabled = false, exclude);
    }
    public static void RemoveAllScripts(GameObject obj, List<Type> exclude = null)
    {
        _AllScripts(obj, (s) => GameObject.Destroy(s), exclude);
    }

    static void _AllScripts(GameObject obj, Action<MonoBehaviour> action, List<Type> exclude = null)
    {
        var scripts = obj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (!(exclude != null && exclude.Contains(script.GetType())))
                action.Invoke(script);
        }
    }

    public static string ArrToStr<T>(T[] arr)
    {
        var res = "";
        foreach (var i in arr)
        {
            res += i.ToString() + "|";
        }
        return res;
    }

    public static void log(object message, bool active = true)
    {
        if(active)
            Debug.Log(message);
    }

    public static void Log(int message, float duration = 3f, Color color = default)
    {
        Log(message.ToString(), duration, color);
    }
    public static void Log(bool message, float duration = 3f, Color color = default)
    {
        Log(message.ToString(), duration, color);
    }
    public static void Log(string message, float duration = 3f, Color color = default)
    {
        DebugPlus.LogOnScreen(message).Duration(duration).Color(color == default(Color) ? Color.cyan : color);
    }
}

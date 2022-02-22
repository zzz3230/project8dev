using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BuildingConnectionType { Anchor, Free}
public struct BuildingConnection
{
    public BaseBuildingPrefabClass baseObj;
    public BaseBuildingPrefabClass connectedObj;

    public BuildingConnectionType connectionType;
    public Vector3 localPos;
    public int? anchorId;
}

public class BasicBuildingGroup : BaseBuildingGroup
{
    static GameObject _enviroment;
    public static BasicBuildingGroup Create()
    {
        Utils.log(nameof(BasicBuildingGroup) + " :_Created");
        if (_enviroment == null)
            _enviroment = new GameObject("enviroment");
        //return default;
        GameObject gameObject = new GameObject(nameof(BasicBuildingGroup) + "_GameObject", 
            typeof(BasicBuildingGroup),
            typeof(Rigidbody));

        gameObject.transform.SetParent(_enviroment.transform);
        return gameObject.GetComponent<BasicBuildingGroup>();
    }

    List<BaseBuildingPrefabClass> _objects = new List<BaseBuildingPrefabClass> { };

    Dictionary<BaseBuildingPrefabClass, List<BaseBuildingPrefabClass>> _objectsLinks = 
        new Dictionary<BaseBuildingPrefabClass, List<BaseBuildingPrefabClass>> { };

    List<BuildingConnection> _objectsConnections = 
        new List<BuildingConnection> { };

    public void Add(BaseBuildingPrefabClass obj, BaseBuildingPrefabClass by, BuildingConnection connectionInfo)
    {
        obj.gameObject.transform.SetParent(this.transform);

        _objects.Add(obj);
        _objectsLinks.Add(obj, new List<BaseBuildingPrefabClass> { });
        if(by != null)
        {
            //_objectsLinks[by].Add(obj);
            _objectsLinks.AddValueToListByKey(by, obj);
        }

        connectionInfo.baseObj = by;
        connectionInfo.connectedObj = obj;

        _objectsConnections.Add(connectionInfo);
        obj.group = this;
    }
    List<BaseBuildingPrefabClass> GetAllLinked(BaseBuildingPrefabClass obj)
    {
        List<BaseBuildingPrefabClass> result = new List<BaseBuildingPrefabClass> { };

        List<BaseBuildingPrefabClass> current = new List<BaseBuildingPrefabClass> { obj };
        while (true)
        {
            int linksSum = 0;
            current.ForEach((b) => { linksSum += _objectsLinks[b].Count; });
            if (linksSum > 0)
            {
                var buffer = new List<BaseBuildingPrefabClass> { };
                foreach (var c in current)
                {
                    buffer.AddRange(_objectsLinks[c]);
                }
                current = buffer;
                result.AddRange(buffer);
            }
            else
                break;
        }
        return result;
    }

    public void Remove(BaseBuildingPrefabClass obj) 
    {
        obj.gameObject.transform.SetParent(this.transform.parent);

        _objects.Remove(obj);

        var fobjs = GetAllLinked(obj);
        var sobjs = _objects.Except(fobjs);

        var sgr = Create();
        sgr._objects = sobjs.ToList();
        sgr._objectsLinks = 
            _objectsLinks.Where((x) => sgr._objects.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        for (int i = 0; i < sgr._objectsLinks.Count; i++)
        {
            var index = sgr._objectsLinks[sgr._objects[i]].IndexOf(obj);
            if (index != -1)
                sgr._objectsLinks[sgr._objects[i]].RemoveAt(index);
        }
        sgr.UpdateTransforms();


        _objects = fobjs;
        _objectsLinks = 
            _objectsLinks.Where((x) => _objects.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        UpdateTransforms();

        obj.group = null;
    }

    void UpdateTransforms()
    {
        _objects.ForEach((x) => x.gameObject.transform.SetParent(this.transform));
    }

    public BaseBuildingPrefabClass Debug_GetByIndex(int id)
    {
        return _objects[id];
    }
    public void Debug_RemoveByIndex(int id)
    {
        Remove(_objects[id]);
    }

    public void Debug_PrintAllLinked(BaseBuildingPrefabClass obj)
    {
        var x = GetAllLinked(obj);
        var str = "";
        x.ForEach((b) => { str += b.name + ";"; });
        Debug.Log(str);
    }

    public void Break() 
    {
        _objects.ForEach((x) => x.gameObject.transform.SetParent(this.transform.parent));
    }
}
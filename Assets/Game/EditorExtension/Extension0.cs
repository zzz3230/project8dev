using OldNetwork;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(NetworkPrefabsManager))]
public class PrefabsIndexator : Editor
{
    public override void OnInspectorGUI()
    {
        NetworkPrefabsManager myTarget = (NetworkPrefabsManager)target;

        if (GUILayout.Button("Reindex all prefab"))
        {
            myTarget.prefabs = new List<NetworkPrefab> { };

            var paths = AssetDatabase.GetAllAssetPaths();
            Debug.Log(Utils.ArrToStr(paths));

            for (int i = 0; i < paths.Length; i++)
            {
                var assets = AssetDatabase.LoadAllAssetsAtPath(paths[i]);
                for (int j = 0; j < assets.Length; j++)
                {
                    var obj = assets[j] as GameObject;
                    if (obj != null)
                    {
                        if (obj.TryGetComponent<NetworkPrefab>(out var mp))
                        {
                            myTarget.prefabs.Add(mp);
                        }
                    }
                }
            }

        }
        if (GUILayout.Button("Set index s"))
        {
            myTarget.prefabs.ForEach((p) => p.index = myTarget.GetPrefabIndex(p));
        };

        EditorGUILayout.IntField("Experience", 0);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}



[CustomEditor(typeof(BasicBuildingAnchorManager))]
public class BuildingAnchorManagerEditor : Editor
{
    bool extEdit = false;
    public override void OnInspectorGUI()
    {
        //SerializedObject serObj = new SerializedObject(target);

        BasicBuildingAnchorManager myTarget = (BasicBuildingAnchorManager)target;
        //var ancs = serObj.FindProperty(nameof(myTarget.anchors));
        //var ancsActive = serObj.FindProperty(nameof(myTarget.anchorsActive));

        extEdit = EditorGUILayout.Toggle("extEdit", extEdit);

        GUI.enabled = extEdit;
        if (GUILayout.Button("Spawn"))
        {
            var _enumValues = System.Enum.GetValues(typeof(BuildingAnchorDirection));
            for (int i = 0; i < _enumValues.Length; i++)
            {
                var ev = (BuildingAnchorDirection)_enumValues.GetValue(i);
                var gm = new GameObject(ev.ToString());
                gm.transform.SetParent(myTarget.transform);
            }
        }

        if (GUILayout.Button("Auto generate"))
        {
            var boxCol = myTarget.gameObject.GetComponentInChildren<BoxCollider>();

            var posFDL = new Vector3(boxCol.center.x + boxCol.size.x / 2, boxCol.center.y - boxCol.size.y / 2, boxCol.center.z - boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.FDownLeft).position = posFDL;

            var posBDR = new Vector3(boxCol.center.x - boxCol.size.x / 2, boxCol.center.y - boxCol.size.y / 2, boxCol.center.z + boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.BDownRight).position = posBDR;

            var posFDR = new Vector3(boxCol.center.x + boxCol.size.x / 2, boxCol.center.y - boxCol.size.y / 2, boxCol.center.z + boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.FDownRight).position = posFDR;

            var posBDL = new Vector3(boxCol.center.x - boxCol.size.x / 2, boxCol.center.y - boxCol.size.y / 2, boxCol.center.z - boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.BDownLeft).position = posBDL;

            var posFUL = new Vector3(boxCol.center.x + boxCol.size.x / 2, boxCol.center.y + boxCol.size.y / 2, boxCol.center.z - boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.FUpLeft).position = posFUL;

            var posBUR = new Vector3(boxCol.center.x - boxCol.size.x / 2, boxCol.center.y + boxCol.size.y / 2, boxCol.center.z + boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.BUpRight).position = posBUR;

            var posFUR = new Vector3(boxCol.center.x + boxCol.size.x / 2, boxCol.center.y + boxCol.size.y / 2, boxCol.center.z + boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.FUpRight).position = posFUR;

            var posBUL = new Vector3(boxCol.center.x - boxCol.size.x / 2, boxCol.center.y + boxCol.size.y / 2, boxCol.center.z - boxCol.size.z / 2);
            myTarget.GetAnchor(BuildingAnchorDirection.BUpLeft).position = posBUL;


        }
        GUI.enabled = true;

        var enumValues = System.Enum.GetValues(typeof(BuildingAnchorDirection));
        for (int i = 0; i < enumValues.Length; i++)
        {
            var ev = (BuildingAnchorDirection)enumValues.GetValue(i);
            //var val = myTarget.anchors.ContainsKey(ev) ? myTarget.anchors[ev] : null;
            var val = myTarget.GetAnchor(ev);

            EditorGUILayout.BeginHorizontal();

            //var active = myTarget.anchorsActive.ContainsKey(ev) ? myTarget.anchorsActive[ev] : true;
            var active = myTarget.GetAnchorActive(ev);


            if (GUILayout.Button(active ? "Disable" : "Enable"))
            {
                //myTarget.anchorsActive[ev] = !active;
                myTarget.SetAnchorActive(ev, !active);

                IconManager.SetIcon(val.gameObject, !active ? IconManager.LabelIcon.Blue : IconManager.LabelIcon.Red);
            }

            GUILayout.Label(ev.ToString());

            GUI.enabled = active;
            var nval = (Transform)EditorGUILayout.ObjectField(val, typeof(Transform), true);
            GUI.enabled = true;


            EditorGUILayout.EndHorizontal();

            if (nval != val && nval != null)
            {
                //myTarget.anchors[ev] = nval;
                myTarget.UpdateAnchor(ev, nval);

                //Debug.Log("Update anchor value");
            }

            if (GUI.changed)
            {
                //target = myTarget;
                //serObj.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(myTarget);
                PrefabUtility.RecordPrefabInstancePropertyModifications(myTarget);
            }

        }

        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}



public class UI : MonoBehaviour
{
    public static GameObject text;

    [MenuItem("UI/Text")]
    static void DoSomething()
    {
        Instantiate(text);
    }

    [MenuItem("Assets/Create/Py file", false, 1)]
    private static void CreateNewAsset()
    {
        ProjectWindowUtil.CreateAssetWithContent(
            "pycode.py",
            @"
from game import *
from game_funcs import *

class NewClass():
    def start(self):
        pass

def export():
    return NewClass
");
    }

}


#endif
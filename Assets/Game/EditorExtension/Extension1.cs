using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(GameResourcesManager)), CanEditMultipleObjects]
public class ResourcesManagerEditor : Editor
{
    bool isOnScene = false;
    bool _editNames = false;

    void DisableGUI()
    {
        GUI.enabled = false;
    }
    void EnableGUI()
    {
        if (!isOnScene)
            GUI.enabled = true;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();

        GameResourcesManager resManager = (GameResourcesManager)target;


        isOnScene = resManager.gameObject.scene.name != null;

        if (isOnScene)
            DisableGUI();
        else
            Global.resourcesManager = resManager;


        EditorGUILayout.LabelField(resManager.selectedRes == null ? "select res" : resManager.selectedRes.name);

        var res = EditorGUILayout.ObjectField("Res", resManager.selectedRes, typeof(UnityEngine.Object), false) as UnityEngine.Object;

        if (res != null)
        {
            resManager.selectedRes = res;
        }

        string showGuid = System.Guid.Empty.ToString();

        if (resManager.selectedRes != null)
        {
            var info = resManager.GetUUIDOfResource(resManager.selectedRes, false);
            showGuid = info.uuid;
        }

        EditorGUILayout.TextField(showGuid.ToString());
        if (GUILayout.Button("Add"))
        {

            var selected = resManager.selectedRes;
            var uuid = showGuid == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : showGuid;

            if (resManager.resources.All((x) => x.uuid != uuid))
            {
                resManager.resources.Add(new ResourceInfo
                {
                    name = selected.name,
                    res = selected,
                    uuid = uuid,
                });
            }
            else
                EditorUtility.DisplayDialog("Alert", "Resource is already added", "ok");

        }

        _editNames = EditorGUILayout.Toggle("Edit names", _editNames);

        for (int i = 0; i < resManager.resources.Count; i++)
        {
            var resInfo = resManager.resources[i];

            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField(resInfo.uuid);
            if (GUILayout.Button("Copy"))
                GUIUtility.systemCopyBuffer = resInfo.uuid;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (!_editNames) DisableGUI();
            resInfo.name = EditorGUILayout.TextField(resInfo.name);
            if (!_editNames) EnableGUI();

            resInfo.res = EditorGUILayout.ObjectField("", resInfo.res, typeof(UnityEngine.Object), false);

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(resManager);
            PrefabUtility.RecordPrefabInstancePropertyModifications(resManager);

            var filePath = Path.Combine(Utils.LOG_PATH, "res_infos", $"{DateTime.Now:dMMMyyyy}_res_info.json");
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, Newtonsoft.Json.JsonConvert.SerializeObject(resManager.resources));
        }
    }
}


[CustomEditor(typeof(PrefabManager)), CanEditMultipleObjects]
public class PrefabManagerEditor : Editor
{
    bool _editNames = false;
    bool isOnScene = false;

    void DisableGUI()
    {
        GUI.enabled = false;
    }
    void EnableGUI()
    {
        if (!isOnScene)
            GUI.enabled = true;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();

        PrefabManager prefabManager = (PrefabManager)target;

        isOnScene = prefabManager.gameObject.scene.name != null;

        if (isOnScene)
            DisableGUI();
        //var selObj = serializedObject.FindProperty("selectedPrefab").arr;
        //serializedObject.FindProperty("selectedPrefab");


        EditorGUILayout.LabelField(prefabManager.selectedPrefab == null ? "select prefab" : prefabManager.selectedPrefab.name);

        var prefab = EditorGUILayout.ObjectField("Prefab", prefabManager.selectedPrefab, typeof(Prefab), false) as Prefab;

        if (prefab != null)
        {
            prefabManager.selectedPrefab = prefab;
        }

        string showGuid = System.Guid.Empty.ToString();

        if (prefabManager.selectedPrefab != null)
        {
            showGuid = prefabManager.selectedPrefab.uuid;
        }

        EditorGUILayout.TextField(showGuid.ToString());
        if (GUILayout.Button("Add"))
        {

            var selected = prefabManager.selectedPrefab;
            var uuid = selected.uuid == Guid.Empty.ToString() ? Guid.NewGuid().ToString() : selected.uuid;

            if (prefabManager.prefabs.All((x) => x.uuid != uuid))
            {
                prefabManager.prefabs.Add(new PrefabInfo
                {
                    name = selected.name,
                    prefab = selected,
                    uuid = uuid,
                });
                selected.uuid = uuid;

                Undo.RecordObject(selected, "set uuid");
            }
            else
                EditorUtility.DisplayDialog("Alert", "Prefab is already added", "ok");

        }

        _editNames = EditorGUILayout.Toggle("Edit names", _editNames);

        for (int i = 0; i < prefabManager.prefabs.Count; i++)
        {
            var prefabInfo = prefabManager.prefabs[i];

            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField(prefabInfo.uuid);
            if (GUILayout.Button("Copy"))
                GUIUtility.systemCopyBuffer = prefabInfo.uuid;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (!_editNames) DisableGUI();
            prefabInfo.name = EditorGUILayout.TextField(prefabInfo.name);
            if (!_editNames) EnableGUI();

            EditorGUILayout.ObjectField("", prefabInfo.prefab, typeof(Prefab), false);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Update all uuids"))
        {
            for (int i = 0; i < prefabManager.prefabs.Count; i++)
            {
                prefabManager.prefabs[i].prefab.uuid = prefabManager.prefabs[i].uuid;

                Undo.RecordObject(prefabManager.prefabs[i].prefab, "set uuid");
            }

        }



        if (GUI.changed)
        {
            EditorUtility.SetDirty(prefabManager);
            PrefabUtility.RecordPrefabInstancePropertyModifications(prefabManager);

            var filePath = Path.Combine(Utils.LOG_PATH, "prefabs_infos", $"{DateTime.Now:dMMMyyyy}_prefab_info.json");
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, Newtonsoft.Json.JsonConvert.SerializeObject(prefabManager.prefabs));
        }
        //serializedObject.ApplyModifiedProperties();
    }
}
#endif
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PythonMonoBehaviourScript))]
public class PythonMonoBehaviourScriptEditor : Editor
{
    void DisableGUI()
    {
        GUI.enabled = false;
    }
    void EnableGUI()
    {
        GUI.enabled = true;
    }

    public override void OnInspectorGUI()
    {
        PythonMonoBehaviourScript pyMonoBehaviour = (PythonMonoBehaviourScript)target;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = File.Exists(StPythonEngine.MakeGameSourcePath(pyMonoBehaviour.scriptPath)) ? Color.white : Color.red;

        pyMonoBehaviour.scriptPath = EditorGUILayout.TextField("Script path: ", pyMonoBehaviour.scriptPath, style);

        if (GUILayout.Button("Select"))
        {
            string path = EditorUtility.OpenFilePanel("Select script", StPythonEngine.MakeGameSourcePath("/"), "py");
            //Debug.Log(path);
            pyMonoBehaviour.scriptPath = path.Replace(StPythonEngine.MakeGameSourcePath(""), "");
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("root"), new GUIContent("Root object:"));

        //var list = pyMonoBehaviour.variables;
        var list = serializedObject.FindProperty("variables");
        EditorGUILayout.PropertyField(list, new GUIContent("Variables:"), true);

        GUILayout.Space(5);

        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Reload scipt"))
        {
            pyMonoBehaviour.ReloadScript();
        }
        GUI.enabled = true;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(pyMonoBehaviour);
            PrefabUtility.RecordPrefabInstancePropertyModifications(pyMonoBehaviour);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
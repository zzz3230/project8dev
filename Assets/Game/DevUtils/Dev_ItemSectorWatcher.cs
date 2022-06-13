using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR && false
[CustomEditor(typeof(Dev_ItemSectorWatcher))]
public class Dev_ItemSectorWatcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Dev_ItemSectorWatcher myTarget = (Dev_ItemSectorWatcher)target;
        foreach (var itemInstance in ItemsManager._loadedSectors[0])
        {
            EditorGUILayout.PropertyField(itemInstance, typeof(ItemInstance));
        }
    }
}
#endif

//[ExecuteAlways]
public class Dev_ItemSectorWatcher : MonoBehaviour
{
    [SerializeField] int startRange = 0;
    [SerializeField] int endRange = 64;
    [SerializeField] ItemInstance[] items;


    private void Update()
    {
        items = ItemsManager._loadedSectors[0]
            [
            new System.Range(
                new System.Index(startRange), 
                new System.Index(endRange))
            ];
    }
}

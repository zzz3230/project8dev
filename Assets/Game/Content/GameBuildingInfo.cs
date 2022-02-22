using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameBuildingInfo
{
    public bool interactive { get { return _interactive; } }
    [SerializeField] bool _interactive;

    public BaseBuildingPrefabClass originalPrefabClass { get { return _originalPrefabClass; } }
    [SerializeField] BaseBuildingPrefabClass _originalPrefabClass;
}

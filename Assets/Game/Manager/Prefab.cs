using System;
using UnityEngine;

public class Prefab : MonoBehaviour
{
    public string prefabName = "--name--";
    [ReadOnly] public string uuid = Guid.Empty.ToString();
}

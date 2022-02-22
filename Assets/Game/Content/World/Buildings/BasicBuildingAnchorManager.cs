using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum BuildingAnchorDirection
{
    /*Left,
    Right,
    Up,
    Down,
    Forward,
    Back*/
    FUpLeft,
    FUpRight,
    FDownLeft,
    FDownRight, 
    BUpLeft,
    BUpRight,
    BDownLeft,
    BDownRight
}

[Serializable]
public class BasicBuildingAnchorManager : MonoBehaviour
{
    //[SerializeField] public Dictionary<BuildingAnchorDirection, Transform> anchors = new Dictionary<BuildingAnchorDirection, Transform> { };
    //[SerializeField] public Dictionary<BuildingAnchorDirection, bool> anchorsActive = new Dictionary<BuildingAnchorDirection, bool> { };

    public List<BuildingAnchorDirection> anchorsKeys = new List<BuildingAnchorDirection> { };
    public List<Transform> anchorsValues = new List<Transform> { };

    public List<BuildingAnchorDirection> anchorsActiveKeys = new List<BuildingAnchorDirection> { };
    public List<bool> anchorsActiveValues = new List<bool> { };


    public void SetAnchorActive(BuildingAnchorDirection dir, bool val)
    {
        if (anchorsActiveKeys.Contains(dir))
        {
            anchorsActiveValues[anchorsActiveKeys.IndexOf(dir)] = val;
        }
        else
        {
            anchorsActiveKeys.Add(dir);
            anchorsActiveValues.Add(val);
        }
    }
    
    public bool GetAnchorActive(BuildingAnchorDirection dir)
    {
        if (anchorsActiveKeys.Contains(dir))
        {
            return anchorsActiveValues[anchorsActiveKeys.IndexOf(dir)];
        }
        else
        {
            return true;
        }
    }

    public void UpdateAnchor(BuildingAnchorDirection dir, Transform tr)
    {
        if (anchorsKeys.Contains(dir))
        {
            anchorsValues[anchorsKeys.IndexOf(dir)] = tr;
        }
        else
        {
            anchorsKeys.Add(dir);
            anchorsValues.Add(tr);
        }
    }
    public Transform GetAnchor(BuildingAnchorDirection dir)
    {
        if (anchorsKeys.Contains(dir))
        {
            return anchorsValues[anchorsKeys.IndexOf(dir)];
        }
        else
        {
            return null;
        }
    }

    public List<Transform> GetNearestsAnchors(Vector3 pos)
    {
        return anchorsValues.OrderBy((t) => (pos - t.position).sqrMagnitude).ToList();
    }

}

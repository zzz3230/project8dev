using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    protected Quaternion _rotation = Quaternion.identity;
    public Quaternion rotation { get { return _rotation; } set { _rotation = value; } }
    public virtual void BeginBuilding(NewBasePlayerScript playerScript, RuntimeBuildingInfoScript originalPrefab) {  }
    public virtual void EndBuilding() { }
    public virtual bool Place() { return false; }
}

[System.Serializable]
public enum BuilderID
{
    None, Base, Def
}
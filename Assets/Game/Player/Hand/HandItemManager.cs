using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemManager : MonoBehaviour
{
    public Transform anchor;
    public GameObject itemObjectInstance; 
    
    public DefBuilder defBuilderOriginal;
    public DefBuilder defBuilder;
    public NewBasePlayerScript playerScript;
    bool building = false;
    // Start is called before the first frame update
    void Start()
    {
        // defBuilder.BeginBuilding(this, pref)
        defBuilder = Utils.SpawnBuilder(defBuilderOriginal);
    }

    public void AppendBuilderRotation(float mw)
    {
        var rot = defBuilder.rotation.eulerAngles;
        rot.y += mw;
        defBuilder.rotation = Quaternion.Euler(rot);
    }

    public void UseItem(MouseButton mbtn)
    {
        if(mbtn == MouseButton.Right)
        {
            if (building)
                defBuilder.Place();
        }
    }

    public void SetCurrentInHandItem(ItemsManagerPointer ptr)
    {
        Destroy(itemObjectInstance);
        if (building)
        {
            defBuilder.EndBuilding();
            building = false;
        }

        var item = ptr[0];
        
        if (item.empty)
            return;

        if (item.info.buildingInfo.placeable)
        {
            if(item.info.builderId == BuilderID.Def)
            {
                defBuilder.BeginBuilding(playerScript, item.info.buildingInfo.previewPrefab.GetComponent<RuntimeBuildingInfoScript>());
                building = true;
            }
            return;
        }

        if(item.info.handItemInfo.interactiveHandObject != null)
        {
            var pref = Instantiate(item.info.handItemInfo.interactiveHandObject, anchor);
            var info = pref.GetComponent<RuntimeUnitInfoScript>();
            info.Setup(item, playerScript);
            itemObjectInstance = pref.gameObject;
        }
        else
        {
            itemObjectInstance = Instantiate(item.info.handItemInfo.staticHandObject, anchor).gameObject;
        }
    }
}

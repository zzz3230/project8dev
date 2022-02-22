using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class TEST_loganc : MonoBehaviour
{
    private void Start()
    {
        Instantiate(PrefabManager.Instance.GetPrefabByUUID("f20a7075-115c-43ff-962c-6fce03f8a0e6")).transform.parent = transform;
    }

    private void Update()
    {
        //Debug.Log(GetComponent<BasicBuildingAnchorManager>().anchors[BuildingAnchorDirection.Forward]);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBuilder : BaseBuilder
{
    NewBasePlayerScript _playerScript;
    bool _updating = false;
    BaseBuildingPrefabClass _originalPrefab;
    BaseBuildingPrefabClass _previewPrefab;

    public Material validMat;
    public Material invalidMat;
    public string buildingLayerName = "BuildingPreview";
    public Layers buildingLayer = Layers.BuildingPreview;

    public override void BeginBuilding(NewBasePlayerScript playerScript, BaseBuildingPrefabClass originalPrefab)
    {
        base.BeginBuilding(playerScript, originalPrefab);
        _playerScript = playerScript;
        _updating = true;
        _originalPrefab = originalPrefab;

        _previewPrefab = Instantiate(originalPrefab);

        Utils.SetMaterial(_previewPrefab.gameObject, validMat);

        Utils.SetLayerOnAll(_previewPrefab.gameObject, Utils.GetLayer(buildingLayer));
    }

    private void Update()
    {
        //print("UDP");
        if (!_updating) return;

        RaycastHit hit;
        Ray ray = _playerScript._camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //пускаем луч
        Physics.Raycast(ray, out hit, 10, ~Utils.GetLayerMask(buildingLayer)); 
        Debug.DrawRay(ray.origin, ray.direction * 10);
        //print(ray.origin + " : " + hit.point);
        if (hit.collider != null)
        {
            Vector3 previewPos = Vector3.one;
            Quaternion previewRot = Quaternion.identity;
            BaseBuildingPrefabClass by = null;

            if (hit.collider.gameObject.transform.parent != null && hit.collider.gameObject.transform.parent.TryGetComponent<BaseBuildingPrefabClass>(out var b))
            {
                //print(hit.collider.gameObject.transform.parent.name);
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    var gr = ((BasicBuildingGroup)b.group);
                    gr.Remove(b);
                    Destroy(b.gameObject);
                }

                var nearAncs = b.anchorManager.GetNearestsAnchors(hit.point);

                previewPos = nearAncs[0].position;

                by = b;
            }
            else
            {
                previewPos = hit.point;
            }


            _previewPrefab.transform.position = previewPos;
            _previewPrefab.transform.rotation = _rotation;

            if (Input.GetKeyDown(KeyCode.R))
            {
                var obj = Instantiate(_originalPrefab, _previewPrefab.transform.position, _rotation);
                if(by != null)
                {
                    if (by.group == null)
                        by.group = BasicBuildingGroup.Create();
                    
                    ((BasicBuildingGroup)by.group).Add(obj, by, default);
                }
                else
                    obj.group = BasicBuildingGroup.Create();
            }
        }
    }
}

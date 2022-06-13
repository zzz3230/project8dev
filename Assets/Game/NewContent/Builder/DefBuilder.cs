using UnityEngine;

public class DefBuilder : BaseBuilder
{
    NewBasePlayerScript _playerScript;
    bool _updating = false;
    RuntimeBuildingInfoScript _originalPrefab;
    RuntimeBuildingInfoScript _previewPrefab;
    OverlapCheckerScript _previewPrefabOverlapChecker;

    RuntimeBuildingInfoScript by;

    public Material validMat;
    public Material invalidMat;
    public string buildingLayerName = "BuildingPreview";
    public Layers buildingLayer = Layers.BuildingPreview;
    bool canPlace = true;

    public override void BeginBuilding(NewBasePlayerScript playerScript, RuntimeBuildingInfoScript originalPrefab)
    {
        base.BeginBuilding(playerScript, originalPrefab);
        _playerScript = playerScript;
        _updating = true;
        _originalPrefab = originalPrefab;
        canPlace = true;

        _previewPrefab = Instantiate(originalPrefab);

        _previewPrefabOverlapChecker = _previewPrefab.GetComponent<OverlapCheckerScript>();

        Utils.SetMaterial(_previewPrefab.gameObject, validMat);

        Utils.SetLayerOnAll(_previewPrefab.gameObject, Utils.GetLayer(buildingLayer));
    }
    public override void EndBuilding()
    {
        base.EndBuilding();
        if (!_updating)
            return;

        _updating = false;
        _originalPrefab = null;
        Destroy(_previewPrefab.gameObject);
    }
    public override bool Place()
    {
        base.Place();

        if (!canPlace)
            return false;

        var obj = Instantiate(_originalPrefab, _previewPrefab.transform.position, _rotation);
        if (by != null)
        {
            if (by.group == null)
                by.group = BasicBuildingGroup.Create();

            ((BasicBuildingGroup)by.group).Add(obj, by, default);
        }
        else
            obj.group = BasicBuildingGroup.Create();

        _playerScript.DecrementCountInHand();

        return true;
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
            _previewPrefab.gameObject.SetActive(true);

            Vector3 previewPos = Vector3.one;
            Quaternion previewRot = Quaternion.identity;
            //RuntimeBuildingInfoScript by = null;

            bool lCtrl = Input.GetKey(KeyCode.LeftControl);

            if (!lCtrl && hit.collider.gameObject.transform.parent != null && hit.collider.gameObject.transform.parent.TryGetComponent<RuntimeBuildingInfoScript>(out var b))
            {
                _previewPrefab.gameObject.SetActive(true);
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
                if (hit.distance < 10)
                {

                    previewPos = hit.point;
                    if (!lCtrl)
                    {
                        /*previewPos = new Vector3(
                            (float)Math.Round(previewPos.x, 0),
                            (float)Math.Round(previewPos.y, 0),
                            (float)Math.Round(previewPos.z, 0)
                            );*/
                        previewPos = new Vector3(
                            previewPos.x - previewPos.x % 0.8f,
                            previewPos.y - previewPos.y % 0.8f,
                            previewPos.z - previewPos.z % 0.8f
                            );
                    }
                }
                else
                {
                }
            }



            _previewPrefab.transform.position = previewPos;
            _previewPrefab.transform.rotation = _rotation;

            var newCanPlace = !_previewPrefabOverlapChecker.CheckOveralp();
            if (newCanPlace != canPlace)
            {
                canPlace = newCanPlace;
                Utils.SetMaterial(_previewPrefab.gameObject, canPlace ? validMat : invalidMat);
            }

            //_previewPrefab.GetComponentsInChildren<BoxCollider>()[0]
            //    ;Physics.

            if (Input.GetKeyDown(KeyCode.R))
            {
                var obj = Instantiate(_originalPrefab, _previewPrefab.transform.position, _rotation);
                if (by != null)
                {
                    if (by.group == null)
                        by.group = BasicBuildingGroup.Create();

                    ((BasicBuildingGroup)by.group).Add(obj, by, default);
                }
                else
                    obj.group = BasicBuildingGroup.Create();
            }
        }
        else
            _previewPrefab.gameObject.SetActive(false);
    }
}

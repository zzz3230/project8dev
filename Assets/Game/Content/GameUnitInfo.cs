using UnityEngine;


[System.Serializable]
public class GameUnitInfo
{
    public bool placeable { get { return _placeable; } }
    [SerializeField] bool _placeable;

    public bool isItem { get { return _isItem; } }
    [SerializeField] bool _isItem;

    public GameItemInfo itemInfo { get { return _itemInfo; } }
    [SerializeField] GameItemInfo _itemInfo;


    public GameBuildingInfo placingInfo { get { return _placingInfo; } }
    [SerializeField] GameBuildingInfo _placingInfo;

    public string unitStrId { get { return _unitStrId; } }
    [SerializeField] string _unitStrId;

    public ulong unitHash { get { return _unitHash; } }
    [SerializeField] ulong _unitHash;


}

using UnityEngine;

public struct HintUnitData
{
    public string name;
    public ItemInstance itemInstance;
}

public class HintUnitScript : MonoBehaviour
{
    public HintUnitData GetData()
    {
        return new HintUnitData
        {
            name = "NAME",
            itemInstance = new ItemInstance
            {
                count = 3,
                empty = false,
                info = ItemsHub.GetItemInfoBySIID(SIID.game_brick_wall_01),
                metadata = new UnitMetadata
                {
                    durability = 10f,
                    maxDurability = 35f,
                    uuid = "1234-5678"
                }
            }
        };
    }
}

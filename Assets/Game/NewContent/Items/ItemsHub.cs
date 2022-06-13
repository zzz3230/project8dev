using System.Collections.Generic;

public static class ItemsHub
{
    static List<ItemInfo> items = new List<ItemInfo>();

    static Dictionary<string, ItemInfo> itemsInfoPairsById = new Dictionary<string, ItemInfo>();

    public static void AddItemsInfo(IEnumerable<ItemInfo> _items)
    {
        items.AddRange(_items);

        itemsInfoPairsById.Clear();
        foreach (var item in _items)
        {
            if (item.strId == "~" || item.strId == "~SIID")
                item.strId = item.id.ToString();

            itemsInfoPairsById.Add(item.strId, item);
        }
    }
    public static ItemInfo GetItemInfoBySIID(SIID siid)
    {
        //return items.Find(i => i.id == siid);
        return itemsInfoPairsById[siid.ToString()];
    }
    public static ItemInfo GetItemInfoByStrId(string id)
    {
        //Log.Ms(itemsInfoPairsById[id]);
        //return items.Find(i => i.strId == id);
        return itemsInfoPairsById[id];

    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InbuiltItemsScript : MonoBehaviour
{
    public List<SO_ItemInfo> items;
    private void Awake()
    {
        ItemsHub.AddItemsInfo(items.Select(i => i.item));
    }
}

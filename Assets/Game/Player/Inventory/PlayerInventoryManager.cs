using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public Action updateInventory;

    ItemsManagerPointer[] itemsPointers;

    void CallUpdate()
    {
        updateInventory();
    }

    public void MoveItemInInventory(SlotManager source, SlotManager destination, int count = 1)
    {
        if (count == 0)
            return;

        if(count == 1)
        {
            (destination.localIndex, source.localIndex) = (source.localIndex, destination.localIndex);
        }

        itemsPointer[destination.localIndex].count += count;
        itemsPointer[source.localIndex].count -= count;

        if(itemsPointer[source.localIndex].count <= 0)
            itemsPointer.Remove(source.localIndex);

        CallUpdate();
    }
}

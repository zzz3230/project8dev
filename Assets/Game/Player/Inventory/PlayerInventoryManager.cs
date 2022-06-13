using System;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public Action updateInventory;

    ItemsManagerPointer[] itemsPointers;

    PlayerInventoryWidgetScript inventoryScript;

    SlotManager[] slotsMangers;

    //public void SetPointers(ItemsManagerPointer[] pointers)
    //{
    //    itemsPointers = pointers;
    //}

    public void Init(PlayerInventoryWidgetScript inventoryScript, ItemsManagerPointer[] ptrs, SlotManager[] slots)
    {
        this.inventoryScript = inventoryScript;
        this.itemsPointers = ptrs;
        this.slotsMangers = slots;
    }

    public SlotManager GetSlot(int index)
    {
        return slotsMangers[index];
    }


    void CallUpdate()
    {
        //updateInventory();
        //var test = ItemsManager._loadedSectors;

        for (int i = 0; i < inventoryScript.slotsCount; i++)
        {
            //print(i);
            slotsMangers[i].ViewUpdate();
        }
    }

    public void MoveItem(int sourceSlotIndex, int destinationSlotIndex, int count = 1)
    {
        MoveItem(slotsMangers[sourceSlotIndex], slotsMangers[destinationSlotIndex], count);
    }

    public void MoveItem(SlotManager source, SlotManager destination, int count = 1)
    {
        if (count <= 0)
            return;

        SlotManager.MoveItems(source, destination, count);

        CallUpdate();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public Action updateInventory;

    ItemsManagerPointer[] itemsPointers;

    PlayerHUD_UI_Manager uiManager;

    SlotManager[] slotsMangers;

    public void SetPointers(ItemsManagerPointer[] pointers)
    {
        itemsPointers = pointers;
    }

    public void Init(PlayerHUD_UI_Manager uiManger, ItemsManagerPointer[] ptrs, SlotManager[] slots)
    {
        this.uiManager = uiManger;
        this.itemsPointers = ptrs;
        this.slotsMangers = slots;
    }


    void CallUpdate()
    {
        //updateInventory();
        var test = ItemsManager._loadedSectors;

        for (int i = 0; i < uiManager.slotsCount; i++)
        {
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

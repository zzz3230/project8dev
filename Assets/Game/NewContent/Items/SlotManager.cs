using System.Collections.Generic;
using UnityEngine;


public enum MouseButton
{
    Left,
    Right,
    Middle,
}

public class SlotManager
{
    public SlotWidgetScript widget
    {
        get => _widget;
        set
        {
            _widget = value;
            _widget.manager = this;
        }
    }
    SlotWidgetScript _widget;

    public ItemsManagerPointer itemPointer;

    readonly bool _isShadow = false;
    List<SlotManager> _shadows = new List<SlotManager>();
    public NewBasePlayerScript playerScript;
    System.Action onUpdated;
    public void BindOnUpdate(System.Action a)
    {
        onUpdated += a;
    }
    public void UnbindOnUpdate(System.Action a)
    {
        if (onUpdated is not null)
            onUpdated -= a;
    }

    public SlotManager() { }
    public SlotManager(SlotManager parent, SlotWidgetScript visualEl)
    {
        widget = visualEl;
        itemPointer = parent.itemPointer;
        _isShadow = true;
    }

    public void BeginDrag(MouseButton mBtn)
    {
        DragAndDropManager.Begin(this, mBtn);
        //Debug.Log("begin " + this.itemPointer.offset + " with button " + mBtn.ToString());
    }

    public void EndDrag()
    {
        var source = DragAndDropManager.End();
        var toMove = 1;

        if (source.mouseBtn == MouseButton.Left)
            toMove = source.slotManager.itemPointer[0].count;
        else if (source.mouseBtn == MouseButton.Right)
            toMove = source.slotManager.itemPointer[0].count / 2;

        MoveItems(source.slotManager, this, toMove);

        source.slotManager.ViewUpdate();
        ViewUpdate();


        //if (playerScript != null)
        //    playerScript.UpdateHUD();

        //Debug.Log($"end from {source.slotManager.itemPointer.offset} to {itemPointer.offset} to_move {toMove}");
    }

    public void ViewUpdate(bool fromShadow = false)
    {
        var slot = _widget;// uiManager.GetSlot(i);

        slot.durablilty = 0;

        if (itemPointer[0].empty)
        {
            slot.image = Texture2D.whiteTexture;
            slot.count = 0;
        }
        else
        {
            slot.image = itemPointer[0].info.ico;
            slot.count = itemPointer[0].count;

            var mt = itemPointer[0].metadata;
            if (mt is not null)
                if (mt.durability < mt.maxDurability)
                {
                    slot.durablilty = mt.durability / mt.maxDurability;
                }
        }

        if (!fromShadow)
            _shadows.ForEach(x => x.ViewUpdate(true));

        onUpdated?.Invoke();


        //if (fromShadow)
        //{
        //    Log.Ms("from shadow");
        //}
    }

    public SlotManager NewShadow(SlotWidgetScript visualElement)
    {
        var shadow = new SlotManager(this, visualElement);
        _shadows.Add(shadow);
        return shadow;
    }

    public void GiveItem(string strId, int count, UnitMetadata metadata = null)
    {
        itemPointer[0].info = ItemsHub.GetItemInfoByStrId(strId);
        itemPointer[0].count = count;
        itemPointer[0].metadata = metadata;
        itemPointer[0].empty = false;

        ViewUpdate();
    }

    public static void MoveItems(SlotManager source, SlotManager destination, int count = 1)
    {
        if (source._isShadow || destination._isShadow)
        {
            Log.Warning("moving items in shadow slots", "SlotManager-MoveItems");
            return;
        }

        if (source == destination)
            return;

        var isDestEmpty = destination.itemPointer[0].empty;
        var sourcePtr = source.itemPointer[0];
        var destinationPtr = destination.itemPointer[0];

        if (count == source.itemPointer[0].count)
        {
            if (isDestEmpty)
            {
                source.itemPointer.Swap(destination.itemPointer);
                return;
            }
        }

        if (
            source.itemPointer[0].info.stackable &&
            (sourcePtr.info.CompareType(destinationPtr.info) || isDestEmpty) &&
            (sourcePtr.CompareMetadata(destinationPtr) || isDestEmpty)
            ) // .info
        {
            source.itemPointer.MoveTo(
                destination.itemPointer,
                Mathf.Clamp(
                    Mathf.Clamp(
                        sourcePtr.info.stack - destinationPtr.count,
                        0,
                        sourcePtr.count
                        ),
                    0,
                    count
                    )
                );
            return;
        }


        source.itemPointer.Swap(destination.itemPointer);
    }

    internal void ViewUpdateDurability()
    {
        var mt = itemPointer[0].metadata;
        var slot = _widget;

        if (mt is not null)
            if (mt.durability < mt.maxDurability)
            {
                slot.durablilty = mt.durability / mt.maxDurability;
            }

        _shadows.ForEach(x => x.ViewUpdateDurability());
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public enum MouseButton
{
    Left, 
    Right,
    Middle,
}

public class SlotManager
{
    public SlotWidgetScript widget { 
        get => _widget;
        set {
            _widget = value;
            _widget.manager = this; }
    }
    SlotWidgetScript _widget;

    public ItemsManagerPointer itemPointer;

    readonly bool _isShadow = false;
    List<SlotManager> _shadows = new List<SlotManager>();
    public NewBasePlayerScript playerScript;

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
        Debug.Log("begin " + this.itemPointer.offset + " with button " + mBtn.ToString());
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

        if(playerScript != null)
            playerScript.UpdateHUD();

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
            if(mt is not null)
                if (mt.durability < mt.maxDurability)
                {
                    slot.durablilty = mt.durability / mt.maxDurability;
                }
        }

        if(!fromShadow)
            _shadows.ForEach(x => x.ViewUpdate(true));

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

    public static void MoveItems(SlotManager source, SlotManager destination, int count = 1)
    {
        if(source._isShadow || destination._isShadow)
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
            sourcePtr.CompareMetadata(destinationPtr)) // .info
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

        _shadows.ForEach(x => x.ViewUpdate(true));
    }
}
/*
public class SlotUIManager : VisualElement
{
    public SlotManager manager;

    public SlotUIManager()
    {
        var bg = new VisualElement();
        bg.AddToClassList("st-slot-bg");
        bgBox = bg;
        Add(bg);
        var img = new VisualElement();
        img.AddToClassList("st-slot-img");
        bg.Add(img);
        image = img;
        var text = new Label();
        text.text = "1";
        text.AddToClassList("st-slot-count-label");
        bg.Add(text);
        countLabel = text;

        this.RegisterCallback<MouseEnterEvent>(x => OnMouseEnter(x));
        this.RegisterCallback<MouseLeaveEvent>(x => OnMouseLeave(x));
        this.RegisterCallback<MouseUpEvent>(x => OnMouseUp(x));
        this.RegisterCallback<MouseDownEvent>(x => OnMouseDown(x));
    }

    private void OnMouseDown(MouseDownEvent x)
    {
        //Debug.Log("aaa");
        manager.BeginDrag((MouseButton)x.button);
    }

    private void OnMouseUp(MouseUpEvent x)
    {
        manager.EndDrag(); 
        //bgBox.style.backgroundColor = new StyleColor(new Color(1, 0, 0, 0.4f));
    }

    private void OnMouseLeave(MouseLeaveEvent x)
    {
        //bgBox.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0.4f));
    }

    private void OnMouseEnter(MouseEnterEvent x)
    {
        //bgBox.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0.7f));
    }


    public VisualElement image;
    public VisualElement bgBox;
    public Label countLabel;

    public void SetImage(Texture2D img)
    {
        image.style.backgroundImage = img;
    }

    public void SetCount(int val)
    {
        countLabel.text = val == 0 ? string.Empty : val.ToString();
    }
    bool highlighted = false;

    public void Highlight(bool val)
    {
        if (highlighted == val)
            return;

        var cl = "st-slot-bg-highlighted";
        if (highlighted)
            bgBox.RemoveFromClassList(cl);
        else
            bgBox.AddToClassList(cl);

        highlighted = val;
    }
}


public class PlayerHUD_UI_Manager : VisualElement
{
    public void SwitchVisible()
    {
        style.display = style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
    }

    public void SetÑurrentSlot(int index)
    {
        //Log.Ms(index);

        if (slots.Count == 0)
            return;
        slots.ForEach(slot => slot.Highlight(false));
        //Log.Ms(index);
        slots[index].Highlight(true);
    }

    public int slotsCount = 9;
    List<SlotUIManager> slots = new List<SlotUIManager>();

    #region loaded
    public Action<object> loaded
    {
        get { return _loaded; }
        set { if (tocall && !called) { value(this); called = true; } _loaded = value; }
    }
    Action<object> _loaded;

    bool tocall = false;
    bool called = false;
    void Loaded()
    {
        if (called)
            return;

        if (loaded == null)
        {
            tocall = true;
        }
        else
        {
            loaded(this);
            called = true;
        }

    }
    #endregion

    public SlotUIManager GetSlot(int index)
    {
        return slots[index];
    }

    public PlayerHUD_UI_Manager()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            //Log.Ms("slots spawned");

            this.Q("hud-slots-box")?.Clear();
            slots.Clear();

            for (int i = 0; i < slotsCount; i++)
            {
                slots.Add(new SlotUIManager());
                this.Q("hud-slots-box")?.Add(slots.Last());
            }

            Loaded();
            //slots[2].Highlight(true);
        });
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<PlayerHUD_UI_Manager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion

}
*/
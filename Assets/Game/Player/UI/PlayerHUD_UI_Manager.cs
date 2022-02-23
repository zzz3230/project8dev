using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class SlotManager
{
    public SlotUIManager visualElement;
    public ItemsManagerPointer itemPointer;
    
    public void ViewUpdate()
    {
        var slot = visualElement;// uiManager.GetSlot(i);

        if (itemPointer[0].empty)
        {
            slot.SetImage(Texture2D.whiteTexture);
            slot.SetCount(0);
            return;
        }

        slot.SetImage(itemPointer[0].info.ico);
        slot.SetCount(itemPointer[0].count);
    }

    public static void MoveItems(SlotManager source, SlotManager destination, int count = 1)
    {
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
            (sourcePtr.info.CompareType(destinationPtr.info) || isDestEmpty))
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
}

public class SlotUIManager : VisualElement
{
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


public class PlayerHUD_UI_Manager : VisualElement, ILoaded
{
    public void SwitchVisible()
    {
        style.display = style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
    }

    public void SetÑurrentSlot(int index)
    {
        if (slots.Count == 0)
            return;
        slots.ForEach(slot => slot.Highlight(false));
        //Log.Ms(index);
        slots[index].Highlight(true);
    }

    public int slotsCount = 9;
    List<SlotUIManager> slots = new List<SlotUIManager>();

    public System.Action<object> loaded { get; set; }

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

            loaded?.Invoke(this);
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

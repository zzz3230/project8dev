using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class SlotManager
{
    public SlotUIManager visualElement;
    public int localIndex = -1;
    public int globalIndex = -1;
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


public class PlayerHUD_UI_Manager : VisualElement
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
        slots[index].Highlight(true);
    }

    public int slotsCount = 9;
    List<SlotUIManager> slots = new List<SlotUIManager>();

    public SlotUIManager GetSlot(int index)
    {
        return slots[index];
    }

    public PlayerHUD_UI_Manager()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            this.Q("hud-slots-box")?.Clear();
            slots.Clear();

            for (int i = 0; i < slotsCount; i++)
            {
                slots.Add(new SlotUIManager());
                this.Q("hud-slots-box")?.Add(slots.Last());
            }

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

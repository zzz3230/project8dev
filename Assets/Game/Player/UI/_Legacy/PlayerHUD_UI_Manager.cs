

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
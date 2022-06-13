using UnityEngine.UIElements;

public class StVisualElement : VisualElement
{
    public void Show()
    {
        style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        style.display = DisplayStyle.None;
    }
}

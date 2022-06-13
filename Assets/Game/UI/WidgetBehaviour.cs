using UnityEngine;

public class WidgetBehaviour : MonoBehaviour
{
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public T CastTo<T>() where T : WidgetBehaviour
    {
        return (T)this;
    }
}

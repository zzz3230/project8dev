using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    System.Action onClick;
    public void BindOnClick(System.Action a)
    {
        onClick += a;
    }
    public void ButtonClicked()
    {
        onClick?.Invoke();
    }
}

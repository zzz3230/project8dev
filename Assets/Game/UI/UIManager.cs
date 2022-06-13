using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDoc;

    public T Q<T>(string name) where T : VisualElement
    {
        return uiDoc.rootVisualElement.Q<T>(name);
    }
}

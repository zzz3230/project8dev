using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class MainMenuUIManager : VisualElement
{
    public MainMenuUIManager()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            this.Q("menu-host-btn")?.RegisterCallback<ClickEvent>((ev) =>
            {

            }); 
            this.Q("menu-client-btn")?.RegisterCallback<ClickEvent>((ev) =>
            {

            });
        });
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<MainMenuUIManager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

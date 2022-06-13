using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class StTextField : StVisualElement
{
    bool created = false;
    TextField textField;

    public string value { get => textField.value; set { textField.value = value; } }
    bool lastIn = false;

    public Action onEnterClicked;

    public StTextField()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            if (created)
                return;

            textField = new TextField();
            textField.value = "value -- value";
            Add(textField);

            var evSys = EventSystem.current;

            textField.RegisterCallback<FocusOutEvent>((ev) =>
            {
                if (lastIn)
                {
                    lastIn = false;
                    return;
                }
                StInput.isTextFieldEditing = false;
                //evSys.enabled = true;
                //$"focus out {ev.timestamp}".log();
            });

            textField.RegisterCallback<FocusInEvent>((ev) =>
            {
                //evSys.enabled = false; 
                StInput.isTextFieldEditing = true;
                lastIn = true;
                //$"focus in {ev.timestamp}".log();
            });

            textField.RegisterCallback<KeyDownEvent>((ev) =>
            {
                if (ev.keyCode == KeyCode.Return)
                {
                    onEnterClicked();
                    textField.Focus();
                }
            });

            created = true;
        });
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<StTextField, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class TestOfVIsualEl : VisualElement
{
    int progress = 0;

    public TestOfVIsualEl()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            this.Q("test-btn")?.RegisterCallback<ClickEvent>((ev) => 
            { 
                progress++;
                this.Q<ProgressBar>("test-progbar").value = progress;

            });
        });
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<TestOfVIsualEl, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

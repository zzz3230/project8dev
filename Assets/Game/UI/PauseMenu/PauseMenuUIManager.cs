using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class PauseMenuUIManager : StVisualElement
{
    bool created = false;
    public PauseMenuUIManager()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            if (created)
                return;

            this.Q<Button>("btn-continue").RegisterCallback<ClickEvent>((ev) =>
            {
                Global.gameManager?.ChangeGameState(GameState.Playing);
            });

            this.Q<Button>("btn-settings").RegisterCallback<ClickEvent>((ev) =>
            {
            });

            this.Q<Button>("btn-exit").RegisterCallback<ClickEvent>((ev) =>
            {
                Application.Quit();
            });

            created = true;
        });
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<PauseMenuUIManager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

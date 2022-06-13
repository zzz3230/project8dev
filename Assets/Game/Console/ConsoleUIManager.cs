using System;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class ConsoleUIManager : StVisualElement
{
    bool created = false;

    StPythonEngine cmdEngine = StPython.CreateEngine();

    void WriteLine(object msg)
    {
        outLabel.text += $"[{DateTime.Now:HH:mm:ss}]" + msg + "\n";
    }
    void ExecEntred()
    {
        WriteLine(cmdEngine.Execute(commandField.value));
        commandField.value = "";
    }

    Label outLabel;
    StTextField commandField;

    public ConsoleUIManager()
    {
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            if (created)
                return;


            outLabel = this.Q<Label>("console-out-label");
            commandField = this.Q<StTextField>("command-field");



            outLabel.text = "";
            commandField.value = "";


            cmdEngine.onCatchException += (msg) => WriteLine(msg);

            this.Q<Button>("send-btn").RegisterCallback<ClickEvent>((ev) =>
            {
                //WriteLine(cmdEngine.Execute(commandLabel.value));
                //commandLabel.value = "";
                ExecEntred();
            });

            commandField.onEnterClicked += () => ExecEntred();

            created = true;
        });
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<ConsoleUIManager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

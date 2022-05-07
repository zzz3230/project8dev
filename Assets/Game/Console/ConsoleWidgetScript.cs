using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ConsoleWidgetScript : WidgetBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI outputText;

    public static ConsoleWidgetScript instance;

    StPythonEngine cmdEngine;

    void WriteLine(object msg)
    {
        outputText.text += $"[{DateTime.Now:HH:mm:ss}] " + msg + "\n";
    }
    void ExecEntred()
    {
        var res = cmdEngine.Execute(inputField.text);
        if(res != null)// && (res.GetType() == typeof(string) && res != ""))
        {
            if(res.GetType() != typeof(string))
            {
                cmdEngine.SetVar(res, "_");
                res = cmdEngine.Execute($"str(_)");
            }
            WriteLine(res);
        }
            

        inputField.text = "";
    }
    private void Awake()
    {
        if (instance != null)
            Log.Error($"instance of {nameof(ConsoleWidgetScript)} already exist");
        instance = this; 
        cmdEngine = StPython.CreateEngine();
    }
    private void Start()
    {
        inputField.text = ""; 
        cmdEngine.onCatchException += (s) => WriteLine($"<color=red>{s}</color>");
    }

    /*void ExecEntred()
    {

    }*/

    public void OnDeselect()
    {
        //print("deselect"); 
        StInput.isTextFieldEditing = false;
    }
    public void OnSelect()
    {
        //print("select");
        StInput.isTextFieldEditing = true;
    }
    public void OnSubmit()
    {
        //print("submit");
        ExecEntred();
        inputField.ActivateInputField();//.Select();
    }

    internal void AddClassInstance(object obj, string name)
    {
        //throw new NotImplementedException();
        cmdEngine.SetVar(obj, name);
    }
}

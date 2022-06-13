using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CustomPythonVariable
{
    public string name;
    public UnityEngine.Object value;
}

public class PythonMonoBehaviourScript : MonoBehaviour
{
    public string scriptPath;
    public StPythonEngine engine;
    dynamic inst;
    List<string> methodsNames = new List<string>();
    [SerializeField] public List<CustomPythonVariable> variables = new List<CustomPythonVariable>();
    public GameObject root;

    bool isUpdate = false;
    bool isFixedUpdate = false;

    //List<Coroutine> coroutines = new List<Coroutine>();
    public dynamic GetInstance()
    {
        return inst;
    }

    private void Init()
    {
        if (root)
            variables.Add(new CustomPythonVariable { name = "root", value = root });

        var info = StPython.RegisterPythonMonoBehaviourScript(this);
        inst = info.instance;
        methodsNames = info.methods;



        isUpdate = methodsNames.Contains("update");
        isFixedUpdate = methodsNames.Contains("fixed_update");

        if (methodsNames.Contains("sec_update"))
            StartCoroutine(nameof(SecUpdate));

        postInit?.Invoke();

        if (methodsNames.Contains("start"))
            inst.start();
    }

    public void Start()
    {
        Init();
    }

    void Update()
    {
        if (isUpdate)
            inst.update();
    }
    private void FixedUpdate()
    {
        if (isFixedUpdate)
            inst.fixed_update();
    }

    IEnumerator SecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            inst.sec_update();
        }
    }

    Action postInit;
    public void BindPostInit(Action a)
    {
        postInit += a;
    }

    public void ReloadScript()
    {
        StopAllCoroutines();
        Init();
    }
}

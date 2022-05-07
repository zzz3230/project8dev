using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonMonoBehaviourScript : MonoBehaviour
{
    public string scriptPath;
    public StPythonEngine engine;
    dynamic inst;
    List<string> methodsNames = new List<string>();

    bool isUpdate = false;
    bool isFixedUpdate = false;

    //List<Coroutine> coroutines = new List<Coroutine>();

    private void Init()
    {
        var info = StPython.RegisterPythonMonoBehaviourScript(this);
        inst = info.instance;
        methodsNames = info.methods;

        isUpdate = methodsNames.Contains("update");
        isFixedUpdate = methodsNames.Contains("fixed_update");

        if (methodsNames.Contains("sec_update"))
            StartCoroutine(nameof(SecUpdate));

        if(methodsNames.Contains("start"))
            inst.start();
    }

    void Start()
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
        if(isFixedUpdate)
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

    internal void ReloadScript()
    {
        StopAllCoroutines();
        Init();
    }
}

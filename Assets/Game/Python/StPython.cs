using IronPython.Hosting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using IronPython.Runtime.Types;
using System.Linq;

public class Gcore
{
    public void debug_log(string msg)
    {
        Debug.Log(msg);
    }
}

public static class StPython
{
    public static StPythonEngine CreateEngine()
    {
        var eng = Python.CreateEngine();
        var scope = eng.CreateScope();
        return new StPythonEngine(eng, scope);
    }

    static List<Type> typesToPy = new List<Type>()
    {
        typeof(Vector3),
        typeof(Vector2),
        typeof(Transform),
        typeof(StInput),
        typeof(RuntimeHandItemScript),
        typeof(Time),
        typeof(RefHub),
        typeof(Quaternion),
        typeof(UnityEngine.Object),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(MonoBehaviour),
    };

    public static StPythonEngine CreateGameScriptsEngine()
    {
        var eng = Python.CreateEngine();
        var scope = eng.CreateScope();
        var en = new StPythonEngine(eng, scope);

        en.onCatchException += (msg) => throw new Exception(msg);
        en.SetGlobalVar(new Gcore(), "gcore");

        for (int i = 0; i < typesToPy.Count; i++)
        {
            var t = DynamicHelpers.GetPythonTypeFromType(typesToPy[i]); 
            en.SetGlobalVar(t, typesToPy[i].Name);
        }

        en.AddSearchPath("/", true);
        en.AddSearchPath("/GameApi/", true);

        return en;
    }
    static StPythonEngine gameScriprsEngine;
    public static (dynamic instance, List<string> methods) RegisterPythonMonoBehaviourScript(PythonMonoBehaviourScript scr)
    {
        if(gameScriprsEngine == null)
            gameScriprsEngine = CreateGameScriptsEngine();

        gameScriprsEngine.ExecuteFile(scr.scriptPath, true);
        dynamic export = gameScriprsEngine.GetVar("export");

        dynamic mainCls = export();
        
        dynamic mainClsInst = mainCls();

        dynamic dir = gameScriprsEngine.Execute("dir(export())");
        string[] allMethodsNames = new string[] { "start", "sec_update", "update", "fixed_update", "third_update", "fivesec_update"};
        List<string> methodsNames = new List<string>();
        for (int i = 1; i < dir.__len__(); i++)
        {
            if (allMethodsNames.ToList().Contains(dir[i]))
                methodsNames.Add(dir[i]);
        }

        mainClsInst.gameObject = scr.gameObject;

        gameScriprsEngine.SetVar(null, "export");

        return (mainClsInst, methodsNames);
    }
}

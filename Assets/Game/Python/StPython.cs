using IronPython.Hosting;
using IronPython.Runtime.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gcore
{
    public void debug_log(object msg)
    {
        Debug.Log(msg);
    }
    public T[] array<T>(IronPython.Runtime.List list)
    {

        List<T> result = new List<T>(list.Count);
        foreach (object element in list)
        {
            result.Add((T)element);
        }
        return result.ToArray();
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
        typeof(Int32),
        typeof(Int64),
        typeof(String),
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
        typeof(OnUseComponentScript),
        typeof(ItemStorageComponentScript),
        typeof(UiComponentScript),
        typeof(SavingDataComponentScript),
        typeof(ReferenceComponentScript),
        typeof(UnitDataComponentScript),
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
    static StPythonEngine gameScriptsEngine;
    public static (dynamic instance, List<string> methods) RegisterPythonMonoBehaviourScript(PythonMonoBehaviourScript scr)
    {
        if (gameScriptsEngine == null)
            gameScriptsEngine = CreateGameScriptsEngine();


        gameScriptsEngine.ExecuteFile(scr.scriptPath, true);
        dynamic export = gameScriptsEngine.GetVar("export");

        dynamic mainCls = export();

        dynamic mainClsInst = mainCls();

        dynamic dir = gameScriptsEngine.Execute("dir(export())");
        string[] allMethodsNames = new string[] { "start", "sec_update", "update", "fixed_update", "third_update", "fivesec_update" };
        List<string> methodsNames = new List<string>();
        for (int i = 1; i < dir.__len__(); i++)
        {
            if (allMethodsNames.ToList().Contains(dir[i]))
                methodsNames.Add(dir[i]);
        }

        mainClsInst.gameObject = scr.gameObject;

        gameScriptsEngine.SetVar(mainClsInst, "_");
        for (int i = 0; i < scr.variables.Count; i++)
        {
            gameScriptsEngine.SetVar(scr.variables[i].value, "__");
            gameScriptsEngine.Execute($"_.{scr.variables[i].name} = __");
        }


        gameScriptsEngine.SetVar(null, "export");
        gameScriptsEngine.SetVar(null, "_");
        gameScriptsEngine.SetVar(null, "__");

        return (mainClsInst, methodsNames);
    }
}

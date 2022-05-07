using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Linq;

public class StPythonEngine
{
    ScriptScope scope;
    ScriptEngine eng;

    public StPythonEngine(ScriptEngine eng, ScriptScope scope)
    {
        this.eng = eng;
        this.scope = scope;

        //scope.SetVariable()

        ICollection<string> searchPaths = eng.GetSearchPaths();
        //searchPaths.Add(Application.dataPath);Python

#if UNITY_EDITOR
        searchPaths.Add(Application.dataPath + @"\StreamingAssets\Lib\");
#endif
#if !UNITY_EDITOR
        searchPaths.Add(System.IO.Directory.GetCurrentDirectory() + @"\StreamingAssets\Lib\");
#endif

        eng.SetSearchPaths(searchPaths);

        eng.Execute(@"
from __future__ import nested_scopes
from __future__ import generators
from __future__ import division
from __future__ import absolute_import
from __future__ import with_statement
from __future__ import print_function
from __future__ import unicode_literals
", scope);

    }
    public Action<string> onCatchException;
    public bool ContainsVariable(string name)
    {
        return scope.ContainsVariable(name);
    }
    public void AddSearchPath(string path, bool gameSouce = false)
    {
        if (gameSouce)
            path = Application.streamingAssetsPath + "/GameSource" + path;

        ICollection<string> searchPaths = eng.GetSearchPaths();
        searchPaths.Add(path);
        eng.SetSearchPaths(searchPaths);
        //Log.Ms(Utils.ArrToStr(searchPaths.ToArray()));
    }
    public dynamic GetVar(string name)
    {
        return scope.GetVariable(name);
    }
    public void SetGlobalVar(object obj, string name)
    {
        eng.GetBuiltinModule().SetVariable(name, obj);
    }
    public void SetVar(object obj, string name)
    {
        scope.SetVariable(name, obj);// l
    }
    public T GetVar<T>(string name)
    {
        return scope.GetVariable<T>(name);
    }
    public List<T> GetList<T>(string name)
    {
        IList<object> originalResult = (IList<object>)GetVar(name);
        List<T> typeSafeResult = new List<T>();
        foreach (object element in originalResult)
        {
            typeSafeResult.Add((T)element);
        }
        return typeSafeResult;
    }
    public dynamic Execute(string code)
    {
        try
        {
            return eng.Execute(code, scope);
        }
        catch (Exception ex)
        {
            onCatchException(eng.GetService<ExceptionOperations>().FormatException(ex));
            return null;
        }
    }
    public dynamic Execute<T>(string code)
    {
        try
        {
            return eng.Execute<T>(code, scope);
        }
        catch (Exception ex)
        {
            onCatchException(eng.GetService<ExceptionOperations>().FormatException(ex));
            return null;
        }
    }
    public static string MakeGameSourcePath(string path)
    {
        return Application.streamingAssetsPath + "/GameSource" + path;
    }
    public void ExecuteFile(string path, bool gameSouce = false)
    {
        if(gameSouce)
            path = Application.streamingAssetsPath + "/GameSource" + path;
        try
        {
            eng.ExecuteFile(path, scope);
        }
        catch (Exception ex)
        {
            onCatchException(eng.GetService<ExceptionOperations>().FormatException(ex));
        }
    }

}

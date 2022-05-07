using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Runtime.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public class Gcore
{
    //public Type Vector3 = typeof(Vector3);
    //public Vector3 GVector3(float x, float y, float z)
    //{
    //    return new Vector3(x, y, z);
    //}
    public void writeln(string msg)
    {
        Debug.Log(msg);
    }
}*/
public class CoreCL
{
    public void hi()
    {
        Debug.Log("Hello core!");
    }
    public void write_ln(string msg)
    {
        Console.WriteLine(msg);
    }
}

//[ExecuteInEditMode]
public class TEST_loganc : MonoBehaviour
{
    
    private void Start()
    {
        return;
        var d = DynamicHelpers.GetPythonTypeFromType(typeof(Vector3));

        var en = StPython.CreateEngine();
        en.onCatchException += (msg) => throw new Exception(msg);
        en.SetGlobalVar(new Gcore(), "gcore");
        en.SetGlobalVar(
            d,
            "Vector3"
            );
        //en.SetVar(new CoreCL(), "core");
        //en.Execute("global gcore");_
        //en.Execute("gcore = _gcore");

        //en.Execute("core = _core");

        en.ExecuteFile("/GameApi/game.py", true);
        en.AddSearchPath("/", true);
        en.AddSearchPath("/GameApi/", true);

        en.ExecuteFile("/Test/test_script.py", true);
        Log.Ms(en.GetVar("main")());



        
        //Instantiate(PrefabManager.Instance.GetPrefabByUUID("f20a7075-115c-43ff-962c-6fce03f8a0e6")).transform.parent = transform;
        var eng = Python.CreateEngine();
        var scope = eng.CreateScope();

        ICollection<string> searchPaths = eng.GetSearchPaths();

        //Path to the folder of greeter.py
        searchPaths.Add(Application.dataPath);
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
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

        eng.Execute(@"
def main():
    return str(list(range(10)))#'Hello world! ' + str(1 / 3)
", scope);
        //eng.SetSearchPaths(new string[]{ "./" });

        dynamic mainCls = scope.GetVariable("main")();
        print(mainCls);
    }

    private void Update()
    {
        //Debug.Log(GetComponent<BasicBuildingAnchorManager>().anchors[BuildingAnchorDirection.Forward]);
    }
}
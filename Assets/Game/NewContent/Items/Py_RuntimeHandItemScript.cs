using UnityEngine;

[RequireComponent(typeof(PythonMonoBehaviourScript))]
public class Py_RuntimeHandItemScript : RuntimeHandItemScript
{
    public override void GameStart()
    {
        var ps = GetComponent<PythonMonoBehaviourScript>();
        ps.BindPostInit(() => ps.GetInstance().handItemScript = this);
    }
}

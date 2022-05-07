using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeUnitScript : MonoBehaviour
{
    public RuntimeUnitInfoScript info;

    void Start()
    {
        GameStart();
    }
    void Update()
    {
        GameUpdate();
    }
    private void FixedUpdate()
    {
        GameFixedUpdate();
    }

    public virtual void GameUpdate() { }
    public virtual void GameStart() { }
    public virtual void GameFixedUpdate() { }

}

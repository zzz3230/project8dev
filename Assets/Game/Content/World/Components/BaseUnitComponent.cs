using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitComponent : MonoBehaviour
{
    public virtual void GameStart() { }
    public virtual void GameUpdate() { }
    public virtual void GameFixedUpdate() { }
    public bool isUpdating = true;
    public bool isFixedUpdating = false;

    public void PauseUpdating()
    {
        isUpdating = false;
        isFixedUpdating = false;
        enabled = false;
    }
    public void ResumeUpdating()
    {
        isUpdating = true;
        isFixedUpdating = true;
        enabled = true;
    }

    private void Start()
    {
        GameStart();
    }
    private void Update()
    {
        if(isUpdating)
            GameUpdate();
    }
    private void FixedUpdate()
    {
        if(isFixedUpdating)
            GameFixedUpdate();
    }
}

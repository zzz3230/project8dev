using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnKeyPressComponent : BaseUnitComponent
{
    public KeyCode keyCode { 
        get { return _keyCode; } 
        set { if (value == KeyCode.None) PauseUpdating(); else ResumeUpdating(); _keyCode = value; } 
    }
    [SerializeField] KeyCode _keyCode = KeyCode.None;
    public UnityEvent onKeyPressed;

    public override void GameUpdate()
    {
        if(_keyCode != KeyCode.None)
            if(Input.GetKeyDown(_keyCode))
                onKeyPressed.Invoke();
    }
}

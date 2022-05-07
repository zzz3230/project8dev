using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoaded
{
    public Action<object> loaded {
        get { return _loaded; }
        set { if (tocall) value(self); _loaded = value; } 
    }

    Action<object> _loaded { get; set; }

    object self { get; set; }
    bool tocall { get; set; }
    void Loaded(object self)
    {
        //if(tocall && loaded != null)
        this.self = self;
        if(loaded == null)
        {
            tocall = true;
            return;
        }
        loaded(self);
    }
}
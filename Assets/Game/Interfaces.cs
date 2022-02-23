using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoaded
{
    public Action<object> loaded { get; set; }
}
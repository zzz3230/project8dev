using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameItemInfo
{
    public bool interactive { get { return _interactive; } }
    [SerializeField] bool _interactive;

    public float damageFactor { 
        get { return _damageFactor; } set { _damageFactor = value; } }
    [SerializeField] float _damageFactor;

    public DamageType damageType { 
        get { return _damageType; } set { _damageType = value; } }
    [SerializeField] DamageType _damageType;

    public uint maxStack
    {
        get { return _maxStack; }
        set { _maxStack = value; }
    }
    [SerializeField] uint _maxStack = 16;
}

public class ItemInstanceInfo
{
    public GameItemInfo original { get; }
    public GameObject instance { get; set; }
}
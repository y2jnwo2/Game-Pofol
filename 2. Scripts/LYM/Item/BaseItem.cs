using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BaseItem 
{
    public enum ItemCategory
    {
        WEAPON = 0,
        ARMOUR = 1,
        GOLD = 2,
        BUFF = 3,
        POTION = 4

    }
    [SerializeField]
    private string name;
    [SerializeField]
    private string description;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }
    public string DESCRIPTION
    {
        get { return this.description; }
        set { this.description = value; }
    }
}

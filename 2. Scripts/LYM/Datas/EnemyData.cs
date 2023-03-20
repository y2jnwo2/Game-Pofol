using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float exp;
    public enum monsterType {Ghost, Wolf, Ork};
    public int monAtk;
    public int monDef;
    public float curHp, curMp;
    public float maxHp, maxMp;
    public string[] dropItems = { "Euqip", "Pothion", "Gold",
                                  "Euqip2", "Pothion2", "Gold2",
                                 "Euqip2", "Pothion2", "Gold3" };

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item :ScriptableObject
{
    

    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public string weaponType;
    public enum ItemType
    {
        Equipment,
        Potion,
        Skill
    }

    // ============수정
    // 아이템 설명
    [TextArea]
    public string itemDesc;
    //public SlotToolTip theSlotToolTip;

 
    //public bool Use()
    //{
    //    return false;
    //}
    
}

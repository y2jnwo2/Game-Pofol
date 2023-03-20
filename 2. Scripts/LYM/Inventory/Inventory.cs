using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    
    public static bool inventoryActivated = false;
    
    public GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotParent;

    public GameObject go_DragInven;

    // 슬롯들
    private InventorySlot[] inventorySlots;
    private InventorySlot[] quickSlots;
    public InventorySlot[] equipSlots;
    private bool isNotPut;

    [SerializeField]
    private GameObject go_QuickSlotParent;
    [SerializeField]
    private GameObject go_EquipSlotParent;

    void Start()
    {
       inventorySlots = go_SlotParent.GetComponentsInChildren<InventorySlot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<InventorySlot>();
        equipSlots = go_EquipSlotParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        
        TryOpenInventory();

    }




    // inventoryActivated가 false일때 I누르면 켜지고 true일때 누르면 꺼짐
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
        go_DragInven.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        go_DragInven.SetActive(false);
        
    }
    public void BtnCloseInven()
    {
        go_InventoryBase.SetActive(false);
        go_DragInven.SetActive(false);
        inventoryActivated = false;
    }
    // 아이템 종류별로 비교
    public void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _item.itemType) {
           
            PutSlot(quickSlots, _item, _count);
        }
        if (isNotPut || Item.ItemType.Equipment == _item.itemType) {
            PutSlot(inventorySlots, _item, _count);
        }
        
        if (isNotPut) {
            Debug.Log("공간이 부족합니다");
        }

    }

    public void PutSlot(InventorySlot[] _slots, Item _item, int _count)
    {

        if (Item.ItemType.Equipment != _item.itemType ) {
            for (int i = 0; i < _slots.Length; i++) {
                if (_slots[i].item != null) {
                    if (_slots[i].item.itemName == _item.itemName) {
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }
                }
            }
        }
        if (Item.ItemType.Potion == _item.itemType) {
            for (int i = 3; i < _slots.Length; i++) {
                if (_slots[i].item == null) {
                    {
                        _slots[i].AddItem(_item, _count);
                        isNotPut = false;
                        return;
                    }
                }

            }
        }
       
        else {
            for (int i = 0; i < _slots.Length; i++) {
                if (_slots[i].item == null) {
                    {
                        _slots[i].AddItem(_item, _count);
                        isNotPut = false;
                        return;
                    }
                }

            }
        }
        isNotPut = true;
    }


    public int GetItemCount(string _itemName)
    {
        int temp = SearchSlotItem(inventorySlots, _itemName);
        return temp != 0 ? temp : SearchSlotItem(quickSlots, _itemName);
    }
    private int SearchSlotItem(InventorySlot[] _slots, string _itemName)
    {
        for (int i = 0; i < _slots.Length; i++) {
            if (_slots[i].item != null) {
                if (_itemName == _slots[i].item.itemName)
                    return _slots[i].itemCount;
            }
        }

        return 0;
    }

    public void SetItemCount(string _itemName, int _itemCount)
    {
        if (!ItemCountAdjust(inventorySlots, _itemName, _itemCount))
            ItemCountAdjust(quickSlots, _itemName, _itemCount);
    }

    private bool ItemCountAdjust(InventorySlot[] _slots, string _itemName, int _itemCount)
    {
        for (int i = 0; i < _slots.Length; i++) {
            if (_slots[i].item != null) {
                if (_itemName == _slots[i].item.itemName) {
                    _slots[i].SetSlotCount(-_itemCount);
                    return true;
                }
            }
        }
        return false; // 인벤토리에 없어서 퀵슬롯에서 빼야 됨
    }
}

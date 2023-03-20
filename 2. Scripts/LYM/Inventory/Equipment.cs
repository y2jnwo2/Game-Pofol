using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{


    public static bool EquipActivated = false;

    public GameObject go_EquipBase;
    [SerializeField]
    private GameObject go_SlotParent;

    public GameObject go_DragEquip;

    // 슬롯들
    public InventorySlot[] inventorySlots;
    private bool isNotPut;
    private bool isFull = false; // 인벤토리 퀵슬롯 둘다 꽉 찼는지 판단


    void Start()
    {
        inventorySlots = go_SlotParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {

        TryOpenEquipment();

    }




    // inventoryActivated가 false일때 I누르면 켜지고 true일때 누르면 꺼짐
    private void TryOpenEquipment()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            EquipActivated = !EquipActivated;

            if (EquipActivated)
                OpenEquipment();
            else
                CloseEquipment();
        }
    }

    private void OpenEquipment()
    {
        go_EquipBase.SetActive(true);
        go_DragEquip.SetActive(true);
    }

    private void CloseEquipment()
    {
        go_EquipBase.SetActive(false);

    }
    public void BtnCloseEquip()
    {
        go_EquipBase.SetActive(false);
        EquipActivated = false;
    }
   

}

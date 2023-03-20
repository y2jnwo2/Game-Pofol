using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnlStat : MonoBehaviour
{


    public static bool StatActivated = false;

    public GameObject go_StatBase;




    //void Start()
    //{
    //    inventorySlots = go_SlotParent.GetComponentsInChildren<InventorySlot>();
    //}

    void Update()
    {

        TryOpenStat();

    }




    // inventoryActivated가 false일때 I누르면 켜지고 true일때 누르면 꺼짐
    private void TryOpenStat()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            StatActivated = !StatActivated;

            if (StatActivated)
                OpenStat();
            else
                CloseStat();
        }
    }

    private void OpenStat()
    {
        go_StatBase.SetActive(true);
    }

    private void CloseStat()
    {
        go_StatBase.SetActive(false);

    }
    public void BtnCloseStat()
    {
        go_StatBase.SetActive(false);
        StatActivated = false;
    }


}

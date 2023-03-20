﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;

    [SerializeField]
    private Text text_Preview;
    [SerializeField]
    private Text text_Input;
    [SerializeField]
    private InputField if_text;

    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private LDHNetPlayer thePlayer;


    
    void Update()
    {
        if (activated) {
            if (Input.GetKeyDown(KeyCode.Return))
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();
        }
    }

    // 슬롯 드래그가 끝날때 OnEndDrag에서 호출할것
    public void Call()
    {
        
        go_Base.SetActive(true);
        activated = true;
        if_text.text = "몇개를버리시겠습니까?";
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        DragSlot.instance.SetColor(0);
        go_Base.SetActive(false);
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0);

        int num;
        if (text_Input.text != "") {
            if (CheckNumber(text_Input.text)) {
                num = int.Parse(text_Input.text);
                if (num > DragSlot.instance.dragSlot.itemCount)
                    num = DragSlot.instance.dragSlot.itemCount;
            }
            else
                num = 1;
        }
        else
            num = int.Parse(text_Preview.text);

        StartCoroutine(DropItemCorountine(num));
    }

    IEnumerator DropItemCorountine(int _num, int index = 15)
    {
        for (int i = 0; i < _num; i++) {
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab,
                thePlayer.transform.position + thePlayer.transform.forward,
                Quaternion.identity);
            if (transform.position.y >2f) {
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab,
                thePlayer.transform.position + new Vector3 (0, -2.5f,0)+ thePlayer.transform.forward,
                Quaternion.identity);
            }
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f); 
        }

        DragSlot.instance.dragSlot = null;
        go_Base.SetActive(false);
        activated = false;
    }
    // 텍스트에 문자가 섞여있다면 false를 반환
    private bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < _tempCharArray.Length; i++) {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)
                continue;
            isNumber = false;
        }
        return isNumber;
    }
}
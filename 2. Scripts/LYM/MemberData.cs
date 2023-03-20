using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WJMemberData : MonoBehaviour
{
    private LDHNetPlayer playerData;
    [HideInInspector]
    public string playerName = "";

    public float hpRate;
    //파티원의 이름과 파티원의 HP 정보를 표시하기 위해 텍스트와 이미지 레퍼런스를 받는다.
    public Text txtMemberName;
    public Image imgPlayerInfo;

    void Start()
    {
        playerData = GameObject.FindObjectOfType<LDHNetPlayer>();
    }
    private void Update()
    {
        hpRate = playerData.hpRate;
        //실시간으로 변하는 데이터를 받아와서 바꿔주기 위함
        DisplayPartyMemberData();
    }

    public void DisplayPartyMemberData()
    {
        //리스트 안의 텍스트를 설정한 파티원의 이름으로 바꾼다.
        txtMemberName.text = playerName;
        //리스트 안의 체력바를 현재 체력과 최대 체력의 비율로 표시
        imgPlayerInfo.fillAmount = hpRate;
    }
}
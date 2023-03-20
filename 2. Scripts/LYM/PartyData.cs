using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WJPartyData : MonoBehaviour
{
    [HideInInspector]
    public string partyName = "";
    [HideInInspector]
    public int partyPlayer = 0;
    [HideInInspector]
    public int maxPlayer = 0;
    //파티의 이름과 플레이어의 숫자를 표시할 텍스트 레퍼런스
    public Text txtPartyName;
    public Text txtPlayerInfo;

    public void DisplayPartyData()
    {
        //리스트 안의 텍스트를 설정한 파티 이름으로 바꾼다.
        txtPartyName.text = partyName;
        //리스트 안의 텍스트를 현재 인원과 최대 인원으로 표기
        txtPlayerInfo.text = "( " + partyPlayer.ToString() + " / " + maxPlayer.ToString() + " ) ";
    }
}
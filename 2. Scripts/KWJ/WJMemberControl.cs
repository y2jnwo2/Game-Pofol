using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WJMemberControl : MonoBehaviour
{
    //파티원의 목록이 차일드로 생성될 Parent 객체의 레퍼런스
    public GameObject scrollContents;
    //파티원 수만큼 생성될 프리팹 연결 레퍼런스
    public GameObject memberItem;
    //파티원 상태창에 있는 Text와 Image 연결 레퍼런스
    public Text txtUserName;
    public Image imgplayerHP;

    private PhotonView pv = null;
    //생성된 프리팹을 인스턴스화 시킬 게임 오브젝트 객체
    //이 친구가 없으면 SetParent를 쓸 수가 없음
    private GameObject obj;

    public GameObject player;

    private string playerName;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        scrollContents.GetComponent<RectTransform>().pivot = new Vector2(1.0f, 0.0f);

        playerName = PlayerPrefs.GetString("Select");
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(LocalUISet());
        //생성된 프리팹을 게임오브젝트화 하여 ScrollContents의 자식 객체로 만들어준다.
        obj.transform.SetParent(scrollContents.transform, false);

        player = GameObject.Find(playerName);
    }
    IEnumerator LocalUISet()
    {
        //코드로 프리팹을 Instantiate 한 뒤 SetParent를 실행하면 오류로 위치 설정이 되지 않음.
        //게임오브젝트 변수 하나를 준 뒤 소환된 프리팹을 인스턴스화 된 게임오브젝트로 바꿔주고 위치 세팅을 진행해주면 된다.
        obj = PhotonNetwork.Instantiate("MemberList", Vector3.zero, Quaternion.identity, 0);

        txtUserName = memberItem.GetComponent<Transform>().GetChild(0).GetComponent<Text>();
        imgplayerHP = memberItem.GetComponent<Transform>().GetChild(1).GetComponent<Image>();

        WJMemberData memberData = obj.GetComponent<WJMemberData>();

        memberData.playerName = PhotonNetwork.playerName;

        imgplayerHP.fillAmount = memberData.hpRate;

        memberData.DisplayPartyMemberData();

        yield return null;
    }
    //(구현 예정)
    ////네트워크 플레이어가 연결되었을 때 실행되는 콜백 함수
    //void OnPhotonPlayerConnected()
    //{
    //    for (int i = 1; i < _member.PlayerCount; i++)
    //    {
    //        Debug.Log("OnPhotonPlayerConnected 함수 실행");

    //        memberItems[i].SetActive(true);
    //    }
    //}
    ////네트워크 플레이어가 연결를 끊었을 때 실행되는 콜백 함수
    //void OnPhotonPlayerDisConnected()
    //{
    //    //이 부분에서 네트워크 플레이어의 UI를 삭제
    //}
}
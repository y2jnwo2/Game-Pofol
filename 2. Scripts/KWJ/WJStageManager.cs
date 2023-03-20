using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WJStageManager : MonoBehaviour
{
    //접속한 플레이어의 수를 표시할 Text 연결 레퍼런스
    public Text txtConnect;
    //접속 로그를 표시할 Text 연결 레퍼런스
    public Text txtLogMsg;

    //RPC 호출을 위한 Photon View 연결 레퍼런스
    PhotonView pv;

    //플레이어 생성 위치 저장 레퍼런스
    private Transform[] playerPos;
    //보스 생성 위치 연결 레퍼런스
    private Transform bossSpawnPos;

    private string selectClass;

    //게임 끝
    [SerializeField]
    private bool gameEnd;

    void Awake()
    {
        pv = GetComponent<PhotonView>();

        selectClass = PlayerPrefs.GetString("Select");

        //플레이어 생성 위치
        playerPos = GameObject.Find("PlayerSpawn").GetComponentsInChildren<Transform>();

        //마스터클라이언트와 네트워크 플레이어의 씬을 같은 레벨에 둔다?
        PhotonNetwork.automaticallySyncScene = true;

        StartCoroutine(this.CreatePlayer());
        //포톤 클라우드로부터 네트워크 메시지 수신을 연결
        PhotonNetwork.isMessageQueueRunning = true;
        //룸에 입장한 후 접속자 정보 출력
        GetConnectPlayerCount();

        //보스 생성 위치
        bossSpawnPos = GameObject.Find("BossSpawn").GetComponent<Transform>();

        //포톤 네트워크에 연결이 되었고, 마스터 클라이언트(파티장)인 경우
        if (PhotonNetwork.connected && PhotonNetwork.isMasterClient) {
            //보스를 생성하는 코루틴 함수 호출
            StartCoroutine(this.CreateBoss());
        }
    }
    IEnumerator Start()
    {
        //파티 참여 로그 메시지를 출력할 문자열
        string msg = "\n<color=#00ff00> [" + PhotonNetwork.player.NickName + "] 파티 가입</color>";
        //뒤늦게 들어온 플레이어에게 메시지를 전달.
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        //룸 내의 네트워크 객체 간 통신이 완료될 때까지 대기
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator CreateBoss()
    {
        GameObject Enemy = PhotonNetwork.InstantiateSceneObject("Boss", bossSpawnPos.localPosition, bossSpawnPos.localRotation, 0, null);

        Enemy.name = "Enemy";

        yield return null;
    }
    IEnumerator CreatePlayer()
    {
        Room currRoom = PhotonNetwork.room;

        GameObject player = PhotonNetwork.Instantiate(selectClass, playerPos[currRoom.PlayerCount].position, playerPos[currRoom.PlayerCount].rotation, 0);

        player.name = PlayerPrefs.GetString("Select");

        yield return null;
    }
    //룸 접속자 정보를 조회하는 함수
    void GetConnectPlayerCount()
    {
        Room currRoom = PhotonNetwork.room;

        //현재 룸의 접속자와 최대 접속자의 수를 텍스트로 표시함
        txtConnect.text = currRoom.PlayerCount.ToString() + " / " + currRoom.MaxPlayers.ToString();
    }
    //네트워크에 플레이어가 접속했을 때 호출되는 콜백 함수
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        GetConnectPlayerCount();
    }
    //네트워크 플레이어가 룸을 나가거나 접속이 끊어졌을 경우 호출되는 콜백 함수
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }
    //파티 탈퇴 버튼에 연결될 함수
    public void OnClickExitParty()
    {
        //파티 탈퇴 로그 메시지를 출력할 문자열
        string msg = "\n<color=#ff0000> ["
            + PhotonNetwork.player.NickName +
            "] 파티 탈퇴</color>";

        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
        //플레이어가 파티를 떠나며 생성한 네트워크 객체를 삭제
        PhotonNetwork.LeaveRoom();
    }
    //파티에서 탈퇴했을 때 호출되는 콜백 함수
    void OnLeftRoom()
    {
        SceneManager.LoadScene("scLobby");
    }
    // 포톤 추가
    [PunRPC]
    void LogMsg(string msg)
    {
        //로그 메시지 Text UI에 텍스트를 누적시켜 표시
        txtLogMsg.text = txtLogMsg.text + msg;
    }
}
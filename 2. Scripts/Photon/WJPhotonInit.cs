using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WJPhotonInit : MonoBehaviour
{
    //파일 공유를 위한 버전 정보
    public string version = "Ver 0.1.0";
    //동작에 있어 어떤 부분의 문제가 발생하는지 알기 위해 로그레벨을 Full로 고정
    public PhotonLogLevel LogLevel = PhotonLogLevel.Full;

    //플레이어의 닉네임을 설정하는 인풋 필드
    public InputField userId;
    //플레이어가 입장할 파티의 이름
    public InputField partyName;

    //서버의 목록이 차일드로 생성될 Parent 객체의 레퍼런스(UI 버전에서 사용)
    public GameObject scrollContents;
    //파티 목록만큼 생성될 프리팹 연결 레퍼런스
    public GameObject partyItem;

    void Awake()
    {
        //만약 로비에 들어가 있지 않다면
        if (!PhotonNetwork.connected && !PhotonNetwork.offlineMode)
        {
            PhotonNetwork.ConnectUsingSettings(version);

            //어떤 상황이 발생하는지 로그를 찍어준다.
            PhotonNetwork.logLevel = LogLevel;

            //현재 유저의 이름을 포톤에 설정
            PhotonNetwork.playerName = "User " + Random.Range(1, 999);
        }

        partyName.text = "PARTY #" + Random.Range(1, 999).ToString("000");

        scrollContents.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 1.0f);
    }
    //포톤클라우드에 정상적으로 접속한 후 로비에 입장하면 호출되는 콜백 함수
    void OnJoinedLobby()
    {
        Debug.Log("파티 대기실에 입장했습니다.");
        userId.text = GetUserId();
    }
    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");

        if (string.IsNullOrEmpty(userId))
        {
            userId = PhotonNetwork.playerName;
        }
        return userId;
    }
    //파티에 입장하면 호출되는 콜백 함수
    void OnJoinedRoom()
    {
        Debug.Log("파티에 가입되었습니다.");

        //파티에 들어가면 보스방이 있는 씬으로 넘어가 대기
        LoadBossScene();
    }
    //보스방이 있는 씬으로 이동하는 함수
    void LoadBossScene()
    {
        //씬 전환 동안 포톤 서버로부터 메시지 수신 중단
        PhotonNetwork.isMessageQueueRunning = false;

        SceneManager.LoadScene("scNet");
    }
    //Register 버튼을 클릭 시 호출될 함수
    public void OnClickCreateParty()
    {
        string _partyName = partyName.text;

        //파티 이름이 없거나 NULL일 경우 이름 지정
        if (string.IsNullOrEmpty(partyName.text))
        {
            _partyName = "PARTY #" + Random.Range(1, 999).ToString("000");
        }
        //로컬플레이어(본인)의 이름 설정
        PhotonNetwork.player.NickName = userId.text;

        //생성할 파티의 조건을 설정
        RoomOptions partyOptions = new RoomOptions();
        //파티에 누구나 들어올 수 있도록 열려있음
        partyOptions.IsOpen = true;
        //해당 파티의 목록이 로비에서 보임
        partyOptions.IsVisible = true;
        //파티의 최대 인원은 3명으로 제한
        partyOptions.MaxPlayers = 3;

        //지정한 조건에 맞는 파티를 생성
        PhotonNetwork.CreateRoom(_partyName, partyOptions, TypedLobby.Default);

        //파티 생성 확인을 위한 로그
        Debug.Log("파티 생성 완료");
    }
    //생성된 파티의 목록이 변경되었을 때, 호출되는 콜백 함수
    void OnReceivedRoomListUpdate()
    {
        // 포톤 클라우드 서버에서는 룸 목록의 변경이 발생하면 클라이언트로 룸 목록을 재전송하기
        // 아래 로직이 없으면 다른 클라이언트에서 룸을 나갈때마다 룸 목록이 쌓인다.
        // 룸 목록을 다시 받았을 때 새로 갱신하기 위해 기존에 생성된 RoomItem을 삭제  
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PartyList"))
        {
            Destroy(obj);
        }

        int rowCount = 0;
        //스크롤 영역 초기화
        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        //수신받은 파티 목록 정보로 PartyList 프리팹 객체를 생성
        foreach (RoomInfo _party in PhotonNetwork.GetRoomList())
        {
            //해당 파티가 맞는지 이름 확인
            Debug.Log(_party.Name);
            //파티리스트 프리팹을 동적 생성
            GameObject party = (GameObject)Instantiate(partyItem);
            //파티리스트의 parent를 지정
            party.transform.SetParent(scrollContents.transform, false);

            //생성한 리스트에 정보를 표시하기 위한 정보 전달
            WJPartyData partyData = party.GetComponent<WJPartyData>();
            partyData.partyName = _party.Name;
            partyData.partyPlayer = _party.PlayerCount;
            partyData.maxPlayer = _party.MaxPlayers;

            partyData.DisplayPartyData();
            //파티리스트의 버튼 컴포넌트에 클릭 이벤트를 동적으로 연결
            partyData.GetComponent<Button>().onClick.AddListener(delegate
            {
                OnClickPartyList(partyData.partyName);
                Debug.Log("Party Join " + partyData.partyName);
            });

            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }
    }
    void OnClickPartyList(string partyName)
    {
        //로컬 플레이어의 이름 설정
        PhotonNetwork.player.NickName = userId.text;
        PhotonNetwork.JoinRoom(partyName);
    }
}
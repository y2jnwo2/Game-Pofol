 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class LDHNetPlayer : MonoBehaviour
{
    private static SoundManager theSoundManager;
    private int num;
    private bool isMove = false;

    enum CursorType
    {
        None,
        Attack
    }

    CursorType _cursorType = CursorType.None;

    Texture2D _attackIcon;
    Texture2D _noneIcon;

    bool _pressed = false;
    float _pressedTime = 0;

    public GameObject deadScene;
    private static WJFloatDamage floatDamage;
    // LTM (11-15) ==========================수정 =======================
    private GameObject nearObject;
    public float curExp;
    public bool isTrigger;
    // LYM (11- 17) ==========================인벤토리 돈=======================
    public Text txtGold;
    public GameManager theGameManager;
    // LYM (11-22)===================
    public bool isFired;
    private bool isbossAtk;
    //LYM
    [SerializeField]
    float CamSpeedX;
    [SerializeField]
    float CamSpeedY;
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;

    private Rigidbody rigid;
    Animator anim;

    // =========수정 LYM =========================================
    public Enemy enemy;
    public PlayerData playerData;
    public ItemEffectDatabase recentPlayerData;

    [SerializeField]
    private Inventory theInventory;

    // ===========추가 LYM( 11-14) =======================
    public CapsuleCollider collider;
    public bool ishit;

    //KWJ 추가
    //플레이어의 hp와 mp의 Fill Amount를 조작하기 위함
    public Image hpBar;
    public Image mpBar;
    public Slider expBar;

    //플레이어의 Max 체력
    
    public float maxPlayerHp;
    //현재 체력을 저장할 변수
    public float curPlayerHp;
    //(11/10추가) 플레이어의 MP
    
    public float maxPlayerMp;
    //현재 체력을 저장할 변수
    public float curPlayerMp;
    
    //플레이어 hp, mp의 비율
    public float hpRate;
    private float mpRate;
    // 추가 11-21 ====================
    private float expRate;

    // LYM 추가 변수 2개 (11-14)
    public int attackDamage;
    public int defence;
    
    //힐로 채워줄 회복량
    public float healHp = 20.0f;
    //델리게이트를 이용하여 힐 스킬 구현
    delegate float playerHeal(float _currHp, float _Hp);

    //플레이어 손에 들린 무기
    public GameObject weaPon;
    //스킬 이펙트를 저장할 배열
    public GameObject[] skillEffect;

    //적 무기에 닿았을 때 체력 감소하게
    public GameObject enemyWeapon;
    //에너미 데미지를 외부에서 설정, 에너미 스크립트가 연결되면 외부함수 연결
    public float enemiesDamage;

    //Ray 정보 저장 구조체
    Ray ray;
    //Ray에 맞은 오브젝트 정보를 저장할 구조체
    RaycastHit hitinfo;

    private PhotonView pv = null;
    //자신의 트랜스폼 참조 변수
    [SerializeField]
    private Transform myTr;
    [SerializeField]
    private Transform myTrRot;

    //위치 정보 송수신에 사용할 변수
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    //플레이어 UI를 보여줄 Canvas
    public Canvas hudCanvas;
    //미니맵에 플레이어의 위치를 보여줄 마커
    public Image marker;
    //------------------------------(추가 11.17)---------------------------------------
    //큰 미니맵에 플레이어의 위치를 보여줄 마커
    //public Image b_marker;
    //죽었는지 아닌지 체크
    [SerializeField]
    private bool isDie = false;

    public bool atkCursor;

    private void Awake()
    {
        theSoundManager = SoundManager.instance;
        floatDamage = GetComponent<WJFloatDamage>();
        // 추가 LYM (11-15)===========
        recentPlayerData = GameObject.Find("ItemEffectDatabase").GetComponent<ItemEffectDatabase>();

        anim = GetComponentInChildren<Animator>();
        //_text = GetComponent<Text>();//체력감소 테스트
        rigid = GetComponent<Rigidbody>();
        //최상위 플레이어 객체의 Transform 값을 받아옴 (위치값은 이 오브젝트가 바뀌는 것을 확인)
        myTr = GetComponent<Transform>();
        //차상위 플레이어 객체의 Transform 값을 받아옴 (회전값은 이 오브젝트가 바뀌는 것을 확인)
        //최상위 객체 트랜스폼 기준 Child의 첫번째는 Player이므로 이런식으로 접근한다.
        myTrRot = GetComponent<Transform>().GetChild(0);

        //hp와 mp 이미지 및 exp의 슬라이더를 스크립트로 연결
        hpBar = GameObject.Find("HP").GetComponent<Image>();
        mpBar = GameObject.Find("MP").GetComponent<Image>();
        expBar = GameObject.Find("slEXP").GetComponent<Slider>();
        hpRate = curPlayerHp / maxPlayerHp;
        mpRate = curPlayerMp / maxPlayerMp;
        
        //추가(11.9)
        //LYM
        if (PhotonNetwork.offlineMode)
        {
            enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        }
        // =============테스트중  11-17====================
        theGameManager = FindObjectOfType<GameManager>();
        // 추가 LYM 11-14 플레이어 데이터 초기설정
        //playerData.level = 1;
        //playerData.exp = 0;
        //playerData.atk = 10;
        //playerData.def = 2;
        //playerData.curHp = 100;
        //playerData.maxHp = 100;
        //playerData.curMp = 100;
        //playerData.maxMp = 100;
        //attackDamage = playerData.atk;
        //defence = playerData.def;
        

        playerData.SetDefault();
        curPlayerHp = recentPlayerData.tempHp;
        curPlayerMp = recentPlayerData.tempMp;
        curExp = playerData.exp;
        txtGold.text = string.Format("{0}{1,6}", playerData.Gold, "Gold");
        isTrigger = true;
        //체력 초기화
        //maxPlayerHp = curPlayerHp;
        //maxPlayerMp = curPlayerMp;

        
        // 추가 LYM (11-14)===========================================
        collider = GetComponent<CapsuleCollider>();

        //오프라인 상태일 때만 실행될 구문
        if (PhotonNetwork.offlineMode)
        {
            //씬에 존재하는 PlayerMarker라는 오브젝트를 찾아 이미지 컴포넌트에 접근
            marker = GameObject.Find("PlayerMarker").GetComponent<Image>();
            //b_marker = GameObject.Find("BigMarker").GetComponent<Image>();
            //마커의 캔버스 상 초기 위치값을 세팅
            marker.rectTransform.anchoredPosition = new Vector2(135.0f, 87.0f);
            //b_marker.rectTransform.anchoredPosition = new Vector2(405.0f, 259.0f);
        }

        pv = GetComponent<PhotonView>();
        //포톤뷰 옵저브컴포넌트 속성에 현재 스크립트 연결
        pv.ObservedComponents[0] = this;
        //데이터 전송 타입을 설정(데이터의 변경사항이 발생했을 경우에 전송)
        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        //로컬플레이어라면 카메라 암을 켜준다.
        if (pv.isMine)
        {
            cameraArm.gameObject.SetActive(true);
        }
        //로컬플레이어(내 컴퓨터)가 아니라면 물리력을 받지 않도록 처리
        else
        {
            rigid.isKinematic = true;
        }
        //원격 플레이어의 위치 및 회전값을 처리할 변수
        currPos = myTr.position;
        currRot = myTrRot.rotation;
        theSoundManager.StopBGM();
        theSoundManager.PlayBgm("gameBgm");

        num = 1;
    }
    
       
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        if (pv.isMine)
        {
            CamSpeedX = 5.0f;
            CamSpeedY = 15.0f;

            //hp, mp를 100%로 세팅
            hpBar.material.SetFloat("_FillLevel", hpRate);
            mpBar.material.SetFloat("_FillLevel", mpRate);

            _attackIcon = Resources.Load<Texture2D>("Attack");
            _noneIcon = Resources.Load<Texture2D>("None");
            Debug.Log("마우스 커서");
        }

        
    }
    private void Update()
    {
        UpdateMouseCursor();
        //자신의 캐릭터일 때
        if (pv.isMine)
        {
            //플레이어의 현재 체력이 최대치를 넘어가면 최대체력으로 초기화시켜 줌.
            if (curPlayerHp > maxPlayerHp && recentPlayerData.tempMaxHp <= recentPlayerData.tempHp)
            {
                curPlayerHp = 100.0f;   
            }

            if (recentPlayerData.tempMaxHp >= recentPlayerData.tempHp) {
                if ((recentPlayerData.tempLevel > playerData.level)) {
                     curPlayerHp = maxPlayerHp;
                    playerData.level++;
                } 
            }

            //Hit();

            hpRate = curPlayerHp / maxPlayerHp;
            mpRate = curPlayerMp / maxPlayerMp;
            expRate = curExp / recentPlayerData.needExp[playerData.level];
            //hp, mp를 100%로 세팅
            hpBar.material.SetFloat("_FillLevel", hpRate);
            mpBar.material.SetFloat("_FillLevel", mpRate);
            expBar.value = expRate;

            // 추가 LYM 11-19 ===============================

            recentPlayerData.tempHp = curPlayerHp;
            recentPlayerData.tempMp = curPlayerMp;
        }


        if (num >= 4)//********************************************************사운드
        {
            Debug.Log("인트 줄어라");
            num = 1;
        }

    }
    void FixedUpdate()
    {
        FreezeRotation();
        LookAround();
        Move();
        //추가 LYM 11-15
        Interation();
        // 추가 LYM (11-18)==============
        GetPlayerData();
      
    }
    void LookAround()
    {
        if (pv.isMine)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;
            float x = camAngle.x - mouseDelta.y;
            //마우스 우클릭
            if (Input.GetMouseButton(1))
            {
                // x축과 z축이 제한이 없으면 움직이다뒤집히기 때문에 Clamp문으로 제한을 둠 
                if (x < 180f)
                {
                    x = Mathf.Clamp(x, -1f, 25f);
                }
                else
                {
                    x = Mathf.Clamp(x, 322f, 361f);
                }
                //  x - mouseDelta.y 이거는 한국식 시점방향, x + mouseDelta.y 이거는 아메리카식
                cameraArm.rotation = Quaternion.Euler(x - mouseDelta.y * CamSpeedX, camAngle.y + mouseDelta.x * CamSpeedY, camAngle.z);
            }
        }
    }
    // 실제 이동함수
    void Move()
    {
        if (pv.isMine)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            bool isMove = moveInput.magnitude != 0;

            if (isMove)
            {
                //달리는 애니메이션
                anim.SetBool("Run", true);
                anim.SetBool("Shot", false);
                theSoundManager.Walk();

                // 카메라 방향에서 높이를 제거하고 캐릭터위치에서 정면과 오른쪽 방향을 노말라이즈드로 추출
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;

                // 바라보고 있는방향을 기준으로 이동방향을 구함
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                // 캐릭터가 정면을 보지않고 이동방향으로 봄
                characterBody.forward = moveDir;

                // 실제 이동스피드
                transform.position += Vector3.ClampMagnitude(moveDir, 1f) * Time.deltaTime * 20f;

                //오프라인 상태일 때만 실행될 구문
                if (PhotonNetwork.offlineMode)
                {
                    marker.rectTransform.anchoredPosition = new Vector2((this.transform.position.x * 0.58f) + 135.0f, (this.transform.position.z * 0.6f) + 87.0f);
                    //b_marker.rectTransform.anchoredPosition = new Vector2((this.transform.position.x * 0.58f) + 405.0f, (this.transform.position.z * 0.6f) + 259.0f);
                }
            }
            else if (!isMove)
            {
                anim.SetBool("Run", false);
            }
        }
        else
        {
            //원격 플레이어의 아바타를 수신받은 위치까지 부드럽게 이동 및 회전
            myTr.position = Vector3.Lerp(myTr.position, currPos, Time.deltaTime * 7.0f);
            myTrRot.rotation = Quaternion.Slerp(myTrRot.rotation, currRot, Time.deltaTime * 7.0f);

        }
    }
    // 몬스터라든가 다른 물체랑 물리접촉할때 축뒤틀리는거 보정
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }
    [PunRPC]
    private void Hit()
    {
        if (pv.isMine)
        {
            curPlayerHp -= 15.0f;

            //체력바 연동
            hpBar.material.SetFloat("_FillLevel", hpRate);
            if (maxPlayerHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }
    // 추가 LYM (11-14) ===============================
    public void Damage(int damage)
    {
        //사운드 추가
        // IDamage 인터페이스 구현
        if (!ishit)
            curPlayerHp = defence > damage ? curPlayerHp -= 1 : curPlayerHp -= (damage - defence);
        ishit = true;

        if (curPlayerHp <= 0)
        {
            //플레이어가 죽었을 때 함수
            PlayerDie();
            return;
        }

        int hit = 0;
        hit = defence > damage ? hit = 1 : hit = (damage - defence);

        floatDamage.PlayerDamage(hit);

        if (this.gameObject.name == "Warrior")
        {
            theSoundManager.PlaySfx("hurtWar"+num);
        }
        else if (this.gameObject.name == "Wizard")
        {
            //여기에 법사 피격 사운드 넣으면 됨.
            theSoundManager.PlaySfx("hurtWiz"+num);
        }
        else if (this.gameObject.name == "Archer")
        {
            theSoundManager.PlaySfx("hurtArc"+num);
        }
        num++;
        Invoke(nameof(NoHit), 0.5f);
    }
    // 추가 LYM (11-14) ===============================
    public void NoHit()
    {
        //collider.enabled = false;
        ishit = false;
    }

    void PlayerDie()
    {
        //죽음 확인 부울타입 변수 활성화
        isDie = true;
        //보스가 캐릭터를 파악할 수 없도록 태그를 변경
        gameObject.tag = "Untagged";

        //죽었을 때 패널을 찾아서 켜줘야 한다.
        if (SceneManager.GetActiveScene().name == "scsGame")
        {
            //씬에 있는 캔버스의 자식오브젝트인 pnlDead를 불러온다.
            deadScene = GameObject.Find("Canvas").GetComponent<Transform>().GetChild(4).gameObject;
            //패널을 켜준다.
            deadScene.SetActive(true);

            Destroy(this.gameObject);
        }
    }
    //데이터를 송 수신할 때의 콜백 함수
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보 수신(나)
        if (stream.isWriting) {
            stream.SendNext(myTr.position);
            stream.SendNext(myTrRot.rotation);
        }
        //원격 플레이어의 위치 정보 수신(상대방이 보는 나)
        else {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // 수정  LYM (11-15)========================================
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Melee") {
            switch (col.gameObject.name) {
                case "Lip Lower0 L":
                    col.gameObject.GetComponentInParent<Enemy>().player.GetComponent<LDHNetPlayer>().Damage(5);
                    break;
                case "Melee":
                    col.gameObject.GetComponentInParent<Enemy>().player.GetComponent<LDHNetPlayer>().Damage(10);
                    break;
                case "LeftWeapon":
                    col.gameObject.GetComponentInParent<Enemy>().player.GetComponent<LDHNetPlayer>().Damage(15);
                    break;
                case "RightWeapon":
                    col.gameObject.GetComponentInParent<Enemy>().player.GetComponent<LDHNetPlayer>().Damage(15);
                    break;


            }
        }
        OnAttackCollision();
        // 돈추가 =================================================
        // 돈 부딪히면 인스펙터에서 할당한 텍스트위치에 플레이어 데이타 골드 나타내고 사라지게한다
        if (col.gameObject.tag == "Gold") {
            theSoundManager.PlaySfx("getmoney");
            playerData.Gold += 30;
            txtGold.text = string.Format("{0}{1,6}", playerData.Gold, "Gold");
            col.gameObject.SetActive(false);
        }
        #region 스포너 트리거 부분
        // 스폰 추가
        if (isTrigger == true) {
            switch (col.tag) {
                case "Trigger":
                    theGameManager.SpawnEnemyWolf(0, 1, 1);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger1":
                    theGameManager.SpawnEnemyWolf(0, 2, 2);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger2":
                    theGameManager.SpawnEnemyGhost(1, 1, 3);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;

                case "Trigger3":
                    theGameManager.SpawnEnemyWolf(0, 3, 4);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger4":
                    theGameManager.SpawnEnemyGhost(1, 2, 5);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger5":
                    theGameManager.SpawnEnemyWolf(0, 1, 6);
                    theGameManager.SpawnEnemyGhost(1, 2, 6);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger6":
                    theGameManager.SpawnEnemyWolf(0, 2, 7);
                    theGameManager.SpawnEnemyGhost(1, 2, 7);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger7":
                    theGameManager.SpawnEnemyWolf(0, 3, 8);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger8":
                    theGameManager.SpawnEnemyOrk(2, 1, 9);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger9":
                    theGameManager.SpawnEnemyWolf(0, 3, 10);
                    theGameManager.SpawnEnemyGhost(1, 3,10);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger10":
                    theGameManager.SpawnEnemyGhost(1, 3, 11);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger11":
                    theGameManager.SpawnEnemyGhost(1, 3, 12);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger12":
                    theGameManager.SpawnEnemyWolf(0, 6, 13);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger13":
                    theGameManager.SpawnEnemyGhost(1, 2, 14);
                    theGameManager.SpawnEnemyOrk(2, 1, 14);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
                case "Trigger14":
                    theGameManager.SpawnEnemyOrk(2, 2, 15);
                    Invoke(nameof(Cool), 10f);
                    isTrigger = false;
                    break;
            }

            
        }
        #endregion

        if (!ishit) {
            if (col.tag == "Melee") {
                Damage(50);


            }
        }
    }
    
    public void OnAttackCollision()
    {
        
    }
    public void Cool()
    {
        isTrigger = true;
    }
    // 추가 LYM (11-19) ============================스탯 동기화 완료
    public void GetPlayerData()
    {
        attackDamage = recentPlayerData.GetStatAtk();
        defence = recentPlayerData.GetStatDef();

       
       recentPlayerData.tempHp = curPlayerHp;
       recentPlayerData.tempMp = curPlayerMp;
        
        maxPlayerHp = recentPlayerData.GetStatMaxHp();
        maxPlayerMp = recentPlayerData.GetStatMaxMp();
        
        curExp = recentPlayerData.GetStatExp();

        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shop")
            nearObject = other.gameObject;
    }

    void Interation()
    {
        if (Input.GetKey(KeyCode.E) && nearObject != null)
        {
            if (nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
        }
    }

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30f)) {
            if (hit.collider.tag == "Enemy") {
                if (_cursorType != CursorType.Attack) {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                    atkCursor = true;
                }
            }
            else {
                if (_cursorType != CursorType.None) {
                    Cursor.SetCursor(_noneIcon, new Vector2(_noneIcon.width / 8, 0), CursorMode.Auto);
                    _cursorType = CursorType.None;
                    atkCursor = false;
                }
            }
        }
    }

}
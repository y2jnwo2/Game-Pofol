using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseCtrl : MonoBehaviour
{
    private static SoundManager soundManager;
    PhotonView pv = null;
    //투사체 생성 위치
    private Transform bulletPos;
    //버프 이펙트 생성 위치
    private Transform effectPos;
    //일반 공격 프리팹
    public GameObject bullet;
    //스킬 공격 프리팹
    public GameObject[] SkillBullet;
    //--------------------(추가 11.16)--------------------
    //총알을 저장할 리스트
    public List<GameObject> bulletList = new List<GameObject>();

    private int currIndex = 0;

    Ray ray;
    RaycastHit hitinfo;
    //힐을 하는중인지 아닌지 체크해주는 bool 변수 (체크하지 않으면 2 번씩 들어감)
    private bool isHealing;

    private Collider[] _hit;
    //공격 쿨타임인지 아닌지 체크
    private bool isAtkCool;
    //쿨타임 저장 변수
    public float coolTime;
    [SerializeField]
    private float size;
    //어떤 Layer인지 확인할 LayerMask 변수
    public LayerMask whatIsLayer;

    Animator anim;

    // LYM 수정 (11-21) =======================
    private ItemEffectDatabase theItemEffectDatabase;
    [SerializeField]
    private GameObject particle;

    public LDHNetPlayer player;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        bulletPos = GetComponent<Transform>().GetChild(0).GetComponent<Transform>().GetChild(2).GetComponent<Transform>();
        effectPos = GetComponent<Transform>().GetChild(4).GetComponent<Transform>();
        //초반 쿨타임을 0초로 지정
        coolTime = 0.0f;
        //처음에 공격을 할 수 있어야하기 때문에 true로 초기화
        isAtkCool = true;
        //힐을 사용하였는지 체크
        isHealing = false;

        anim = GetComponent<Animator>();
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        soundManager = SoundManager.instance;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LDHNetPlayer>();
    }
    private void Start()
    {
        for (int i = 0; i < 3; ++i)
        {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            obj.transform.parent = GameObject.Find("Magazine").GetComponent<Transform>();
            obj.name = "Bullet0";
            bulletList.Add(obj);
        }
    }
    void FixedUpdate()
    {
        if (pv.isMine)
        {
            //지정 범위 내에 닿은 콜라이더가 Enemy 태그를 달고 있으면, 공격, 스킬 함수를 실행
            _hit = Physics.OverlapSphere(transform.position, size, whatIsLayer);
            foreach (var num in _hit)
            {
                if (num.tag == "Enemy")
                {
                    Attack();
                }
            }
        }
        coolTime -= Time.deltaTime;

        if (coolTime <= 0.0f)
        {
            isAtkCool = true;
            anim.SetBool("Shot", false);
            coolTime = 0.1f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
    }
    //일반, 스킬 공격
    void Attack()
    {
        if (isAtkCool)
        {
            if ((Input.GetMouseButton(0) && player.atkCursor) || (Input.GetMouseButtonDown(0) && player.atkCursor))
            {
                if (!PhotonNetwork.offlineMode)
                {
                    pv.RPC("NormalAttack", PhotonTargets.All, null);
                }
                else
                {
                    NormalAttack();
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!PhotonNetwork.offlineMode)
                {
                    pv.RPC("Skill", PhotonTargets.All, null);
                }
                else
                {
                    Skill();
                }
            }
        }
    }
    [PunRPC]
    public void NormalAttack()
    {
        if (this.gameObject.name == "Warrior")
        {
            soundManager.PlaySfx("warAtk");
        }
        else if (this.gameObject.name == "Wizard")
        {
            soundManager.PlaySfx("wizAtk");
        }
        else if (this.gameObject.name == "Archer")
        {
            soundManager.PlaySfx("arcAtk");
        }

        if (bulletList[currIndex].gameObject.activeSelf)
        {
            return;
        }
        bulletList[currIndex].transform.position = bulletPos.position;
        bulletList[currIndex].transform.rotation = bulletPos.rotation;

        bulletList[currIndex].gameObject.SetActive(true);

        if (currIndex >= 2)
        {
            currIndex = 0;
        }
        else
        {
            currIndex++;
        }

        anim.SetBool("Shot", true);

        isAtkCool = false;
        coolTime = 1.0f;
    }
    //스킬 공격
    [PunRPC]
    public void Skill()
    {
        if (this.GetComponent<LDHNetPlayer>().curPlayerMp > 0)
        {
            #region 전사
            if (this.gameObject.name == "Warrior")
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    soundManager.PlaySfx("warSkill1");
                    anim.SetBool("Shot", true);
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[0], bulletPos.position, bulletPos.rotation);
                        skill.name = "Bullet1";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("War_Earthquake", bulletPos.position, bulletPos.rotation, 0);
                        skill.name = "Bullet1";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    this.GetComponent<LDHNetPlayer>().curPlayerMp -= 15.0f;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    soundManager.PlaySfx("warSkill2");
                    anim.SetBool("Shot", true);
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[1], bulletPos.position, bulletPos.rotation);
                        skill.name = "Bullet2";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("War_DustEx", bulletPos.position, bulletPos.rotation, 0);
                        skill.name = "Bullet2";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    this.GetComponent<LDHNetPlayer>().curPlayerMp -= 15.0f;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    soundManager.PlaySfx("warSkill3");
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[2], transform.position, transform.rotation);
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("War_StrBuff", transform.position, transform.rotation, 0);
                        StartCoroutine(DestroyEffect(skill));
                    }
                    Buff();
                }
            }
            #endregion
            #region 법사
            //법사의 2,3번 스킬은 자기 자신에게서 이펙트가 생성됨.
            else if (this.gameObject.name == "Wizard")
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    soundManager.PlaySfx("wizSkill1");
                    anim.SetBool("Shot", true);
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[0], bulletPos.position, bulletPos.rotation);
                        skill.name = "Bullet1";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("Wiz_MultipleMissile", bulletPos.position, bulletPos.rotation, 0);
                        skill.name = "Bullet1";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    this.GetComponent<LDHNetPlayer>().curPlayerMp -= 15.0f;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    soundManager.PlaySfx("wizSkill2");
                    anim.SetBool("Shot", true);
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[1], new Vector3(transform.position.x,0.3f,transform.position.z), transform.rotation);
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("Wiz_Teleport", transform.position, transform.rotation, 0);
                        StartCoroutine(DestroyEffect(skill));
                    }
                    Teleport();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    soundManager.PlaySfx("wizSkill3");
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[2], new Vector3(transform.position.x, 0.3f, transform.position.z), transform.rotation);
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("Wiz_Heal", transform.position, transform.rotation, 0);
                        StartCoroutine(DestroyEffect(skill));
                    }
                    Heal();
                    isHealing = false;
                }
            }
            #endregion
            #region 궁수
            else if (this.gameObject.name == "Archer")
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    soundManager.PlaySfx("arcSkill1");
                    anim.SetBool("Shot", true);
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[0], bulletPos.position, bulletPos.rotation);
                        skill.name = "Bullet1";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("Arc_FireArrow", bulletPos.position, bulletPos.rotation, 0);
                        skill.name = "Bullet1";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    this.GetComponent<LDHNetPlayer>().curPlayerMp -= 15.0f;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    soundManager.PlaySfx("arcSkill2");
                    anim.SetBool("Shot", true);
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[1], bulletPos.position, bulletPos.rotation);
                        skill.name = "Bullet2";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("Arc_MultiArrow", bulletPos.position, bulletPos.rotation, 0);
                        skill.name = "Bullet2";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    this.GetComponent<LDHNetPlayer>().curPlayerMp -= 15.0f;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    anim.SetBool("Shot", true);
                    soundManager.PlaySfx("arcSkill3");
                    if (PhotonNetwork.offlineMode)
                    {
                        GameObject skill = Instantiate<GameObject>(SkillBullet[2], bulletPos.position, bulletPos.rotation);
                        skill.name = "Bullet3";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    else
                    {
                        GameObject skill = PhotonNetwork.Instantiate("Arc_IceArrow", bulletPos.position, bulletPos.rotation, 0);
                        skill.name = "Bullet3";
                        StartCoroutine(DestroyEffect(skill));
                    }
                    this.GetComponent<LDHNetPlayer>().curPlayerMp -= 15.0f;
                }
            }
            #endregion
            isAtkCool = false;
            coolTime = 1.5f;
        }
    }
    void Teleport()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 30.0f, Color.red);

        if (Physics.Raycast(ray, out hitinfo))
        {
            if (Vector3.Distance(hitinfo.point, transform.position) <= 1.0f)
            {
                return;
            }
            //현 위치에서 RayCastHit된 위치까지의 거리를 계산
            Vector3 dir = new Vector3(hitinfo.point.x - transform.position.x, 0.0f, hitinfo.point.z - transform.position.z);

            //캐릭터가 사라졌다 나타나는 효과를 위해 잠깐 껐다가 켜줌
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //현 위치에서 목표 지점까지 이동시킴.
            this.transform.Translate(dir);

            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<LDHNetPlayer>().curPlayerMp -= 25.0f;
        }
    }
    void Heal()
    {
        //플레이어들을 배열에 저장
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (!isHealing)
            {
                //각 오브젝트의 스크립트에 접근 후, 현재 hp를 20 올려주고, mp를 25 낮춤
                player.GetComponent<LDHNetPlayer>().curPlayerHp += 20.0f;
                player.GetComponent<LDHNetPlayer>().curPlayerMp -= 25.0f;
                isHealing = true;
            }
        }
    }
    void Buff()
    {
        gameObject.GetComponent<LDHNetPlayer>().curPlayerMp -= 20.0f;
        theItemEffectDatabase.tempAtk += 2;
    }

    IEnumerator DestroyEffect(GameObject col)
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(col);
    }
}
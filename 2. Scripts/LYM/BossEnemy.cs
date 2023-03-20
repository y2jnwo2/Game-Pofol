using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
//애니메이션 클립을 저장할 클래스 
public class Anim
{
    public AnimationClip idle;
    public AnimationClip stand;
    public AnimationClip walk;
    public AnimationClip smash;
    public AnimationClip tail;
    public AnimationClip dash;
    public AnimationClip fly;
    public AnimationClip flyAtk;
    public AnimationClip fire;
    public AnimationClip finsh;
    public AnimationClip onHit;
    public AnimationClip die;

}

public class BossEnemy : MonoBehaviour, IDamage
{
    // 프리팹 불뿜는거 (준비중)
    public GameObject BreathFire;

    // 플레이어 향해 머리 돌리는 속도
    Vector3 lookVec;
    // 보스패턴중 어디에 내려찍을지 
    Vector3 tauntVec;

    // 플레이어와 보스의 거리를 재는데 쓸 변수
    public float dist1;
    //private Animation _anim;
    //public Anim anims;
    public Animator anim;
    //  플레이어 바라보는지 조건
    public bool isLook;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public bool isTargetSolo;
    public bool isStart;
    public bool isDamage;

    // public int maxHealth;
    public int enemyHealth;
    public int defence;
    public Transform target;
    public Transform myTr;
    //public Transform landTr;
    //public Transform flyTr;

    private Rigidbody rigid;
    // 공격할때 나왔다 사라질 부위의 콜라이더 (배열로 해야할지도?)
    [SerializeField]
    private BoxCollider attackArea;
    [SerializeField]
    private CapsuleCollider attackFly;
    private CapsuleCollider attackTail;
    // private MeshRenderer meshs;  일단 색바뀌는건 보류
    private NavMeshAgent navMesh;
    // private Animator anim;
    private GameObject[] players;

    private CapsuleCollider capCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        attackArea = GetComponentInChildren<BoxCollider>();
        attackFly = GameObject.Find("MeleeFly").GetComponentInChildren<CapsuleCollider>();
        attackTail = GameObject.Find("Bip01 Tail").GetComponent<CapsuleCollider>();
        // meshs = GetComponent<MeshRenderer>();
        navMesh = GetComponent<NavMeshAgent>();
        //_anim = GetComponentInChildren<Animation>();

        // target = GameObject.Find(PlayerPrefs.GetString("Select")).GetComponent<LDHNetPlayer>().transform;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<LDHNetPlayer>().transform;
        anim = GetComponentInChildren<Animator>();

        //StartCoroutine(Targeting());
    }

    IEnumerator Start()
    {


        anim.SetBool("isStart", true);
        rigid.mass = 3000.0f;
        navMesh.enabled = false;
        yield return new WaitForSeconds(4.5f);

        //yield return new WaitForSeconds(5f);
        navMesh.enabled = true;
        StartCoroutine(Targeting());
        //navMesh.SetDestination(landTr.position);
        yield return null;
    }
    void FixedUpdate()
    {
       
        
    }

    void Update()
    {

        // 죽으면 모든 코루틴 정지
        if (isDead)
        {
            StopAllCoroutines();
            anim.SetBool("isDie", true);
            Destroy(gameObject);
            return;
        }
        // 플레이어 인식하면 플레이어 쪽으로 움직이기
        if (isLook)
        {
            // 플레이어 입력값으로 위치 예측
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;   // 5f 이부분이 예측정도 조절
            transform.LookAt(target.position + lookVec);
        }
        else if (!isLook && isStart && !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fix_idle") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fix_fly attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Fix_walk"))
        {
            navMesh.SetDestination(myTr.forward);
        }

    }


    // 보스 패턴 나누기
    IEnumerator Pattern()
    {
        yield return new WaitForSeconds(0.3f);
        navMesh.enabled = true;
        navMesh.isStopped = false;
        attackFly.enabled = false;
        anim.SetBool("isWalk", true);
        anim.SetBool("isflyAtk", false);
        anim.SetBool("isDash", false);
        anim.SetBool("isSmash", false);
        anim.SetBool("isTail", false);
        anim.SetBool("isFire", false);
        float ranVec = Random.Range(-5.0f, 5.0f);
        navMesh.SetDestination(new Vector3((transform.position.x + ranVec), 0, (transform.position.z + ranVec)));
        yield return new WaitForSeconds(0.3f);


        int ranSel = Random.Range(0, 16);

        switch (ranSel)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                StartCoroutine(Smash());
                break;
            case 5:
            case 6:
            case 7:
                StartCoroutine(Dash());
                break;
            case 8:
            case 9:
            case 10:
                StartCoroutine(TailAtk());
                break;
            case 11:
            case 12:
            case 13:
                StartCoroutine(JumpAtk());
                break;
            case 14:
            case 15:
                StartCoroutine(FireBreath());
                break;

        }
    }


    // 보스 패턴전 플레이어 타겟
    IEnumerator Targeting()
    {
        yield return new WaitForSeconds(0.5f);
        rigid.mass = 1.0f;
        rigid.isKinematic = true;
        rigid.constraints = RigidbodyConstraints.FreezePositionY;
        anim.SetBool("isStart", false);
        if (!isDead)
        {

            int ranNum = Random.Range(0, 10);


            players = GameObject.FindGameObjectsWithTag("Player");

            if (ranNum >= 4)
                CloseTarget();
            else
                LongTarget();

            // 가장 가까운적 찾는거랑 그냥 아무적 쫓는거랑 경우를 6:4로 나누겠음 
            //플레이어가 있을경우 



            yield return new WaitForSeconds(0.3f);

        }




    }


    void CloseTarget()
    {
        // 처음 몬스터가 타겟을잡을때까지 계속 돌아야된다

        // 자신과 가장 가까운 플레이어 찾음
        if (players.Length != 0)
        {

            dist1 = (target.position - myTr.position).sqrMagnitude;
            foreach (GameObject _players in players)
            {
                if ((_players.transform.position - myTr.position).sqrMagnitude <= dist1)
                {

                    target = _players.transform;
                    dist1 = (target.position - myTr.position).sqrMagnitude;


                    float targetRadius = 10f;
                    float targetRange = 10f;

                    RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
                    if (rayHits.Length > 0 && !isAttack)
                    {
                        navMesh.enabled = true;
                        isLook = true;


                        StartCoroutine(Pattern());

                    }
                    else if (rayHits.Length == 0)
                        StartCoroutine(Targeting());
                }



            }
        }

    }

    void LongTarget()
    {
        float targetRadius = 50f;
        float targetRange = 0f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {


            int ranNum = Random.Range(0, rayHits.Length);

            target = players[ranNum].transform;


            isLook = true;
            navMesh.enabled = true;

            StartCoroutine(Pattern());

        }
        else if (rayHits.Length == 0)
            StartCoroutine(Targeting());

    }

    IEnumerator Smash()
    {
        yield return new WaitForSeconds(0.5f);

        navMesh.SetDestination(new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z));
        anim.SetBool("isDash", true);
        navMesh.speed = 25f;
        yield return new WaitForSeconds(1.5f);
        isAttack = true;
        RaycastHit hitInfo;
        Vector3 dir = (target.transform.position - myTr.transform.position).normalized  ;
        if (isAttack == true)
        {
            if (Physics.Raycast(myTr.transform.position, dir, out hitInfo, 15.0f))
            {
                navMesh.SetDestination(target.transform.position);
                anim.SetBool("isSmash", true);
                attackArea.enabled = true;
                target.GetComponent<LDHNetPlayer>().Damage(30);
                yield return new WaitForSeconds(1.5f);
                isAttack = false;
                StartCoroutine(Pattern());
            }
            else
                StartCoroutine(Smash());
        }

    }

    IEnumerator Dash()
    {


        float targetRange = 50f;
        float targetRadius = 50f;
        isLook = true;
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits != null)
        {


            navMesh.acceleration = 720.0f;
            // yield return new WaitForSeconds(1f);
            rigid.AddForce(transform.forward * 40, ForceMode.Acceleration);
            rigid.AddForce(transform.right * 40, ForceMode.Impulse);
            navMesh.SetDestination(target.position);
            anim.SetBool("isDash", true);
            anim.SetBool("isWalk", false);
            attackArea.enabled = true;
            yield return new WaitForSeconds(4f);
            navMesh.SetDestination(target.position);
            rigid.velocity = Vector3.zero;


        }
        anim.SetBool("isDash", false);
        StartCoroutine(Targeting());
    }

    IEnumerator TailAtk()
    {
        navMesh.SetDestination(new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z));
        anim.SetBool("isDash", true);
        navMesh.speed = 25f;
        yield return new WaitForSeconds(1.5f);
        isAttack = true;
        RaycastHit hitInfo;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        if (isAttack == true)
        {
            if (Physics.Raycast(myTr.transform.position, dir, out hitInfo, 30.0f))
            {
                navMesh.SetDestination(target.transform.position);
                anim.SetBool("isTail", true);
                attackTail.enabled = true;
                target.GetComponent<LDHNetPlayer>().Damage(30);
                yield return new WaitForSeconds(1.5f);
                isAttack = false;
                StartCoroutine(Pattern());
            }
            else
                StartCoroutine(TailAtk());
        }
    }

    IEnumerator JumpAtk()
    {
        anim.SetBool("isflyAtk", true);

        yield return new WaitForSeconds(0.1f);

        // tauntVec = target.position + lookVec;
        isLook = false;



        // BoxCollider.enabled = falsse;
        navMesh.enabled = false;
        myTr.transform.position += new Vector3(0, 8f, 0);



        //attackFly.enabled = true;
        target.GetComponent<LDHNetPlayer>().Damage(40);
        yield return new WaitForSeconds(1.0f);

        // attackFly.enabled = false;

        isLook = true;
        // BoxCollider.enabled = true;
        myTr.transform.position -= new Vector3(0, 8f, 0);
        StartCoroutine(Targeting());
        anim.SetBool("isflyAtk", false);
    }


    IEnumerator FireBreath()
    {
        yield return new WaitForSeconds(0.1f);
        BreathFire.SetActive(true);
        isLook = false;
        anim.SetBool("isFire", true);
        navMesh.isStopped = true;
        yield return new WaitForSeconds(6.5f);
        BreathFire.SetActive(false);
        StartCoroutine(Targeting());

    }

    //IEnumerator FinshAtk()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    StartCoroutine(Pattern());

    //}





    public void Fired()
    {
        if (players.Length != 0)
        {

            foreach (GameObject _players in players)
            {
                _players.GetComponent<LDHNetPlayer>().Damage(2);
                _players.GetComponent<LDHNetPlayer>().ishit = true;
                _players.GetComponent<LDHNetPlayer>().isFired = true;
                _players.GetComponent<LDHNetPlayer>().Damage(1);
            }
        }
    }
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position);
        //Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, -10f), 10f);

        Gizmos.color = Color.red;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        // Gizmos.DrawWireSphere(transform.position, 50f);
        Gizmos.DrawRay(myTr.transform.position + new Vector3 (0, 3f, 0), dir * 15f);
    }

    IEnumerator Damage(int damage)
    {
        //랜덤으로 아이템 떨구게 하기위한 변수

        // IDamage 인터페이스 구현
        enemyHealth = defence > damage ? enemyHealth -= 5 : enemyHealth -= (damage - defence);

        int hit = (damage - defence);
        //floatDamage.EnemyDamage(hit);

        Debug.Log(" 몬스터 체력이달았습니다. " + hit);

        if (enemyHealth <= 0)
        {
            anim.SetBool("isDie", true);
            yield return new WaitForSeconds(2.0f);

            isDead = true;
            //사운드 추가
            //gameObject.SetActive(false);



        }
        yield return new WaitForSeconds(1.0f);
        isDamage = false;
    }

    void IDamage.Damage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter(Collider other)
    {
        float targetRadius = 25f;
        float targetRange = 0f;
        if (other.gameObject.tag == "Weapon" && !isDamage)
        {
            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
            foreach (GameObject _players in players)
            {
                int attackDamage = _players.GetComponent<LDHNetPlayer>().attackDamage;
                isDamage = true;
                StartCoroutine(Damage(attackDamage));
            }
        }
    }
}










//==========
//IEnumerator MissileShot()
//    {
//        anim.SetTrigger("doShot");
//        yield return new WaitForSeconds(0.2f);
//        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
//        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
//        bossMissileA.target = target;

//        yield return new WaitForSeconds(0.3f);
//        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
//        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
//        bossMissileB.target = target;

//        yield return new WaitForSeconds(2f);

//        StartCoroutine(Think());
//    }

//    IEnumerator RockShot()
//    {
//        isLook = false;
//        anim.SetTrigger("doBigShot");
//        Instantiate(bullet, transform.position, transform.rotation);
//        yield return new WaitForSeconds(3f);

//        isLook = true;
//        StartCoroutine(Think());
//    }

//    IEnumerator Taunt()
//    {
//        tauntVec = target.position + lookVec;

//        isLook = false;
//        nav.isStopped = false;
//        boxCollider.enabled = false;
//        anim.SetTrigger("doTaunt");
//        yield return new WaitForSeconds(1.5f);
//        meleeArea.enabled = true;

//        yield return new WaitForSeconds(0.5f);
//        meleeArea.enabled = false;

//        yield return new WaitForSeconds(1f);
//        isLook = true;
//        nav.isStopped = true;
//        boxCollider.enabled = true;
//        StartCoroutine(Think());
//    }

//=======================================
//public class BossRock : Bullet
//{ 
//    Rigidbody rigid;
//    float angularPower = 2;
//    float scaleValue = 0.1f;
//    bool isShoot;

//    void Awake()
//    {
//        rigid = GetComponent<Rigidbody>();
//        StartCoroutine(GainPowerTimer());
//        StartCoroutine(GainPower());
//    }

//    IEnumerator GainPowerTimer()
//    {
//        yield return new WaitForSeconds(2.2F);
//        isShoot = true;
//    }

//    IEnumerator GainPower()
//    {
//        while (!isShoot) {
//            angularPower += 0.02f;
//            scaleValue += 0.005f;
//            transform.localScale = Vector3.one * scaleValue;
//            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
//            rigid.velocity = transform.forward  * angularPower * 1.3f;
//            yield return null;
//        }
//    }
//}
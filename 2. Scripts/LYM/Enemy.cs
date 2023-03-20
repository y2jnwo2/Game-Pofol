using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamage, IExp
{
    private static SoundManager sound;

    private WJFloatDamage floatDamage;
    private static SoundManager theSoundManager;
    public enum Type { WOLF, GHOST, ORK };
    public Type enemyType;
    public ItemEffectDatabase theItemEffectDatabase;
    public float enemyHealth;
    public int attackDamage;
    public int defence;

    // 쫓는 대상 (ex: Player)
    public GameObject target;
    // 소환된 위치의 저장
    public Transform orginPos;
    // 애니메이션에 쓰일 변수
    public bool isChase;
    public bool isAttack;
    public bool isDead;

    // 플레이어를 인식하는 거리
    [SerializeField]
    float scanRange = 15f;

    // 몬스터 공격 사정거리
    [SerializeField]
    float attackRange = 5f;

    public Rigidbody rigid;
    public BoxCollider boxCollider;

    public NavMeshAgent nav;
    public Animator anim;

    // Nav에 쓰일 목적지 변수
    public Vector3 destPos;

    public GameObject player;
    
    [SerializeField]
    private Image marker;
    [SerializeField]
    private Transform myTr;

    
    public int exp;  
    
    public GameManager theGameManager;
    // 몬스터별 드랍아이템
    public int dropItem_Num;
    public int dropGold_Num;
    [SerializeField]
    private bool isDamage;

    void Awake()
    {
        floatDamage = GetComponent<WJFloatDamage>();
        theSoundManager = SoundManager.instance;

        isDamage = false;

        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        attackDamage = GetComponent<Enemy>().attackDamage;
        boxCollider = GameObject.FindGameObjectWithTag("Melee").GetComponent<BoxCollider>();
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        theGameManager = FindObjectOfType<GameManager>();
        
        marker = GameObject.Find("EnemyMarker").GetComponent<Image>();
        myTr = GetComponent<Transform>();

        sound = SoundManager.instance;

    }

    void OnEnable()
    {
        switch (enemyType) {
            case Enemy.Type.WOLF:
                enemyHealth = 50;
                break;
            case Enemy.Type.GHOST:
                enemyHealth = 100;
                break;
            case Enemy.Type.ORK:
                enemyHealth = 200;
                break;
        }
        isDamage = false;
    }
   
    void Start()
    {
        marker.rectTransform.anchoredPosition = new Vector2(134.1f, 122.8f);
        
        marker.enabled = false;
        nav.enabled = true;

        nav.isStopped = true;
    }
    void FixedUpdate()
    {
        
        UpdateIdle();
        UpdateMoving();
        player = GameObject.Find(PlayerPrefs.GetString("Select"));
    }
  
    void Update()
    {
    }

    void UpdateIdle()
    {
        if (player == null) {
            marker.enabled = false;
            return;
        }
        if (player != null) {
            float distance = (player.transform.position - transform.position).magnitude;
           

            nav.isStopped = true;

            if (distance <= scanRange) {
                marker.enabled = true;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                target = player;
                UpdateMoving();
                return;
            }
        }
    }

    void UpdateMoving()
    {
        if (target != null) {
            destPos = target.transform.position;
            float distance = (destPos - transform.position).magnitude;

            Vector3 dir = destPos - transform.position;
            if (dir.magnitude >= 40f) {
                UpdateIdle();
                marker.enabled = false;
                anim.SetFloat("locomotion", 1f);
            }
            else if (dir.magnitude >= 0.1f) {
                NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();
                nav.isStopped = false;
                nma.SetDestination(destPos);
                if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolf");
                }
                anim.SetFloat("locomotion", 10f);
                nma.speed = 10;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

                if (distance <= attackRange) {
                    nma.SetDestination(transform.position);
                    UpdateAttack();
                    return;
                }
            }
        }
    }
    void UpdateAttack()
    {
        if (target != null) {
            if(this.gameObject.name == "Enemy")
            {
                sound.PlaySfx("ghostatk");
            }
            else if(this.gameObject.name == "Devils DEMO")
            {
                sound.PlaySfx("monsteratk");
            }
            else if(this.gameObject.name == "Wolf Realistic")
            {
                sound.PlaySfx("wolfatk");
            }
            Vector3 dir = target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 10 * Time.deltaTime);
            anim.SetTrigger("DoAttack");
        }
    }

    IEnumerator Damage(int damage)
    {

        exp = GetComponent<Enemy>().exp;
        enemyHealth = defence > damage ? enemyHealth -= 5 : enemyHealth -= (damage - defence);

        int hit = 0;
        hit = defence > damage ? hit = 5 : hit = (damage - defence);
        floatDamage.EnemyDamage(hit);

        Debug.Log(" ?????? ????????????????. " + hit);

        if (enemyHealth <= 0) {
            anim.SetBool("isDie", true);
            yield return new WaitForSeconds(2.0f);
            Exp(exp);
            //?????? ????
            gameObject.SetActive(false);
            #region 드랍 골드, 아이템
            theGameManager.DropGold(0, dropGold_Num);
            for (int i = 0; i < dropItem_Num; i++) {
                int ranNum = Random.Range(0, 21);

                switch (ranNum) {
                    case 0:
                        theGameManager.DropSword1(1);
                        break;
                    case 1:
                        theGameManager.DropSword2(1);
                        break;
                    case 2:
                        theGameManager.DropSword2(1);
                        break;
                    case 3:
                        theGameManager.DropSword2(1);
                        break;
                    case 4:
                        theGameManager.DropStaff1(1);
                        break;
                    case 5:
                        theGameManager.DropStaff2(1);
                        break;
                    case 6:
                        theGameManager.DropStaff2(1);
                        break;
                    case 7:
                        theGameManager.DropStaff2(1);
                        break;
                    case 8:
                        theGameManager.DropBow1(1);
                        break;
                    case 9:
                        theGameManager.DropBow2(1);
                        break;
                    case 10:
                        theGameManager.DropBow2(1);
                        break;
                    case 11:
                        theGameManager.DropBow2(1);
                        break;
                    case 12:
                        theGameManager.DropHelmet(1);
                        break;
                    case 13:
                        theGameManager.DropHelmet(1);
                        break;
                    case 14:
                        theGameManager.DropHelmet(1);
                        break;
                    case 15:
                        theGameManager.DropArmor(1);
                        break;
                    case 16:
                        theGameManager.DropArmor(1);
                        break;
                    case 17:
                        theGameManager.DropArmor(1);
                        break;
                    case 18:
                        theGameManager.DropPantst(1);
                        break;
                    case 19:
                        theGameManager.DropPantst(1);
                        break;
                    case 20:
                        theGameManager.DropPantst(1);
                        break;
                }
            }
            #endregion
        }
        yield return new WaitForSeconds(1.0f);

        isDamage = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon" && !isDamage) {
            int attackDamage = player.GetComponent<LDHNetPlayer>().attackDamage;
            isDamage = true;
            StartCoroutine(Damage(attackDamage));
        }
    }

    void IDamage.Damage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Exp(int exp)
    {

        theItemEffectDatabase.tempExp += exp;

    }
}
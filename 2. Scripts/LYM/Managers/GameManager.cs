using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 풀링에 인자로 쓰기위해 문자열로 변수할당
    public string[] enemyObj;
    public string[] goldObj;
    public string[] itemObj;
    public ObjectManager objectManager;
    public GameObject player;
    //생성된 프리팹의 이름 변경을 위해 프리팹을 인스턴스화 해 줄 게임오브젝트
    [SerializeField]
    private GameObject chgPlayerName;

    // 몬스터 스폰받을 장소 배열로 선언
    public Transform[] mobSpawner;
    //플레이어가 생성될 위치
    public Transform playerSummoner;
    public Transform itemSpawner;
    public string selClass;

    

    void Awake()
    {
        //// 선언한 문자열에 이름으로 에너미오브젝트 초기화
        enemyObj = new string[] { "EnemyWolf", "EnemyGhost", "EnemyOrk" };
        goldObj = new string[] { "ItemCoin" };
        itemObj = new string[] { "Sword1","Sword2","Staff1","Staff2","Bow1","Bow2", "Helmet", "Armor", "Pants" };
        mobSpawner = GameObject.FindGameObjectWithTag("Spawn").GetComponentsInChildren<Transform>();

         playerSummoner = GameObject.Find("PlaySummon").GetComponent<Transform>();

        selClass = PlayerPrefs.GetString("Select");

        player = Resources.Load<GameObject>(selClass);
    }
    void Start()
    {
        SummonPlayer();
        itemSpawner = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    
    public void SpawnEnemyWolf(int type, int num, int index)
    {
        // 오브젝트풀링 3)  오브젝트풀을 받아와서 사용
        float ranVec = Random.Range(-2.0f, 3.0f);

        if (num <= objectManager.enemyWolf.Length) {
            for (int i = 0; i < num; i++) {
                GameObject enemyWolf = objectManager.MakeObj(enemyObj[type]);
                enemyWolf.transform.position = mobSpawner[index].transform.position + new Vector3(ranVec+i, 0, ranVec-i);
            }
        }
        // 몬스터 배치
        //if (!enemyWolf.activeInHierarchy) {
        //    enemyWolf.transform.position = mobSpawner[2].transform.position;
        //    enemyWolf.transform.position = mobSpawner[2].transform.position;
        //}


        //    enemyWolf.transform.position = mobSpawner[1].transform.position;
        //else if (!enemyWolf.activeInHierarchy || !enemyGhost.activeInHierarchy || !enemyOrk.activeInHierarchy)
        //    enemyGhost.transform.position = mobSpawner[2].transform.position;

        //Rigidbody rigid = enemy.GetComponent<Rigidbody>();
        //Enemy enemyLogic = enemy.GetComponent<Enemy>();

        // 나중에 플레이어가 GameManager클래스 받아야할 때 사용
        //enemyLogic.player = player;
        //enemyLogic.objectManager = objectManager;
    }
    public void SpawnEnemyGhost(int type, int num, int index)
    {

        float ranVec = Random.Range(-2.0f, 3.0f);

        if (num <= objectManager.enemyGhost.Length) {
            for (int i = 0; i < num; i++) {
                GameObject enemyGhost = objectManager.MakeObj(enemyObj[type]);
                enemyGhost.transform.position = mobSpawner[index].transform.position + new Vector3(ranVec+i, 0, ranVec-i);
            }
        }
    }

    public void SpawnEnemyOrk(int type, int num, int index)
    {
        float ranVec = Random.Range(-2.0f, 3.0f);

        if (num <= objectManager.enemyOrk.Length) {
            for (int i = 0; i < num; i++) {
                GameObject enemyOrk = objectManager.MakeObj(enemyObj[type]);
                enemyOrk.transform.position = mobSpawner[index].transform.position + new Vector3(ranVec+i, 0, ranVec-i);
            }
        }
    }
    #region 드랍골드, 아이템 부분
    public void DropGold(int type, int num)
    {

        if (num <= objectManager.itemCoin.Length) {
            for (int i = 0; i < num; i++) {
                GameObject gold = objectManager.MakeObj(goldObj[type]);
                gold.transform.position = itemSpawner.transform.position + new Vector3(0, 0,  1.5f + i);
            }
        }
    }
    public void DropSword1(int  num)
    {
        if (num <= objectManager.Sword1.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[0]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 2.5f, 1.5f + i);
            }
        }
    }
    public void DropSword2(int num)
    {
        if (num <= objectManager.Sword2.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[1]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 2.5f, 1.5f + i);
            }
        }
    }
    public void DropStaff1(int num)
    {
        if (num <= objectManager.Staff1.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[2]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 1.5f, 1.5f + i);
            }
        }
    }
    public void DropStaff2(int num)
    {
        if (num <= objectManager.Staff2.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[3]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 1.5f, 1.5f + i);
            }
        }
    }
    public void DropBow1(int num)
    {
        if (num <= objectManager.Bow1.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[4]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 0, 1.5f + i);
            }
        }
    }
    public void DropBow2(int num)
    {
        if (num <= objectManager.Bow2.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[5]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 0, 1.5f + i);
            }
        }
    }
    public void DropHelmet(int num)
    {
        if (num <= objectManager.Helmet.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[6]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, 0, 1.5f + i);
            }
        }
    }
    public void DropArmor(int num)
    {
        if (num <= objectManager.Armor.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[7]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, -2f, 1f + i);
            }
        }
    }
    public void DropPantst(int num)
    {
        if (num <= objectManager.Pants.Length) {
            for (int i = 0; i < num; i++) {
                GameObject equip = objectManager.MakeObj(itemObj[8]);
                equip.transform.position = itemSpawner.transform.position + new Vector3(0, -1f, 1f + i);
            }
        }
    }
    #endregion
    void SummonPlayer()
    {
        //안 켜주면 캐릭터가 안 움직임 ㅜㅜ
        PhotonNetwork.offlineMode = true;

        chgPlayerName = Instantiate(player, playerSummoner.transform.position, Quaternion.identity);

        //플레이어 프리팹 하위 객체로 카메라가 있으므로, 태그로 찾아서 직접 켜준다.
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(true);

        chgPlayerName.name = selClass;
    }
}
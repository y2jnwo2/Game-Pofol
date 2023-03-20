using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // 몬스터 프리팹들을 선언
    public GameObject enemyGhostPrefab;
    public GameObject enemyWolfPrefab;
    public GameObject enemyOrkPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemSword1Prefab;
    public GameObject itemSword2Prefab;
    public GameObject itemStaff1Prefab;
    public GameObject itemStaff2Prefab;
    public GameObject itemBow1Prefab;
    public GameObject itemBow2Prefab;
    public GameObject itemHelmetPrefab;
    public GameObject itemArmorPrefab;
    public GameObject itemPantsPrefab;

    // 몇개를 쓰고 재사용할지 배열로 선언
    public GameObject[] enemyGhost;
  public GameObject[] enemyWolf;
  public GameObject[] enemyOrk;
  public GameObject[] itemCoin;
  public GameObject[] Helmet;
  public GameObject[] Armor;
  public GameObject[] Pants;
  public GameObject[] Sword1;
  public GameObject[] Sword2;
  public GameObject[] Staff1;
  public GameObject[] Staff2;
  public GameObject[] Bow1;
  public GameObject[] Bow2;

    // MakeObj함수와 Getpool함수에서 스위치문의 몬스터를 한개의 변수로 받아 코드를 단축하기 위함
    GameObject[] targetPool;
    GameObject ObjEnemy;

    void Awake()
    {
        enemyGhost = new GameObject[10];
        enemyWolf = new GameObject[14];
        enemyOrk = new GameObject[4];

        itemCoin = new GameObject[20];
        Helmet = new GameObject[3];
        Armor = new GameObject[3];
        Pants = new GameObject[3];
        Sword1 = new GameObject[3];
        Sword2 = new GameObject[1];
        Staff1 = new GameObject[3];
        Staff2 = new GameObject[1];
        Bow1 = new GameObject[3];
        Bow2 = new GameObject[1];


        Generate();

        
    }

    // 오브젝트풀링 1) 먼저 씬에서 사용할 몬스터를 쫙 다 소환하고 SetActive를 꺼놓음
    void Generate()
    {

            // Enemy
            for (int index = 0; index < enemyWolf.Length; index++) {
            enemyWolf[index] = Instantiate(enemyWolfPrefab);
            enemyWolf[index].gameObject.name = "Wolf Realistic";
            enemyWolf[index].transform.parent = GameObject.Find("EnemyPool").GetComponent<Transform>();
            enemyWolf[index].SetActive(false);
        }
        for (int index = 0; index < enemyGhost.Length; index++) {
            enemyGhost[index] = Instantiate(enemyGhostPrefab);
            enemyGhost[index].gameObject.name = "Enemy";
            enemyGhost[index].transform.parent = GameObject.Find("EnemyPool").GetComponent<Transform>();
            enemyGhost[index].SetActive(false);
        }
        
        for (int index = 0; index < enemyOrk.Length; index++) {
            enemyOrk[index] = Instantiate(enemyOrkPrefab);
            enemyOrk[index].gameObject.name = "Devils DEMO";
            enemyOrk[index].transform.parent = GameObject.Find("EnemyPool").GetComponent<Transform>();
            enemyOrk[index].SetActive(false);
        }

        // Item
        for (int index = 0; index < itemCoin.Length; index++) {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            itemCoin[index].SetActive(false);
        }
        for (int index = 0; index < Helmet.Length; index++) {
            Helmet[index] = Instantiate(itemHelmetPrefab);
            Helmet[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Helmet[index].SetActive(false);
        }
        for (int index = 0; index < Armor.Length; index++) {
            Armor[index] = Instantiate(itemArmorPrefab);
            Armor[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Armor[index].SetActive(false);
        }
        for (int index = 0; index < Pants.Length; index++) {
            Pants[index] = Instantiate(itemPantsPrefab);
            Pants[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Pants[index].SetActive(false);
        }
        for (int index = 0; index < Sword1.Length; index++) {
            Sword1[index] = Instantiate(itemSword1Prefab);
            Sword1[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Sword1[index].SetActive(false);
        }
        for (int index = 0; index < Sword2.Length; index++) {
            Sword2[index] = Instantiate(itemSword2Prefab);
            Sword2[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Sword2[index].SetActive(false);
        }
        for (int index = 0; index < Staff1.Length; index++) {
            Staff1[index] = Instantiate(itemStaff1Prefab);
            Staff1[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Staff1[index].SetActive(false);
        }
        for (int index = 0; index < Staff2.Length; index++) {
            Staff2[index] = Instantiate(itemStaff2Prefab);
            Staff2[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Staff2[index].SetActive(false);
        }
        for (int index = 0; index < Bow1.Length; index++) {
            Bow1[index] = Instantiate(itemBow1Prefab);
            Bow1[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Bow1[index].SetActive(false);
        }
        for (int index = 0; index < Bow2.Length; index++) {
            Bow2[index] = Instantiate(itemBow2Prefab);
            Bow2[index].transform.parent = GameObject.Find("ItemPool").GetComponent<Transform>();
            Bow2[index].SetActive(false);
        }
       
    }
    // 오브젝트 풀링 2) 꺼놓았던 몬스터를 어떠한 매개체를 만났을대 키기위한 함수
    public GameObject MakeObj(string type)
    {
        

        switch (type) {
            case "EnemyWolf":
                targetPool = enemyWolf;
                break;
            case "EnemyGhost":
                targetPool = enemyGhost;
                break;
            case "EnemyOrk":
                targetPool = enemyOrk;
                break;

            case "ItemCoin":
                targetPool = itemCoin;
                break;

            case "Sword1":
                targetPool = Sword1;
                break;
            case "Sword2":
                targetPool = Sword2;
                break;
            case "Staff1":
                targetPool = Staff1;
                break;
            case "Staff2":
                targetPool = Staff2;
                break;
            case "Bow1":
                targetPool = Bow1;
                break;
            case "Bow2":
                targetPool = Bow2;
                break;
            case "Helmet":
                targetPool = Helmet;
                break;
            case "Armor":
                targetPool = Armor;
                break;
            case "Pants":
                targetPool = Pants;
                break;
            
        }
        for (int index = 0; index < targetPool.Length; index++) {
            // 만약에 에너미고스트가 비활성화되어있다면

            if (!targetPool[index].activeSelf) {
                // 가져오기전에 활성화해서 가져온다
                targetPool[index].SetActive(true);

                return targetPool[index];
            }
            
        }
        return null;
    }
    
    

    // 나중에 에너미스크립트 Die 짤때 Destroy함수말고 SetActive(false)로 해야함
    // 회전값이상하면 transform.rotation = Quaternion.identuty;로 기본값 SetActive(false) 밑에 주자
}

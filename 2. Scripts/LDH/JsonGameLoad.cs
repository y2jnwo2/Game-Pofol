using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class JsonGameLoad : MonoBehaviour
{
    public GameObject pnl_choice;
    public GameObject pnl_Dead;
    public PlayerData playerData = new PlayerData();
    public Transform tr;
    string[] dataArray = new string[3];
    public GameObject objname;
    string pn;
    private static SoundManager soundManager;
    LDHNetPlayer ldh;

    private GameObject obj;
    InventorySlot slot;


    private void Awake()
    {
        //playerData = Resources.Load<PlayerData>("PlayerData.");
        //Debug.Log(Resources.Load<Object>("Resources/JsonData/0playerData.json").ToString());

        for (int i = 0; i <= 2; i++)
        {
            Resources.Load<TextAsset>("JsonData/" + i + 1 + "playerData");
        }
        pn = playerData.player.ToString();


        //playerData = 
    }
    private void Start()
    {
        soundManager = SoundManager.instance;

    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "scsGame")
        {
            tr = GameObject.FindGameObjectWithTag("Start").GetComponent<Transform>();

            //slot = GameObject.FindWithTag("Slot").GetComponent<InventorySlot>();
            ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();
        }
    }


    public void Reload()
    {
        string jsonData1 = File.ReadAllText(Application.dataPath + "/Resources/JsonData/" + "0playerData.json");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData1);

        obj = Resources.Load<GameObject>(PlayerPrefs.GetString("Select"));
        objname = Instantiate(obj, playerData.nowPlayerl, tr.rotation);
        objname.name = playerData.player.ToString();

        GameObject panel = GameObject.Find("Canvas").GetComponent<Transform>().GetChild(4).gameObject;

        panel.SetActive(false);

        ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();

        ldh.gameObject.name = PlayerPrefs.GetString("Select");
        ldh.curPlayerHp = playerData.maxHp;
        ldh.maxPlayerHp = playerData.maxHp;
        ldh.curPlayerMp = playerData.curMp;
        ldh.maxPlayerMp = playerData.maxMp;
        ldh.attackDamage = playerData.atk;
        ldh.defence = playerData.def;
        //ldh.txtGold = playerData.Gold;
        ldh.transform.position = playerData.nowPlayerl;
    }





    public void LoadPlayerDataFromJson1()
    {
        soundManager.PlaySfx("Load");
        Debug.Log("이거 실행되냐 ");
        //SceneManager.LoadScene(3);

        pnl_choice.SetActive(false);

        string jsonData1 = File.ReadAllText(Application.dataPath + "/Resources/JsonData/" + "1playerData.json");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData1);
        Debug.Log(playerData.player);

        obj = Resources.Load<GameObject>(playerData.player.ToString());
        objname = Instantiate(obj, playerData.nowPlayerl, tr.rotation);
        objname.name = playerData.player.ToString();

        ldh.gameObject.name = playerData.player;
        ldh.curPlayerHp = playerData.curHp;
        ldh.maxPlayerHp = playerData.maxHp;
        ldh.curPlayerMp = playerData.curMp;
        ldh.maxPlayerMp = playerData.maxMp;
        ldh.attackDamage = playerData.atk;
        ldh.defence = playerData.def;
        //ldh.txtGold = playerData.Gold;
        ldh.transform.position = playerData.nowPlayerl;
        //playerData.quickSlotNumber = slot.quickSlotNumber;

        //// 획득한 아이템
        // slot.item.name = playerData.item.name;
        //// 획득한 아이템 갯수
        // slot.itemCount=playerData.itemCount;
        //slot.itemIcon = playerData.itemIcon;

        //// 같은 아이템일 경우 나타낼 수량과 이미지
        // slot.textCount=playerData.textCount;
        //slot.countImage=playerData.countImage;

        // slot.theWeaponManager=playerData.theWeaponManager;

        // slot.theItemEffectDatabase=playerData.theItemEffectDatabase;

        // slot.toolTip=playerData.toolTip;
        ////private Rect baseRect; // Inventory_Base 이미지 Rect정보 써야함
        // slot.theInputNumber=playerData.theInputNumber;
        // slot.baseRect=playerData.baseRect;  // Inventory_Base 의 영역
        // slot.quickSlotBaseRect=playerData.quickSlotBaseRect; // 퀵슬롯의 영역.
        // slot.equipRect=playerData.equipRect;

        // slot.isQuickSlot=playerData.isQuickSlot; // 슬롯이 퀵슬롯인지 아닌지 판단


    }
    public void LoadPlayerDataFromJson2()
    {
        soundManager.PlaySfx("Load");
        pnl_choice.SetActive(false);

        string jsonData1 = File.ReadAllText(Application.dataPath + "/Resources/JsonData/" + "2playerData.json");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData1);
        Debug.Log(playerData.player);

        obj = Resources.Load<GameObject>(playerData.player.ToString());
        objname = Instantiate(obj, playerData.nowPlayerl, tr.rotation);
        objname.name = playerData.player.ToString();

        ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();

        ldh.gameObject.name = playerData.player;
        ldh.curPlayerHp = playerData.curHp;
        ldh.maxPlayerHp = playerData.maxHp;
        ldh.curPlayerMp = playerData.curMp;
        ldh.maxPlayerMp = playerData.maxMp;
        ldh.attackDamage = playerData.atk;
        ldh.defence = playerData.def;
        //ldh.txtGold = playerData.Gold;
        ldh.transform.position = playerData.nowPlayerl;


    }
    public void LoadPlayerDataFromJson3()
    {
        soundManager.PlaySfx("Load");
        pnl_choice.SetActive(false);

        string jsonData1 = File.ReadAllText(Application.dataPath + "/Resources/JsonData/" + "3playerData.json");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData1);
        Debug.Log(playerData.player);

        obj = Resources.Load<GameObject>(playerData.player.ToString());
        objname = Instantiate(obj, playerData.nowPlayerl, tr.rotation);
        objname.name = playerData.player.ToString();

        ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();

        ldh.gameObject.name = playerData.player;
        ldh.curPlayerHp = playerData.curHp;
        ldh.maxPlayerHp = playerData.maxHp;
        ldh.curPlayerMp = playerData.curMp;
        ldh.maxPlayerMp = playerData.maxMp;
        ldh.attackDamage = playerData.atk;
        ldh.defence = playerData.def;
        //ldh.txtGold = playerData.Gold;
        ldh.transform.position = playerData.nowPlayerl;


    }

    //public void Deldata1()
    //{

    //    string jsonData = File.ReadAllText(path[0]);
    //    System.IO.File.Delete(jsonData);
    //}
    //public void Deldata2()
    //{
    //    string jsonData = File.ReadAllText(path[1]);
    //    System.IO.File.Delete(jsonData);
    //}
    //public void Deldata3()
    //{
    //    string jsonData = File.ReadAllText(path[2]);
    //    System.IO.File.Delete(jsonData);
    //}
}
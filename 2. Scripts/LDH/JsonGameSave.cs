using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class JsonGameSave : MonoBehaviour
{
    private static SoundManager soundManager;
    public ScreenShot Screen;
    public PlayerData playerData = new PlayerData();
    string jsonData;
    public string[] path = new string[3];
    public LDHNetPlayer ldh;
    public InventorySlot slot;

    private void Awake()
    {

    }

    private void Start()
    {
        Screen = GetComponent<ScreenShot>();
        ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();
        slot = GameObject.Find("pnlInventory").transform.Find("Inventory_Base").transform.Find("GridLayout").GetComponentInChildren<InventorySlot>();
        soundManager = SoundManager.instance;
    }

    private void FixedUpdate()
    {

    }
    //[ContextMenu("To Json Data")]
    public void SavePlayerDataToJson1()
    {

        soundManager.PlaySfx("save");
        playerData.player = ldh.gameObject.name;
        playerData.curHp = ldh.curPlayerHp;
        playerData.maxHp = ldh.maxPlayerHp;
        playerData.curMp = ldh.curPlayerHp;
        playerData.maxMp = ldh.maxPlayerMp;
        playerData.atk = ldh.attackDamage;
        playerData.def = ldh.defence;
        playerData.nowPlayerl = ldh.transform.position;



        playerData.date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        jsonData = JsonUtility.ToJson(playerData);
        path[0] = Path.Combine(Application.dataPath + @"/Resources/JsonData", "1playerData.json");
        Debug.Log(path[0]);
        System.IO.File.Delete(Application.dataPath + @"/Resources/JsonData/1playerData.json");
        File.WriteAllText(path[0], jsonData);
        Screen.ScreenShotSave(1);
        Debug.Log(path[0]);
    }
    public void SavePlayerDataToJson2()
    {
        soundManager.PlaySfx("save");
        ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();

        playerData.player = ldh.gameObject.name;
        playerData.curHp = ldh.curPlayerHp;
        playerData.maxHp = ldh.maxPlayerHp;
        playerData.curMp = ldh.curPlayerHp;
        playerData.maxMp = ldh.maxPlayerMp;
        playerData.atk = ldh.attackDamage;
        playerData.def = ldh.defence;
        // playerData.Gold = ldh.txtGold;
        playerData.nowPlayerl = ldh.transform.position;
        playerData.date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        jsonData = JsonUtility.ToJson(playerData);
        path[0] = Path.Combine(Application.dataPath + @"/Resources/JsonData", "2playerData.json");
        Debug.Log(path[0]);
        System.IO.File.Delete(Application.dataPath + @"/Resources/JsonData/2playerData.json");
        File.WriteAllText(path[0], jsonData);
        Screen.ScreenShotSave(2);
        Debug.Log(path[0]);
    }
    public void SavePlayerDataToJson3()
    {
        soundManager.PlaySfx("save");
        ldh = GameObject.FindWithTag("Player").GetComponent<LDHNetPlayer>();

        playerData.player = ldh.gameObject.name;
        playerData.curHp = ldh.curPlayerHp;
        playerData.maxHp = ldh.maxPlayerHp;
        playerData.curMp = ldh.curPlayerHp;
        playerData.maxMp = ldh.maxPlayerMp;
        playerData.atk = ldh.attackDamage;
        playerData.def = ldh.defence;
        playerData.nowPlayerl = ldh.transform.position;
        playerData.date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        jsonData = JsonUtility.ToJson(playerData);
        path[0] = Path.Combine(Application.dataPath + @"/Resources/JsonData", "3playerData.json");
        Debug.Log(path[0]);
        System.IO.File.Delete(Application.dataPath + @"/Resources/JsonData/3playerData.json");
        File.WriteAllText(path[0], jsonData);
        Screen.ScreenShotSave(3);
        Debug.Log(path[0]);
    }
}

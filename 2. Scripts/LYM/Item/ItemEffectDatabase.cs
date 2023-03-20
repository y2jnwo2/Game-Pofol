 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect 
{
    // 키값으로 아이템 이름을 쓸것이다
    public string itemName;
    // 툹팁용 선언
    [Tooltip ("curHp, curMp, Atk, 만 가능함")]
    // 부위
    public string[] part;
    // 수치
    public int[] num;
}
public class ItemEffectDatabase : MonoBehaviour
{
    public static ItemEffectDatabase itemEffectDatabase;

    [SerializeField]
    private ItemEffect[] itemEffects;
    [SerializeField]
    private const string HP = "curHp", MP = "curMp", ATK = "Atk", EXP = "Exp";
    // 스테이터스 불러와야한다
    public PlayerData theplayerStatus;
    //[SerializeField]
    //private WeaponManager theWeaponManager;
    [SerializeField]
    private SlotToolTip theSlotToolTip;
    [SerializeField]
    private QuickSlotController theQuickSlotController;

    public LDHNetPlayer player;
    public int tempLevel;
    public float[] needExp;
    public float tempHp;
    public float tempMp;
    public float tempMaxHp;
    public float tempMaxMp;
    public int tempAtk;
    public int tempDef;
    public int tempExp;
   

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
            if (itemEffectDatabase ==null) {
            itemEffectDatabase = this;
        }
            else if (itemEffectDatabase !=this) {
            Destroy(gameObject);
        }
        tempLevel = 1;
        needExp = new float[13];
        needExp[0] = 0f;
        needExp[1] = 3.0f;
        needExp[2] = 6.0f;
        needExp[3] = 10.0f;
        needExp[4] = 15.0f;
        needExp[5] = 30.0f;
        needExp[6] = 40.0f;
        needExp[7] = 50.0f;
        needExp[8] = 70.0f;
        needExp[9] = 100.0f;
        needExp[10] = 150.0f;
        needExp[11] = 200.0f;
        needExp[12] = 300.0f;

        tempHp = 100f;
        tempMp = 100f;
        tempMaxHp = 100f;
        tempMaxMp = 100f;
        tempAtk = 10;
        tempDef = 2;
        tempExp = 0;
    }

    void Start()
    {
        theSlotToolTip = GameObject.FindObjectOfType<SlotToolTip>();
        theQuickSlotController = GameObject.FindObjectOfType<QuickSlotController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LDHNetPlayer>();
    }

    void Update()
    {

        if (tempExp >= needExp[tempLevel]) {
            tempLevel++;
            theplayerStatus.level++;
            theplayerStatus.statPoint += 3;

            player.curPlayerHp = player.maxPlayerHp;
            player.curPlayerMp = player.maxPlayerMp;

        }
        
    }
    // InventorySlot클래스에서 할일이지만 Find를 줄이기 위해 여기로 거쳐서 호출
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }
    // 사실 근데 함수의 함수의 호출 이것도 좋은 방법은 아니라고 한다;;
    public void HideTip()
    {
        theSlotToolTip.HideToolTip();
    }

    public float UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Potion) {
            for (int x = 0; x < itemEffects.Length; x++) {
                if (itemEffects[x].itemName == _item.itemName) {

                    for (int y = 0; y < itemEffects[x].part.Length; y++) {
                        switch (itemEffects[x].part[y]) {
                            case HP:
                                tempHp = IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case MP:
                               tempMp = IncreaseMP(itemEffects[x].num[y]);
                                break;
                            case ATK:
                                tempAtk = IncreaseATK(itemEffects[x].num[y]);
                               break;
                            case EXP:
                                tempExp = (int)IncreaseEXP(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("적절한 물약이 없다HP ,HP, MP,ATK ");
                                break;
                        }
                    }
                   return 0;
                }
            }
            Debug.Log("아이템이름이 일치안함");
        }
        return 0;
    }
    public void IsActivatedQuickSlot(int _num)
    {
        theQuickSlotController.IsActivatedQuickSlot(_num);
    }

    public bool GetIsCoolTime()
    {
        return theQuickSlotController.GetIsCoolTime();
    }

    public float GetStatHp()
    {
        return tempHp;
    }

    public float GetStatMp()
    {
        return tempMp;
    }

    public float GetStatMaxHp()
    {
        return tempMaxHp;
    }

    public float GetStatMaxMp()
    {
        return tempMaxMp;
    }
    public int GetStatAtk()
    {
        return tempAtk;
    }
    public int GetStatDef()
    {
        return tempDef;
    }

    public float GetStatExp()
    {
        return tempExp;
    }

    public float GetStatGold()
    {
        return theplayerStatus.Gold;
    }




    public float IncreaseHP(int _count)
    {
        if (player.curPlayerHp+ _count < player.maxPlayerHp)
            player.curPlayerHp += _count;
        else
            player.curPlayerHp = player.maxPlayerHp;

        return player.curPlayerHp;
    }

    public float IncreaseMP(int _count)
    {
        if (player.curPlayerMp + _count < player.maxPlayerMp)
            player.curPlayerMp += _count;
        else
            player.curPlayerMp = player.maxPlayerMp;

        return player.curPlayerMp;
    }

    public int IncreaseATK(int _count = 10)
    {
        player.attackDamage += _count;

        return player.attackDamage;
    }

    public int DecreaseAtk( int _count = 20)
    {

        player.attackDamage -= _count;

        
        return  tempAtk = player.attackDamage;
             
    }

    public float IncreaseEXP(int _count)
    {
        player.curExp += _count;
        return player.curExp;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    private static SoundManager sound;
    [SerializeField] private InventorySlot[] quickSlots;  // 퀵슬롯들 (5개)
    [SerializeField] private InventorySlot[] quickSlots2;  // 퀵슬롯들 (4개)
    [SerializeField] private Transform tf_parent;  // 퀵슬롯들의 부모 오브젝트 content 할당
    [SerializeField] private Transform tf_parent2;  // 퀵슬롯들의 부모 오브젝트 content 할당

    private int selectedSlot;  // 선택된 퀵슬롯의 인덱스 (0~8)
    [SerializeField] private GameObject go_SelectedImage;  // 선택된 퀵슬롯 이미지

    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private ItemEffectDatabase theItemEffectDatabase;
    [SerializeField]
    private Image[] image_CoolTime;
    [SerializeField]
    private float coolTime;
    private float currentCoolTime;
    private bool isCoolTime;

    public LDHNetPlayer player;
    public BaseCtrl _base;

    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<InventorySlot>();
        quickSlots2 = tf_parent2.GetComponentsInChildren<InventorySlot>();
        selectedSlot = 0;

        theWeaponManager = FindObjectOfType<WeaponManager>();
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LDHNetPlayer>();
        _base = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCtrl>();
        sound = SoundManager.instance;
    }
    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
    }
    private void TryInputNumber()
    {
        if (!isCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangeSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangeSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangeSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
    }
    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);
        Execute();
    }
    private void SelectedSlot(int _num)
    {
        // 선택된 슬롯
        selectedSlot = _num;

        // 선택된 슬롯으로 이미지 이동
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }
    private void Execute()
    {
        if (quickSlots[selectedSlot].item != null) {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment) {
                player.recentPlayerData.tempDef += 10;
            }
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Potion)
                StartCoroutine(EatPotion()); //StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
            //sound.PlaySfx("useitem");
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Skill) { 
                _base.Skill();
                CoolTimeReset();
            }
                
        }
        else {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
        }
    }
    public void IsActivatedQuickSlot(int _num)
    {
        //if (selectedSlot == _num  ) {
        //    Execute();
        //    return;
        //}
        if (DragSlot.instance != null) {
            if (DragSlot.instance.dragSlot != null) {
                if (DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot) {
                    //Execute();
                    return;
                }
            }
        }
        
    }
    public IEnumerator EatPotion()
    {
        // 포션 빠는데는 쿨타임을 뺐다 퀵슬롯 4 5
        //CoolTimeReset();
        if (quickSlots[selectedSlot].item.itemName != "ATKPotion") { 
        theItemEffectDatabase.UseItem(quickSlots[selectedSlot].item);
        sound.PlaySfx("useItem");
        quickSlots[selectedSlot].SetSlotCount(-1);
    }
        else 
        theItemEffectDatabase.UseItem(quickSlots[selectedSlot].item);
        sound.PlaySfx("useItem");
        quickSlots[selectedSlot].SetSlotCount(-1);
        yield return new WaitForSeconds(20.0f);
        theItemEffectDatabase.DecreaseAtk();

    }
    private void CoolTimeReset()
    {
        currentCoolTime = coolTime;
        isCoolTime = true;

    }
    private void CoolTimeCalc()
    {
        if (isCoolTime) {
            currentCoolTime -= Time.deltaTime;  // 1 초에 1 씩 감소

            for (int i = 0; i < image_CoolTime.Length; i++)
                image_CoolTime[i].fillAmount = currentCoolTime / coolTime;

            if (currentCoolTime <= 0)
                isCoolTime = false;
        }
    }
    public bool GetIsCoolTime()
    {
        return isCoolTime;
    }
}

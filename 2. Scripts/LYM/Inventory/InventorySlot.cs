using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,IPointerEnterHandler, IPointerExitHandler 
{
    private static SoundManager sound;
    // 획득한 아이템
    public Item item;
    // 획득한 아이템 갯수
    public int itemCount;
    public Image itemIcon;

    // 같은 아이템일 경우 나타낼 수량과 이미지
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private GameObject countImage;

    private WeaponManager theWeaponManager;

    private ItemEffectDatabase theItemEffectDatabase;
    public Inventory inven;

    public SlotToolTip toolTip;
    //private Rect baseRect; // Inventory_Base 이미지 Rect정보 써야함
    private InputNumber theInputNumber;
    [SerializeField] private RectTransform baseRect;  // Inventory_Base 의 영역
    [SerializeField] RectTransform quickSlotBaseRect; // 퀵슬롯의 영역.
    [SerializeField] private RectTransform equipRect;

    [SerializeField]
    private  bool isQuickSlot; // 슬롯이 퀵슬롯인지 아닌지 판단
    [SerializeField]
    private int quickSlotNumber;

    void Start()
    {
         theWeaponManager = FindObjectOfType<WeaponManager>();
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        toolTip = FindObjectOfType<SlotToolTip>();
        theInputNumber = FindObjectOfType<InputNumber>();
        inven = FindObjectOfType<Inventory>();
        sound = SoundManager.instance;
    }
    
    private void SetColor(float _alpha)
    {
        Color color = itemIcon.color;
        color.a = _alpha;
        itemIcon.color = color;

    }
    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        
        item = _item;
        itemCount = _count;
        itemIcon.sprite = item.itemImage;

        
        // 장비 템일 아닐경우는 수량이미지와 숫자텍스트가 보여야됨
        if (item.itemType != Item.ItemType.Equipment) {
            countImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else {
            textCount.text = "0";
            countImage.SetActive(false);
        }
        SetColor(1);
    }
    // 아이템을 추가로 먹을경우 슬롯을 늘려야됨
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0) {
            ClearSlot();
        }
        else if (itemCount >=30) {
            Debug.Log("슬롯꽉참");
        }
    }
    // 아이템 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemIcon.sprite = null;
        SetColor(0);

        textCount.text = "0";     
        countImage.SetActive(false);
    }
    public int GetQuickSlotNumber()
    {
        return quickSlotNumber;
    }

    public void DecreaseAtk()
    {
        theItemEffectDatabase.DecreaseAtk();
    }
    // 여기서 부터 드래그앤드롭 구현
    public void OnPointerClick(PointerEventData eventData)
    {
        // 이 스크립트가 적용된 객체에 마우슨 오클릭하면 실행되게 끔 하는 조건
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (item != null) {
                if (!isQuickSlot)
                {
                    // 장비템일 경우 장착
                    if (item.itemType == Item.ItemType.Equipment)
                    {
                        #region 아이템 장착
                        switch (item.itemName)
                        {
                            case "Sword1":
                                if (inven.equipSlots[0].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempAtk += 15;
                                }
                                inven.equipSlots[0].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Sword2":
                                if (inven.equipSlots[0].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempAtk += 30;
                                }
                                inven.equipSlots[0].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Staff1":
                                if (inven.equipSlots[0].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempAtk += 15;
                                }
                                inven.equipSlots[0].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Staff2":
                                if (inven.equipSlots[0].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempAtk += 30;
                                }
                                inven.equipSlots[0].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Bow1":
                                if (inven.equipSlots[0].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempAtk += 15;
                                }
                                inven.equipSlots[0].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Bow2":
                                if (inven.equipSlots[0].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempAtk += 30;
                                }
                                inven.equipSlots[0].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Helmet":
                                if (inven.equipSlots[1].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempDef += 10;
                                }
                                inven.equipSlots[1].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Armor":
                                if (inven.equipSlots[2].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempDef += 10;
                                }
                                inven.equipSlots[2].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                            case "Pants":
                                if (inven.equipSlots[3].itemCount == 0)
                                {
                                    theItemEffectDatabase.tempDef += 10;
                                }
                                inven.equipSlots[3].AddItem(item);
                                ClearSlot();
                                toolTip.HideToolTip();
                                break;
                        }
                        #endregion
                    }
                    // 포션일 경우 바로먹고 피 회복
                    else
                    {
                        SetSlotCount(-1);
                        if (item.itemName == "ATKPotion")
                        {
                            Debug.Log(item.itemName + "을 사용했습니다");
                            sound.PlaySfx("useItem");
                            SetSlotCount(-1);
                        }
                        
                        
                        }
                    }
                    else if (!theItemEffectDatabase.GetIsCoolTime())
                    {
                        theItemEffectDatabase.UseItem(item);
                        sound.PlaySfx("useItem");
                        if (item.itemType == Item.ItemType.Potion)
                            SetSlotCount(-1);
                }
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("클릭포인터확인");
        // 맨처음 마우스 클릭한위치로 트랜스폼 위치 저장
        if (item != null ) {

            // 드래그슬롯 코드를 쓰기위해 인스턴스
            if (Inventory.inventoryActivated && !Equipment.EquipActivated) {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemIcon);

                DragSlot.instance.transform.position = eventData.position;
            }
            else if (Inventory.inventoryActivated && Equipment.EquipActivated) {
                DragSlot.instance.dragSlot = this;
                // 드래그 중일때 나타날 이미지
                DragSlot.instance.DragSetImage(itemIcon);

                DragSlot.instance.transform.position = eventData.position;
            }
            
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;

        if (quickSlotNumber >= 5 && item.itemType == Item.ItemType.Equipment) {
            DragSlot.instance.dragSlot = null; 
        }
    }
    // 드래그한 슬롯이 인벤토리영역과 퀵 슬롯 영역을 벗어놨는지 체크
    public void OnEndDrag(PointerEventData eventData)
    {
        // 인벤토리와 퀵슬롯 영역을 벗어난 곳에서 드래그를 끝냈다면
        if (!(
            (DragSlot.instance.transform.localPosition.x > baseRect.rect.xMin
            && DragSlot.instance.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y > baseRect.rect.yMin - 130f
            && DragSlot.instance.transform.localPosition.y < baseRect.rect.yMax + 130f)
            ||
            (DragSlot.instance.transform.localPosition.x + baseRect.transform.localPosition.x > quickSlotBaseRect.rect.xMin  
            && DragSlot.instance.transform.localPosition.x + baseRect.transform.localPosition.x < quickSlotBaseRect.rect.xMax
            && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y > quickSlotBaseRect.rect.yMin + quickSlotBaseRect.transform.localPosition.y
            && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y < quickSlotBaseRect.rect.yMax + quickSlotBaseRect.transform.localPosition.y )
            || 
            (DragSlot.instance.transform.localPosition.x + baseRect.transform.localPosition.x > equipRect.rect.xMin + equipRect.transform.localPosition.x
              && DragSlot.instance.transform.localPosition.x + baseRect.transform.localPosition.x < equipRect.rect.xMax + equipRect.transform.localPosition.x
               && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y > equipRect.rect.yMin + equipRect.transform.localPosition.y
               && DragSlot.instance.transform.localPosition.y + baseRect.transform.localPosition.y < equipRect.rect.yMax + equipRect.transform.localPosition.y
            )))
            {
            if (DragSlot.instance.dragSlot != null && quickSlotNumber < 5)
                theInputNumber.Call();
        }
    
        // 인벤토리 혹은 퀵슬롯 영역에서 드래그가 끝났다면
        else {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
            
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) {

            
            ChangeSlot();

            if (isQuickSlot)  // 인벤토리->퀵슬롯 or 퀵슬롯->퀵슬롯
                {
                if (quickSlotNumber >= 5 && item.weaponType == "Common" && DragSlot.instance.dragSlot.item == null) {

                    theItemEffectDatabase.tempDef += 10;

                }
                if (quickSlotNumber == 5 && item.weaponType == "Warrior" && DragSlot.instance.dragSlot.item == null) {

                    if (item.itemName == "Sword1")
                        theItemEffectDatabase.tempAtk += 15;
                    if (item.itemName == "Sword2")
                        theItemEffectDatabase.tempAtk += 30;

                }
                
                theItemEffectDatabase.IsActivatedQuickSlot(quickSlotNumber);
                
            }
            else  // 인벤토리->인벤토리. 퀵슬롯->인벤토리
            {
                if (DragSlot.instance.dragSlot.isQuickSlot)  // 퀵슬롯->인벤토리
                    theItemEffectDatabase.IsActivatedQuickSlot(DragSlot.instance.dragSlot.quickSlotNumber);
            }
        }
    }
    //온엔드드래그와 온드롭의 차이는 온엔드드래그는 그냥 단순히 클릭한 점에서 어디든지 클릭을 뗐을경우 호출하고
    //온드롭은 클릭한 객체와 같은 (지금은 같은 슬롯일경우) 인스턴스에서 클릭을 뗄때 호출

    public void ChangeSlot()
    {
        // 스왑하는 과정에서 옮기는 슬롯의 데이터가 옮겨지는 슬롯에 덮여씌여지니까 다른 객체에 임시로 보관했다 가져온다
        Item _tempItem = item;
        
        int _tempItemCount = itemCount;

        
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);


        
        //switch (item.itemName) {
        //    case "Sword1":
        //        theItemEffectDatabase.tempAtk += 15;
        //        break;
        //    case "Sword2":
        //        theItemEffectDatabase.tempAtk += 30;
        //        break;
        //    case "Staff1":
        //        theItemEffectDatabase.tempAtk += 15;
        //        break;
        //    case "Staff2":
        //        theItemEffectDatabase.tempAtk += 30;
        //        break;
        //    case "Bow1":
        //        theItemEffectDatabase.tempAtk += 15;
        //        break;
        //    case "Bow2":
        //        theItemEffectDatabase.tempAtk += 30;
        //        break;

        //}
        
        // 스왑할때 옮겨지는 슬롯에 아무것도 없으면 그냥 바로 데이터가 씌여지면된다
        if (_tempItem != null) {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else {
            Debug.Log("클리어 슬롯");
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    

    // 인벤토리 툴팁 마우스 들어갈때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && quickSlotNumber==-1)
            theItemEffectDatabase.ShowToolTip(item, transform.position);
    }
    // 슬롯에서 마우스 빠져나올떄 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null && quickSlotNumber == -1)
            theItemEffectDatabase.HideTip();
    }
}

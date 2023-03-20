using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CreateItem
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public float itemCraftingTime;

    public string[] needItemName;
    public int[] needItemNumber;


    public GameObject go_ItemPrefab; // 실제 생성될 포션
}

public class CreateTable : MonoBehaviour
{
    public static bool isOpenCreateTable = false;

    private bool isOpen = false;
    //public bool lookCreateTable = false;
    private Inventory theInventory;


    [SerializeField] private CreateItem[] createItems;  // 제작할 수 있는 연금 아이템 리스트
    [SerializeField] private Transform tf_BaseUI; // 연금 아이템 베이스 UI
    [SerializeField] private Transform tf_PotionAppearPos; // 포션이 생성될 위치

    public GameObject go_CreateBase;

    private bool isCrafting = false; // 아이템의 제작 시작 여부 (ture면 제작 中)

    private Queue<CreateItem> createItemQueue = new Queue<CreateItem>(); // 연금 테이블 아이템 제작 대기열 큐
    private CreateItem currentCraftingItem;  // 현재 제작 중인 연금 아이템(큐의 첫 번째 원소)
    
    [SerializeField]
    private float craftingTime;  // 제작 시간
    [SerializeField]
    private float currentCraftingTime; // 실제 갱신되는 시간. craftingTime 가 되기까지 갱신됨

    [SerializeField] private Slider slider_gauge; // 슬라이더 게이지
    //[SerializeField] private GameObject go_Liquid; // 동작 시키면 액체 등장(포션 제작 중이면 등장)
    [SerializeField] private Image[] image_CraftingItems; // 대기열 슬롯에 있는 아이템 이미지들

    private int page = 1; // 현재 페이지
    [SerializeField] private int theNumberOfSlot; // 한 페이지당 슬롯의 최대 개수(4개)

    [SerializeField] private Image[] image_CreateItems; // 페이지에 따른 포션 이미지들(4개 사용)
    [SerializeField] private Text[] text_CreateItems; // 페이지에 따른 포션 텍스트들(4개 사용)
    [SerializeField] private Button[] btn_CreateItems; // 페이지에 따른 포션 버튼들(4개 사용)

    void Awake()
    {
       // createUI= go_SlotParent.GetComponentsInChildren<InventorySlot>();
    }

    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();
        ClearSlot();
    }
    void Update()
    {
        if (!isFinish()) {
           Crafting();
        }
        TryOpenInventory();
    }
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            isOpenCreateTable = !isOpenCreateTable;

            if (isOpenCreateTable)
                OpenWindow();
            
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.L))
                    CloseWindow();
        }
    }


    private bool isFinish()
    {
        if (createItemQueue.Count == 0 && !isCrafting) {
           // go_Liquid.SetActive(false);
            slider_gauge.gameObject.SetActive(false);
            return true;
        }
        else {
            //go_Liquid.SetActive(true);
            slider_gauge.gameObject.SetActive(true);
            return false;
        }
    }

    private void Crafting()
    {
        if (!isCrafting && createItemQueue.Count != 0)
            DequeueItem();

        if (isCrafting) // DequeItem 을 통해 isCrafting = true 즉 제작 중이된 후
        {
            currentCraftingTime += Time.deltaTime;
            slider_gauge.value = currentCraftingTime;

            if (currentCraftingTime >= craftingTime)
                ProductionComplete();
        }
    }

    private void DequeueItem()
    {
        isCrafting = true; // 대기열에서 뺏으니 제작 시작
        currentCraftingItem = createItemQueue.Dequeue();

        craftingTime = currentCraftingItem.itemCraftingTime;
        currentCraftingTime = 0;
        slider_gauge.maxValue = craftingTime;

        CraftingImageChange();
    }

    private void CraftingImageChange()
    {
        image_CraftingItems[0].gameObject.SetActive(true); // 첫 번재 대기열 이미지는 무조건 보여져야 함. 제작 중이니까

        for (int i = 0; i < createItemQueue.Count + 1; i++) // 이미지는 그대로 남아있으니까 +1 (위에서 Dequeue 하기 전 상태)
        {
            image_CraftingItems[i].sprite = image_CraftingItems[i + 1].sprite;  // 이미지 하나씩 땡겨오기

            if (i + 1 == createItemQueue.Count + 1)
                image_CraftingItems[i + 1].gameObject.SetActive(false);
        }
    }

    public void Buttonclick(int _buttonNum)
    {
        if (createItemQueue.Count < 3) {
            int createItemArrayNumber = _buttonNum + (page - 1) * theNumberOfSlot;

            for (int i = 0; i < createItems[createItemArrayNumber].needItemName.Length; i++) {
                if (theInventory.GetItemCount(createItems[createItemArrayNumber].needItemName[i]) < createItems[createItemArrayNumber].needItemNumber[i]) {
                    return;  // 제작 안됨
                }
            }

            // 인벤토리에서 재료 차감
            for (int i = 0; i < createItems[createItemArrayNumber].needItemName.Length; i++) {
                theInventory.SetItemCount(createItems[createItemArrayNumber].needItemName[i], createItems[createItemArrayNumber].needItemNumber[i]);
            }

            createItemQueue.Enqueue(createItems[createItemArrayNumber]);

            image_CraftingItems[createItemQueue.Count].gameObject.SetActive(true);
            image_CraftingItems[createItemQueue.Count].sprite = createItems[_buttonNum].itemImage;
        }
    }

    private void ProductionComplete()
    {
        isCrafting = false;
        image_CraftingItems[0].gameObject.SetActive(false);

        Instantiate(currentCraftingItem.go_ItemPrefab, tf_PotionAppearPos.position, Quaternion.identity);
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
   
    private void CanCreateTableOpen()
    {
        if (isOpenCreateTable) {
            go_CreateBase.SetActive(true);
        }
    }

    //public void Window()
    //{
    //    isOpen = !isOpen;
    //    if (isOpen)
    //        OpenWindow();
    //    else
    //        CloseWindow();
    //}

    private void OpenWindow()
    {
        isOpen = true;
        isOpenCreateTable = true;
        if (isOpenCreateTable) {
            go_CreateBase.SetActive(true);
        }
        tf_BaseUI.localScale = new Vector3(1f, 1f, 1f);
    }

    public void CloseWindow()
    {
        isOpen = false;
        isOpenCreateTable = false;
        tf_BaseUI.localScale = new Vector3(0f, 0f, 0f); // 꼼수 ⭐ 창 비활성화 하면 안됨. 아직 포션 제조중일 수 있어서 크기만 줄여 둠
    }

    private void ClearSlot()
    {
        for (int i = 0; i < theNumberOfSlot; i++) {
            image_CreateItems[i].sprite = null;
            image_CreateItems[i].gameObject.SetActive(false);
            btn_CreateItems[i].gameObject.SetActive(false);
            text_CreateItems[i].text = "";
        }
    }
}

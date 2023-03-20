using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGet : MonoBehaviour
{
    private static SoundManager sound;
    [SerializeField]
    private float range;

    // 현재 아이템을 줏을 거리가 됐는지 판단
    private bool pickupActivated = false;


    // 충돌체 정보를 Hit에 담는다
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Inventory theInventory;
    public LDHNetPlayer player;
    public Item item;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<LDHNetPlayer>();
        item = FindObjectOfType<Item>();
        sound = SoundManager.instance;
    }
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated) {
            if (hitInfo.transform != null) {
              theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemAdd>().item);
                sound.PlaySfx("getitem");
                hitInfo.transform.gameObject.SetActive(false);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        Debug.DrawRay(transform.position + new Vector3(0, -4.0f), transform.TransformDirection(Vector3.forward) * 20f, Color.blue);

        if (Physics.Raycast(transform.position + new Vector3(0, -4.0f), transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask)) {

            if (hitInfo.transform.tag == "Item") {
                switch (hitInfo.transform.name) {
                    case "Helmet(Clone)":
                        ItemInfoAppear();
                        break;
                    case "Armor(Clone)":
                        ItemInfoAppear();
                        break;
                    case "Pants(Clone)":
                        ItemInfoAppear();
                        break;
                    case "Sword1(Clone)":
                        if (player.name == "Warrior")
                        ItemInfoAppear();
                        break;
                    case "Sword2(Clone)":
                        if (player.name == "Warrior")
                            ItemInfoAppear();
                        break;
                    case "Staff1(Clone)":
                        if (player.name == "Wizard")
                            ItemInfoAppear();
                        break;
                    case "Staff2(Clone)":
                        if (player.name == "Wizard")
                            ItemInfoAppear();
                        break;
                    case "Bow1(Clone)":
                        if (player.name == "Archer")
                            ItemInfoAppear();
                        break;
                    case "Bow2(Clone)":
                        if (player.name == "Archer")
                            ItemInfoAppear();
                        break;

                    default:
                        ItemInfoAppear();

                        break;
                }
            }
        }
        else
            InfoDisappear();
    }

    private void ItemInfoAppear()
    {
            pickupActivated = true;

    }

    private void InfoDisappear()
    {
        pickupActivated = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DragSlot : MonoBehaviour
{
    // 어디서든 참조할수 있게끔 static 선언
    static public DragSlot instance;

    public InventorySlot dragSlot;
    [SerializeField]
    private Image imageItem;

    void Start()
    {
        instance = this;

    }
    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
        
    }

    // 처음에 안보이다가 드래그할때만 보여지게 설정
    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        // 색깔 투명도 설정
        color.a = _alpha;
        imageItem.color = color;
    }
    
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CreateToolTip theArchemy;
    [SerializeField] private int[] buttonNum;
    [SerializeField] private string[] buttonName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theArchemy.ShowTooltip(buttonName,buttonNum);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theArchemy.HideToolTip();
    }
}

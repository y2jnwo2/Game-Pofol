using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAdd : MonoBehaviour
{
    public Item item;

    public void OnEnable()
    {
        Invoke(nameof(disapper), 20f);
    }

    public void disapper()
    {
        this.gameObject.SetActive(false);
    }
}

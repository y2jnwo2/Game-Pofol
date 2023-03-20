using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator anim;
    private LDHNetPlayer enterPlayer;

    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform itemPos;
    public Text talkText;
    public string[] talkData;

    public void Enter(LDHNetPlayer player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
        talkText.text = "다양한 포션을\n준비중이야";
    }
    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;

    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if (price > enterPlayer.playerData.Gold) {
            StopCoroutine(CantBuy());
            StartCoroutine(CantBuy());
            return;
        }

        enterPlayer.playerData.Gold -= price;
        Vector3 ranVec = Vector3.forward * Random.Range(-1.1f, 1.1f) + Vector3.right * Random.Range(-1.1f, 1.1f);
        Instantiate(itemObj[index], itemPos.position+new Vector3(0,-1.4f,0) + ranVec,itemPos.rotation );

    }

    IEnumerator CantBuy()
    {
        talkText.text = talkData[0];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[1];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WJFloatDamage : MonoBehaviour
{
    public GameObject hudDamageText;
    public Transform hudPos;

    //적 오브젝트가 데미지를 입었을 때
    public void EnemyDamage(int damage)
    { 
        //데미지를 입었을 때 생성할 텍스트 오브젝트
        GameObject hudText = Instantiate(hudDamageText);
        //텍스트가 표시될 위치
        hudText.transform.position = hudPos.position;

        // 텍스트가 카메라를 바라보도록 한다.
        hudText.transform.LookAt(new Vector3(Camera.main.transform.position.x - transform.position.x, Camera.main.transform.position.y - transform.position.y, Camera.main.transform.position.z - transform.position.z));
        hudText.transform.Rotate(new Vector3(0, 180.0f, 0));
        //텍스트에게 데미지를 전달해준다.
        hudText.GetComponent<WJEnemyDamageSet>().damage = damage;
    }
    //플레이어가 데미지를 입었을 때
    public void PlayerDamage(int damage)
    {
        //데미지를 입었을 때 생성할 텍스트 오브젝트
        GameObject hudText = Instantiate(hudDamageText);
        //텍스트가 표시될 위치
        hudText.transform.position = hudPos.position;
        // 텍스트가 카메라를 바라보도록 한다.
        hudText.transform.LookAt(new Vector3(Camera.main.transform.position.x - transform.position.x, Camera.main.transform.position.y - transform.position.y, Camera.main.transform.position.z - transform.position.z));
        hudText.transform.Rotate(new Vector3(0, 180.0f, 0));
        //텍스트에게 데미지를 전달해준다.
        hudText.GetComponent<WJPlayerDamageSet>().damage = damage;
    }
}

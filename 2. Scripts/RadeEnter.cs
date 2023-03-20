using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RadeEnter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //플레이어가 파티를 구하기 위한 로비씬으로 재이동
            Debug.Log("입장");
            SceneManager.LoadScene("scLobby");
        }
    }
}

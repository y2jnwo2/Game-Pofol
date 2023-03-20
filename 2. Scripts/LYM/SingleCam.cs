using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WJSingleCam : MonoBehaviour
{
    public GameObject cameraArm;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "scsGame")
        {
            cameraArm.SetActive(true);
            PhotonNetwork.offlineMode = true;
        }
        else if (SceneManager.GetActiveScene().name == "scLobby")
        {
            PhotonNetwork.offlineMode = false;
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

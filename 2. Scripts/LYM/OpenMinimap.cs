using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WJOpenMinimap : MonoBehaviour
{
    //미니맵이 있는 패널
    public GameObject minimap;
    //큰 미니맵이 있는 패널
    public GameObject bigmap;

    [SerializeField]
    private bool isMinimap = true;

    void Update()
    {
        OpenMinimap();
    }

    void OpenMinimap()
    {
        if (Input.GetKeyDown(KeyCode.M) && isMinimap)
        {
            minimap.SetActive(false);
            bigmap.SetActive(true);
            isMinimap = false;
        }

        else if (Input.GetKeyDown(KeyCode.M) && !isMinimap)
        {
            minimap.SetActive(true);
            bigmap.SetActive(false);
            isMinimap = true;
        }
    }
}
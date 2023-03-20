using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ScreenShot : MonoBehaviour
{
    //private Camera camera;       //보여지는 카메라.

    private int resWidth;
    private int resHeight;
    string _path;
    string _name;
    private void Awake()
    {
       //camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    // Use this for initialization
    void Start()
    {
        resWidth = Screen.width;
        resHeight = Screen.height;
        _path = Application.dataPath + "/Resources/ScreenShot/";
    }

    public void ScreenShotSave(int _num)
    {
        DirectoryInfo dir = new DirectoryInfo(_path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(_path);
        }
        _name = _path + _num + "playerData" + ".jpg";
        
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToJPG();
        System.IO.File.Delete(Application.dataPath + @"/Resources/ScreenShot/" + _num + "PlayerData");
        File.WriteAllBytes(_name, bytes);
        Destroy(rt);
    }

    public void ScreenShotLoad()
    {

    }
}
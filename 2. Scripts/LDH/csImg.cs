using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class csImg : MonoBehaviour
{

    public RawImage image;
    public int num = 0;
    public bool isImg = false;

    ///

    public Text text;
    PlayerData _date = new PlayerData();
    void Update()

    {
        if (this.gameObject.activeSelf == true)
        {
            string folderPath = Application.dataPath + "/Resources/ScreenShot";
            string filePath = Application.dataPath + "/Resources/ScreenShot/" + num + "playerData.jpg";
            if (Directory.Exists(folderPath) == false)
            {
                Debug.Log("not Found folder");
            }
            if (File.Exists(filePath) == false)
            {
                Debug.Log("not Found file");
                text.text = "Empty";
            }
            else
            {
                isImg = true;
                byte[] texBuffer = File.ReadAllBytes(filePath);
                Texture2D imageTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
                imageTexture.LoadImage(texBuffer);
                image.texture = imageTexture;
                string jsonData1 = File.ReadAllText(Application.dataPath + "/Resources/JsonData/" + num + "playerData.json");
                _date = JsonUtility.FromJson<PlayerData>(jsonData1);
                text.text = _date.date.ToString();
            }
        }
    }
    // NOTE
    // LoadImage()로부터 텍스쳐 크기가 결정되므로 앞의 두 개의 파라미터는 사실 딱히 의미가 없다.
    // 세 번째 파라미터도 마찬가지지만 default로 넣으면 0 값이 들어가는데, TextureFormat에 0이 없으므로 에러가 난다.
    // 네 번째 파라미터를 true로 바꾸면 밉맵 체인을 형성하므로, false로 해준다.


}

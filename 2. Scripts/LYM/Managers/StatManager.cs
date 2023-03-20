using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    private static SoundManager sound;
    [SerializeField]
    private ItemEffectDatabase thePlayerData;
    public PlayerData playerData;

    public Text[] txt;

    public int strPoint;
    public int dexPoint;
    public int intPoint;

    void Start()
    {
        strPoint=0;
        dexPoint=0;
        intPoint=0;
        sound = SoundManager.instance;
    }


    void Update()
    {
        thePlayerData = FindObjectOfType<ItemEffectDatabase>();
        LevelUp();
    }
    public void LevelUp()
    {
         switch(thePlayerData.tempLevel) {
            case 2:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                // 여기에 레벨업 사운드 이팩트 넣어줘요
                sound.PlaySfx("levelup");
                break;
            case 3:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 4:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 5:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 6:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 7:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 8:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 9:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 10:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 11:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 12:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;
            case 13:
                txt[0].text = string.Format("{0}", thePlayerData.theplayerStatus.level);
                sound.PlaySfx("levelup");
                break;

        }
        txt[4].text = string.Format("{0}", thePlayerData.theplayerStatus.statPoint);
        
    }
    public void StatStr(int str)
    {

        if (thePlayerData.theplayerStatus.statPoint > 0) {
            strPoint++;
            thePlayerData.tempAtk += str * 2;
            thePlayerData.tempMaxHp += str * 2;
            txt[1].text = string.Format("{0}", strPoint);

            thePlayerData.theplayerStatus.statPoint--;
        }

    }
    public void StatDex(int dex)
    {
        if (thePlayerData.theplayerStatus.statPoint > 0) {
            dexPoint++;
            thePlayerData.tempAtk += dex * 2;
            thePlayerData.tempMaxHp += dex * 2;
            txt[2].text = string.Format("{0}", dexPoint);

            thePlayerData.theplayerStatus.statPoint--;

        }
    }
    public void StatInt(int ints)
    {
        if (thePlayerData.theplayerStatus.statPoint > 0) {
            intPoint++;
            thePlayerData.tempAtk += ints * 2;
            thePlayerData.tempMaxMp += ints * 2;
            txt[3].text = string.Format("{0}", intPoint);

            thePlayerData.theplayerStatus.statPoint--;

        }
    }
    //public void PointUp()
    //{
    //    txt[0].text = thePlayerData.theplayerStatus.statPoint.ToString();
    //    txt[1].text = strPoint.ToString();
    //    txt[2].text = dexPoint.ToString();
    //    txt[3].text = intPoint.ToString();
    //}
}

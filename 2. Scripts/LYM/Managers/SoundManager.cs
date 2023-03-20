using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    Sound[] sfx = null;
    [SerializeField]
    Sound[] bgm = null;
    [SerializeField]
    Sound walksnd = null;
    [SerializeField]
    Sound hurt = null;

    public AudioSource bgmVol = null;
    public AudioSource[] sfxVol = null;


    //Slider
    public Slider bgmSld = null;
    public Slider sfxSld = null;
    public Slider bgmSldg = null;
    public Slider sfxSldg = null;

    //Toggle
    public Toggle bgmTgl = null;
    public Toggle sfxTgl = null;
    public Toggle bgmTgls = null;
    public Toggle sfxTgls = null;


    public AudioSource walk = null;

    //이펙트 효과음을 실행시킬 오디오 소스
    [SerializeField]
    private AudioSource audioBgm = null;
    //배경음을 실행시킬 오디오 소스
    [SerializeField]
    private AudioSource[] audioSfx = null;

    //공격음 받을 소스
    public AudioSource atdSound = null;
    //플레이어

    [SerializeField]
    private AudioSource audioHurt = null;

    [SerializeField]
    private AudioSource audioEnemyatk = null;

    [SerializeField]
    private AudioSource audioEnemy = null;


    void Awake()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }

        bgmSld.value = 1;
        bgmSldg.value = 1;
        sfxSld.value = 1;
        sfxSldg.value = 1;

        bgmTgl.isOn = false;
        bgmTgls.isOn = false;
        sfxTgl.isOn = false;
        sfxTgls.isOn = false;

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        bgmSldg.value = bgmSld.value;
        sfxSldg.value = sfxSld.value;
        //MuteCheck();
        Debug.Log(sfxSld.value);
    }
    public void PlayBgm(string bgm_Name)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm_Name == bgm[i].name)
            {
                audioBgm.clip = bgm[i].clip;
                audioBgm.Play();

            }
        }
    }
    public void Walk()
    {

        walk.clip = walksnd.clip;
        if (walk.isPlaying == true)
        {
            return;
        }
        else
            walk.Play();
    }

    public void StopBGM()
    {
        audioBgm.Stop();
    }
    public void PlaySfx(string sfx_Name)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx_Name == sfx[i].name)
            {
                for (int j = 0; j < audioSfx.Length; j++)
                {
                    if (!audioSfx[j].isPlaying)
                    {
                        audioSfx[j].clip = sfx[i].clip;
                        atdSound = audioSfx[j];
                        audioSfx[j].Play();
                        Debug.Log(j);
                        return;
                    }

                }
                //if(sfx_Name == )
                Debug.Log("모든 오디오가 재생중입니다.");
                return;
            }
        }
    }
    public void StopSfx(string sfx_Name_done)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (sfx_Name_done == sfx[i].name)
            {
                for (int j = 0; j < audioSfx.Length; j++)
                {
                    if (audioSfx[j].isPlaying)
                    {
                        audioSfx[j].clip = sfx[i].clip;
                        walk = audioSfx[j];
                        audioSfx[j].Stop();

                        return;
                    }
                }
                Debug.Log("모든 오디오가 중중입니다.");
                return;
            }
        }
        return;
    }
    public void StopAtkSnd()
    {
        atdSound.Stop();
    }

    public void Volum()
    {
        bgmVol.volume = bgmSld.value * 0.25f;
        bgmVol.volume = bgmSldg.value * 0.25f;


        sfxVol[0].volume = sfxSld.value * 0.25f;
        sfxVol[0].volume = sfxSldg.value * 0.25f;

        sfxVol[1].volume = sfxSld.value * 0.25f;
        sfxVol[1].volume = sfxSldg.value * 0.25f;

        sfxVol[2].volume = sfxSld.value * 0.25f;
        sfxVol[2].volume = sfxSldg.value * 0.25f;

        sfxVol[3].volume = sfxSld.value * 0.25f;
        sfxVol[3].volume = sfxSldg.value * 0.25f;

        sfxVol[4].volume = sfxSld.value * 0.25f;
        sfxVol[4].volume = sfxSldg.value * 0.25f;

        sfxVol[5].volume = sfxSld.value * 0.25f;
        sfxVol[5].volume = sfxSldg.value * 0.25f;

        sfxVol[6].volume = sfxSld.value * 0.25f;
        sfxVol[6].volume = sfxSldg.value * 0.25f;



    }

    //void MuteCheck()
    //{
    //    if (bgmTgl.isOn == true || bgmTgls == true)
    //    {
    //        bgmVol.mute = true;
    //    }
    //    else
    //        bgmVol.mute = false;



    //    if (sfxTgl.isOn == true || sfxTgls.isOn == true)
    //    {
    //        sfxVol.mute = true;
    //    }
    //    else
    //        sfxVol.mute = false;


    //}
}



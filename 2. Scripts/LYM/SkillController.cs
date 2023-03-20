using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField]
    private Skill currentGun; // 현재 들고 있는 총. 📜Gun.cs 가 할당 됨.

    private float currentFireRate; // 이 값이 0 보다 큰 동안에는 총알이 발사 되지 않는다. 초기값은 연사 속도인 📜Gun.cs의 fireRate 

    private bool isReload = false;  // 재장전 중인지. 
    private bool isFineSightMode = false; // 정조준 중인지.

    [SerializeField]
    private Vector3 originPos;  // 원래 총의 위치(정조준 해제하면 나중에 돌아와야 하니까)

    private AudioSource audioSource;  // 발사 소리 재생기
    public static bool isActivate = true;  // 활성화 여부
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    

       

    private void PlaySE(AudioClip _clip)  // 발사 소리 재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public void GunChange(Skill _gun)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);

        isActivate = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 스킬 중복 교체 실행 방지
    public static bool isChangeWeapon;

    // 스킬 교체 딜레이
    [SerializeField]
    private float changeweaponDelayTime;
    [SerializeField]
    private float changeweaponEndDelayTime;

    // 이거 다 무기아니라 스킬로 바꿔볼것
    [SerializeField]
    private Skill[] guns;
    [SerializeField]
   // private Hand[] hands;

    private Dictionary<string, Skill> gunDictionary = new Dictionary<string, Skill>();

    [SerializeField]
    private string currentWeaponType;  // 현재 무기의 타입 (총, 도끼 등등)
    public static Transform currentWeapon;  // 현재 무기. static으로 선언하여 여러 스크립트에서 클래스 이름으로 바로 접근할 수 있게 함.
    public static Animator currentWeaponAnim; // 현재 무기의 애니메이션. static으로 선언하여 여러 스크립트에서 클래스 이름으로 바로 접근할 수 있게 함.

    [SerializeField]
    private SkillController theSkillController;  // 총 일땐 📜GunController.cs 활성화, 손일 땐 📜GunController.cs 비활성화 
   

    void Start()
    {
        for (int i = 0; i < guns.Length; i++) {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
       
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
       // currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeweaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeweaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType) {
            case "Skill":
                
                SkillController.isActivate = false;
                break;
            
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if (_type == "SKill") {
            theSkillController.GunChange(gunDictionary[_name]);
        }
        
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WJBossEffect : MonoBehaviour
{
    [SerializeField]
    //자기 자신의 위치
    private Transform boss;
    //플레이어 이름
    private string player;
    //맞았을 때 터질 이펙트
    public GameObject[] effects;
    //맞았는지 확인할 변수
    private bool isHit = false;
    //---------------------------(11.19 추가)
    //보스의 이펙트를 저장할 리스트
    public List<GameObject> hitEffects = new List<GameObject>();
    //보스의 이펙트를 넣어둘 공간
    public GameObject parentEffect;

    void Awake()
    {
        boss = GetComponent<Transform>();
        //저장된 플레이어의 이름을 받아서 이펙트를 다르게 표현
        player = PlayerPrefs.GetString("Select");
        //---------------------------(11.19 추가)
        parentEffect = GameObject.Find("BossEffect");
    }
    //---------------------------(11.19 추가)
    void Start()
    {
        for (int i = 0; i < 9; i++) {
            GameObject obj = Instantiate(effects[i]);
            obj.SetActive(false);
            obj.transform.parent = parentEffect.transform;
            hitEffects.Add(obj);
        }
    }
    //---------------------------(11.20 추가)
    IEnumerator EffectOnOff(GameObject effect)
    {
        effect.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        effect.SetActive(false);
    }
    //---------------------------(11.20 추가)
    //몬스터의 트리거가 해당 이름의 객체와 부딪힐 경우 이펙트를 실행한다.
    void OnCollisionEnter(Collision col)
    {
        #region 전사
        if (player == "Warrior") {
            //전사 평타(미완)
            if (col.gameObject.name == "Bullet0" && !isHit) {
                isHit = true;
                hitEffects[0].transform.position = col.contacts[0].point;
                hitEffects[0].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[0]));
            }
        }
        #endregion
        #region 궁수
        else if (player == "Archer") {
            //궁수 평타
            if (col.gameObject.name == "Bullet0" && !isHit) {
                isHit = true;
                hitEffects[3].transform.position = col.contacts[0].point;
                hitEffects[3].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[3]));
            }
            //궁수 1번스킬
            else if (col.gameObject.name == "Bullet1" && !isHit) {
                isHit = true;
                hitEffects[4].transform.position = col.contacts[0].point;
                hitEffects[4].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[4]));
            }
            //궁수 2번스킬
            else if (col.gameObject.name == "Bullet2" && !isHit) {
                isHit = true;
                hitEffects[5].transform.position = col.contacts[0].point;
                hitEffects[5].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[5]));
            }
            //궁수 3번스킬
            else if (col.gameObject.name == "Bullet3" && !isHit) {
                isHit = true;
                hitEffects[6].transform.position = col.contacts[0].point;
                hitEffects[6].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[6]));
            }
        }
        #endregion
        #region 법사
        else if (player == "Wizard") {
            //법사 평타
            if (col.gameObject.name == "Bullet0" && !isHit) {
                isHit = true;
                hitEffects[7].transform.position = col.contacts[0].point;
                hitEffects[7].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[7]));
            }
            //법사 1번스킬
            else if (col.gameObject.name == "Bullet1" && !isHit) {
                isHit = true;
                hitEffects[8].transform.position = col.contacts[0].point;
                hitEffects[8].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[8]));
            }
        }
        #endregion
    }
    //---------------------------(11.20 추가)
    //파티클이 닿으면 충돌처리를 해주는 함수
    private void OnParticleCollision(GameObject col)
    {
        if (player == "Warrior") {
            //전사 1번스킬(미완)
            if (col.name == "Bullet1" && !isHit) {
                isHit = true;
                hitEffects[1].transform.position = new Vector3(boss.position.x, 0.0f, boss.position.z);
                hitEffects[1].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[1]));
            }
            //전사 2번스킬(미완)
            else if (col.name == "Bullet2" && !isHit) {
                isHit = true;
                hitEffects[2].transform.position = new Vector3(boss.position.x, 0.0f, boss.position.z);
                hitEffects[2].transform.rotation = boss.rotation;
                StartCoroutine(EffectOnOff(hitEffects[2]));
            }
        }
    }
    //이펙트가 한 번만 뜨게 해주기 위해 트리거를 빠져나갈 때 false로 바꿔준다.
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Bullet0" || col.gameObject.name == "Bullet1" ||
            col.gameObject.name == "Bullet2" || col.gameObject.name == "Bullet3" && isHit) {
            isHit = false;
        }
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WJEnemyEffect : MonoBehaviour
{
    private static SoundManager sound;
    [SerializeField]
    //자기 자신의 위치
    private Transform enemy;
    //플레이어 이름
    private string player;
    //맞았을 때 터질 이펙트
    public GameObject[] effects;

    void Awake()
    {
        enemy = GetComponentInParent<Transform>();
        //저장된 플레이어의 이름을 받아서 이펙트를 다르게 표현
        player = PlayerPrefs.GetString("Select");
        sound = SoundManager.instance;
    }
    //몬스터의 트리거가 해당 이름의 객체와 부딪힐 경우 이펙트를 실행한다.
    //이펙트가 딱 서로 맞은 위치 contact[0].point
    void OnCollisionEnter(Collision col)
    {
        #region 전사
        if (player == "Warrior")
        {
            //전사 평타(미완)
            if (col.gameObject.name == "Bullet0")
            {
                GameObject obj = Instantiate(effects[0], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                    Debug.Log("hi");
                }
                col.gameObject.SetActive(false);
            }
        }
        #endregion
        #region 궁수
        else if (player == "Archer")
        {
            //궁수 평타
            if (col.gameObject.name == "Bullet0")
            {
                GameObject obj = Instantiate(effects[3], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                }
                col.gameObject.SetActive(false);
            }
            //궁수 1번스킬
            else if (col.gameObject.name == "Bullet1")
            {
                GameObject obj = Instantiate(effects[4], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                }
                col.gameObject.SetActive(false);
            }
            //궁수 2번스킬
            else if (col.gameObject.name == "Bullet2")
            {
                GameObject obj = Instantiate(effects[5], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                }
                col.gameObject.SetActive(false);
            }
            //궁수 3번스킬
            else if (col.gameObject.name == "Bullet3")
            {
                GameObject obj = Instantiate(effects[6], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                }
                col.gameObject.SetActive(false);
            }
        }
        #endregion
        #region 법사
        else if (player == "Wizard")
        {
            //법사 평타
            if (col.gameObject.name == "Bullet0")
            {
                GameObject obj = Instantiate(effects[7], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                }
                col.gameObject.SetActive(false);
            }
            //법사 1번스킬
            else if (col.gameObject.name == "Bullet1")
            {
                GameObject obj = Instantiate(effects[8], col.contacts[0].point, transform.rotation);
                Destroy(obj, 1.0f);
                if (this.gameObject.name == "Enemy")
                {
                    sound.PlaySfx("ghosthurt");
                }
                else if (this.gameObject.name == "Devils DEMO")
                {
                    sound.PlaySfx("monsterhurt");
                }
                else if (this.gameObject.name == "Wolf Realistic")
                {
                    sound.PlaySfx("wolfhurt");
                }
                col.gameObject.SetActive(false);
            }
        }
        
     
        #endregion
    }
    //파티클이 닿으면 충돌처리를 해주는 함수
    private void OnParticleCollision(GameObject col)
    {
        //전사 1번스킬(미완)
        if (col.name == "Bullet1")
        {
            GameObject obj = Instantiate(effects[1], transform.position, transform.rotation);
            Destroy(obj, 1.0f);
            col.SetActive(false);
        }
        //전사 2번스킬(미완)
        else if (col.name == "Bullet2")
        {
            GameObject obj = Instantiate(effects[2], transform.position, transform.rotation);
            Destroy(obj, 1.0f);
            col.SetActive(false);
        }
    }
}
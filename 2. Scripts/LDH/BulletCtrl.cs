using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    //유도탄의 타겟팅 위치
    public Transform target;
    //자신의 리지드 바디를 불러온다.
    private Rigidbody rb;
    //회전값의 크기를 설정해준다.
    public float turn;

    //가까운 적의 위치를 찾기 위한 변수
    public List<GameObject> foundObjects;
    public float shortDis;
    //불릿에 있는 콜라이더를 받는 변수
    public BoxCollider col;
    public SphereCollider scol;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
        scol = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        turn = 35.0f;

        foundObjects = null;
    }
    //Enemy와 트리거 충돌이 되는 동안 유도 시작
    void ChasingShot()
    {
        rb.velocity = transform.forward * 35.0f;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        rb.MoveRotation(Quaternion.RotateTowards(this.transform.rotation, targetRotation, turn));
    }
    //오브젝트 활성화와 동시에 총알을 발사하고 적들의 정보를 담는다.
    void BulletShot()
    {
        rb.velocity = transform.forward * 35.0f;

        //게임에 있는 에너미 태그 오브젝트들을 찾아서 리스트 생성
        foundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        //가까운 에너미들의 거리를 계산하여 저장
        shortDis = Vector3.Distance(gameObject.transform.position, foundObjects[0].transform.position);

        target = foundObjects[0].transform;

        foreach (GameObject found in foundObjects) {
            float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
            if (Distance < shortDis) {
                target = found.transform;
                shortDis = Distance;
            }
        }
    }
    //오브젝트 발사 후 1초가 지나면 사라지도록 설정
    IEnumerator BulletFalseTime()
    {
        yield return new WaitForSeconds(1.0f);

        col.enabled = false;
        scol.enabled = false;
        gameObject.SetActive(false);
    }
    //활성화와 동시에 발사함수 및 비활성화를 돕는 함수 실행
    private void OnEnable()
    {
        BulletShot();
        StartCoroutine(BulletFalseTime());
    }
    //비활성화 상태일 때는 적들의 정보를 저장한 배열과, 위치값, 콜라이더를 비활성화 해준다.
    private void OnDisable()
    {
        foundObjects = null;
        target = null;
        col.enabled = true;
        scol.enabled = true;
        rb.angularVelocity = Vector3.zero;
    }
    //여기서는 추적만 함.
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Enemy") {
            ChasingShot();
        }
    }
}
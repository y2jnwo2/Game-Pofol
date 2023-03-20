using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WJPlayerDamageSet : MonoBehaviour
{
    //텍스트가 위로 올라가는 속도
    private float moveSpeed;
    //텍스트의 알파값 컨트롤 속도
    private float alphaSpeed;
    //텍스트 프리팹 파괴 지연시간
    private float destroyTime;
    //텍스트메쉬 컴포넌트에 연결
    private TextMesh text;
    //텍스트의 알파값에 접근하기 위한 Color 변수
    private Color alpha;
    //텍스트의 색상 변경을 위한 Color 변수
    private Color color;
    //받은 데미지를 저장하기 위한 int 변수
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;

        text = GetComponent<TextMesh>();
        color = text.color;
        alpha = text.color;
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDamage();
    }

    public void PlayerDamage()
    {
        //텍스트가 이동할 위치
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        //텍스트의 알파값을 0 ~ 255까지 자연스럽게 변경
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        //텍스트의 전체적인 색상과 알파값을 저장한다.
        color = new Color(255, 0, 0, alpha.a);
        //텍스트의 컬러에 반영.
        text.color = color;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}

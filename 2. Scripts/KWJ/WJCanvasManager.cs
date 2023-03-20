using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WJCanvasManager : MonoBehaviour
{
    private static SoundManager soundManager;
    public static WJCanvasManager canvasManager;
    //로그인 UI
    public GameObject login;
    //로딩 UI
    public GameObject loading;
    //옵션 UI
    public GameObject option;

    public GameObject save;

    private bool isOptionOpen = false;

    public Text txtLoading;
    public Image imgLoading;

    //로딩에 걸리는 시간
    private float loadTime;
    //로딩 시간
    private float startLoadTime = 0.0f;

    GameObject obj;

    void Awake()
    {
        if (canvasManager == null)
        {
            canvasManager = this;
        }
        else if (canvasManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        option.SetActive(false);
        loading.SetActive(false);
        save.SetActive(false);
        soundManager = SoundManager.instance;
    }
    private void Start()
    {
       
        

    }
    void Update()
    {
        //게임, 보스 씬에서만 옵션 창을 여닫을 수 있도록
        if (SceneManager.GetActiveScene().name == "scsGame" || SceneManager.GetActiveScene().name == "scNet")
        {
            OptionOnOff();
        }

       
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "scSelect")
        {
            login.SetActive(true);
        }
        else if(SceneManager.GetActiveScene().name == "scLoading")
        {
            obj = GameObject.Find("Canvas").GetComponent<Transform>().GetChild(0).gameObject;
            loading.SetActive(true);
            StartCoroutine(LoadScene());
        }
        else if (SceneManager.GetActiveScene().name == "scsGame")
        {
            loading.SetActive(false);
            login.SetActive(false);
            save.SetActive(true);
            //soundManager.PlayBgm("gameBgm");
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //로딩 과정을 보여줄 함수
    IEnumerator LoadScene()
    {
        yield return null;
        //백그라운드에서 씬을 로딩해준다.
        AsyncOperation ao = SceneManager.LoadSceneAsync("scsGame");
        //씬이 로딩 되더라도 바로 넘어가지 않도록 해줌.
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            loadTime += Time.deltaTime;

            //fillAmount를 startLoadTime에서 loadTime으로 가는 동안 부드럽게 진행시켜줌.
            imgLoading.fillAmount = Mathf.Lerp(startLoadTime, 2.0f, loadTime);
            //ToString("P");는 숫자를 백분율로 나타내준다.
            txtLoading.text = "로딩중 " + (imgLoading.fillAmount * 100.0f).ToString("0") + "%";

            if (loadTime >= 2.0f)
            {
                //2초가 지난후에 로딩 패널을 꺼주고
                loading.SetActive(false);
                //게임 간단 설명창을 1.5초 정도 띄워준다.
                obj.SetActive(true);
                if (loadTime >= 3.5f) {
                    //로딩 시간 3.5초가 지나면 다음 씬 로딩
                    ao.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
    //오픈, 로딩 씬일 때 옵션이 켜지지 않게 하기 위해 함수로 따로 만들었다.
    void OptionOnOff()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //옵션 창이 열려 있지 않을 때.
            if (!isOptionOpen)
            {
                isOptionOpen = true;
                option.SetActive(true);
            }
            //옵션 창이 열려있을 때
            else if (isOptionOpen)
            {
                isOptionOpen = false;
                option.SetActive(false);
            }
        }
    }
    //UI의 X 버튼으로 끄기 위한 버튼 클릭 함수
    public void OptionOff()
    {
        //O 버튼을 누르지 않고 끄면 Bool 타입 변수가 바뀌지 않기 때문에 추가
        isOptionOpen = false;
        option.SetActive(false);
    }
   
}
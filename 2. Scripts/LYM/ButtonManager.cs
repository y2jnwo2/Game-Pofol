using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WJButtonManager : MonoBehaviour
{
    private static SoundManager soundManager;
    private static WJButtonManager instance;

    //버튼들이 있는 캔버스
    public GameObject OpenCanvas;
    public GameObject sparkleCanvas;
    //옵션 UI
    public GameObject Option;
    //로그인 탭이 있는 패널
    public GameObject login_pnl;
    public GameObject load_pnl;
    public GameObject choice_pnl;
   

    //오픈씬에서 나오는 버튼 오브젝트
    public Button btnStart;
    public Button btnOption;
    public Button btnExit;

    //캐릭터 선택 화면에서 나오는 버튼 오브젝트
    public Button btnWar;
    public Button btnMag;
    public Button btnArc;
    public Button btnLogin;
    public Button btnCreat;
    public Button btnLoad;

    //옵션창을 끄는 버튼
    public Button btnClose;

    void Awake()
    {
        //버튼 컨트롤 한 번에 통합적으로 관리
        if (WJButtonManager.instance == null)
        {
            WJButtonManager.instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Option.SetActive(false);

        if (SceneManager.GetActiveScene().name == "scSelect")
        {
            login_pnl.SetActive(true);
        }

        soundManager = SoundManager.instance;
        soundManager.PlayBgm("openBgm");
        soundManager.PlaySfx("janbul");

    }

    void FixedUpdate()
    {
        //로딩씬에서부터 필요 없어지므로 삭제
        //if (SceneManager.GetActiveScene().name == "scLoading" || SceneManager.GetActiveScene().name == "scsGame")
        //{
        //    Destroy(gameObject, 3.0f);
        //}
        //옵션 탭이 꺼져있을 경우
        if (!Option.activeSelf && SceneManager.GetActiveScene().name == "scOpen")
        {
            sparkleCanvas.SetActive(true);
            if (load_pnl.activeSelf == false)
            {
                OpenCanvas.SetActive(true);
            }
          
        }
    }
    #region 오픈씬
    public void OnClickStart()
    {
        soundManager.StopSfx("janbul");
        soundManager.PlaySfx("btnOpenSC");
        //캐릭터 선택 씬으로 이동
        Debug.Log("tlwkr snfla");
        OpenCanvas.SetActive(false);
        load_pnl.SetActive(true);

    }
    public void OnClickOpenOption()
    {
        Debug.Log("눌리냐");
        soundManager.StopSfx("janbul");
        soundManager.PlaySfx("btnOpenSC");
        //sparkleCanvas.SetActive(false);
       //awwawwwd OpenCanvas.SetActive(false);
        Option.SetActive(true);
    }
    public void OnClickCloseOption()
    {
        soundManager.StopSfx("janbul");
        soundManager.PlaySfx("openUIclick");
        //sparkleCanvas.SetActive(true);
        //OpenCanvas.SetActive(true);
        Option.SetActive(false);
    }
    public void OnClickExit()
    {
        //프로그램 종료
        Application.Quit();
    }
    #endregion
    #region 캐릭터 선택 씬
    public void ClickWar()
    {
        PlayerPrefs.SetString("Select", "Warrior");
        Debug.Log(PlayerPrefs.GetString("Select"));
        soundManager.PlaySfx("selWarrior");
    }
    public void ClickWiz()
    {
        PlayerPrefs.SetString("Select", "Wizard");
        Debug.Log(PlayerPrefs.GetString("Select"));
        soundManager.PlaySfx("selWizard");
    }
    public void ClickArc()
    {
        PlayerPrefs.SetString("Select", "Archer");
        Debug.Log(PlayerPrefs.GetString("Select"));
        soundManager.PlaySfx("selArcher");
    }
    public void Login()
    {
        login_pnl.SetActive(false);
        //로딩 씬으로 이동
        SceneManager.LoadScene("scLoading");
        soundManager.PlaySfx("openUIclick");
    }
    public void Creat()
    {
        load_pnl.SetActive(false);
        SceneManager.LoadScene("scSelect");
        soundManager.PlaySfx("openUIclick");

    }
    public void Load()
    {
        soundManager.PlaySfx("openUIclick");
        load_pnl.SetActive(false);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("scsGame");
        choice_pnl.SetActive(true);
    }
    public void selExit()
    {
        choice_pnl.SetActive(false);
        load_pnl.SetActive(true);
    }
    #endregion
}
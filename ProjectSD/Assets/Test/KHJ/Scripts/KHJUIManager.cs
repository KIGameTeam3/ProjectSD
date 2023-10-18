using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KHJUIManager : MonoBehaviour
{
    float currentTime = 0f;
    float clearTime = 1.0f;
    //[SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private UIHitCollider startHit;

    //[SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private UIHitCollider endHit;

    [SerializeField] private GameObject restartPanel;
    [SerializeField] private UIHitCollider restartHit;

    //{플레이어 관련 UI
    //[SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject pUiPivot; //playerUi 하위 패널입니다.

    [Header("TopPanel")]
    [SerializeField] private GameObject topPanel;
    
    //[SerializeField] private GameObject rightPanel; // 채팅과 버프 이미지 있는 패널

    [Header("LeftPanel")]
    [SerializeField] private GameObject leftPanel; // 코인과 시간 체력 있는 패널
    public GameObject buffImg; //버프 이미지 오브젝트
    
    [Header("UnitDestroyMsg")]
    public TMP_Text[] chatText;  // 팝업 알림 텍스트 리스트 
    public GameObject msgPanel;

    //체력
    [SerializeField] private GameObject healthObj;
    [SerializeField] private Image currentHpImg; // 변동하는 이미지
    [SerializeField] private TMP_Text healthText; //체력 수치 텍스트
    //시간
    public GameObject timePanel;
    [SerializeField] private GameObject timeObj;
    public TMP_Text timeText; // 시간 수치 텍스트
    //}플레이어 관련 UI

    [Header("ShopCanvas")]
   
    [SerializeField] private GameObject shopPanel; //상점 캔버스 하위 패널
    public bool isOpenShop = false;
    //코인
    [SerializeField] private GameObject coinObj; 
    [SerializeField] private TMP_Text coinText; // 코인 수치 텍스트

    [Header("ResultCanvas")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject rTimeObj;
    [SerializeField] private GameObject rCoinObj;
    [SerializeField] private GameObject rKillObj;

    [Header("Boss")]
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private GameObject bossHpObj;
    [SerializeField] private Image currentBossImg; // 변동하는 이미지
    [SerializeField] private TMP_Text bossHpText; //체력 수치 텍스트

    //유닛 리스트
    public GameObject unit;
    List<GameObject> unitList = new List<GameObject>();


    #region 싱글톤 변수
    //싱글턴으로 관리한다.
    private static KHJUIManager instance;
    public static KHJUIManager Instance
    {
        get
        {
            return instance;
        }

        private set { instance = value; }
    }
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        //{시작 종료 재시작 캔버스
        GameObject startCanvas = GameObject.Find("StartCanvas");
        startPanel = startCanvas.transform.GetChild(0).gameObject;
        startHit = startPanel.GetComponent<UIHitCollider>();

        GameObject endCanvas = GameObject.Find("EndCanvas");
        endPanel = endCanvas.transform.GetChild(0).gameObject;
        endHit = endPanel.GetComponent<UIHitCollider>();

        GameObject restartCanvas = GameObject.Find("ReStartCanvas");
        restartPanel = restartCanvas.transform.GetChild(0).gameObject;
        restartHit = restartPanel.GetComponent<UIHitCollider>();
        //} 시작 종료 재시작 캔버스

        //{PlayerUICanvas 
        GameObject playerUi = GameObject.Find("PlayerUICanvas");
        pUiPivot = playerUi.transform.GetChild(0).gameObject;

        //leftPanel
        leftPanel = pUiPivot.transform.GetChild(0).gameObject;
        buffImg = leftPanel.transform.GetChild(0).gameObject;

        //topPanel
        topPanel = pUiPivot.transform.GetChild(1).gameObject;  
        //{체력
        healthObj = topPanel.transform.GetChild(1).gameObject;
        currentHpImg = healthObj.GetComponent<Image>();
        healthText = healthObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        
        //}PlayerUICanvas 

        //{shopCanvas 관련 변수
        GameObject shopCanvas = GameObject.Find("ShopCanvas");
        shopPanel = shopCanvas.transform.GetChild(0).gameObject;
        //코인 변수
        GameObject coinPanel = shopPanel.transform.GetChild(1).gameObject;  
        coinObj = coinPanel.transform.GetChild(0).gameObject;
        coinText = coinObj.GetComponent<TMP_Text>();
        //}shopCanvas 관련 변수

        //{resultCanvas 관련 변수
        GameObject resultCanvas = GameObject.Find("ResultCanvas");
        resultPanel = resultCanvas.transform.GetChild(0).gameObject;
       

        rTimeObj = resultPanel.transform.GetChild(0).gameObject;
        rCoinObj = resultPanel.transform.GetChild(1).gameObject;
        rKillObj = resultPanel.transform.GetChild(2).gameObject;
        //}resultCanvas 관련 변수

        //시간 변수
        GameObject timeCanvas = GameObject.Find("TimeCanvas");
        timePanel = timeCanvas.transform.GetChild(0).gameObject;
        timeObj = timePanel.transform.GetChild(0).gameObject;
        timeText = timeObj.GetComponent<TMP_Text>();
        //boss 관련
        GameObject bossCanvas = GameObject.Find("BossCanvas");
        bossPanel = bossCanvas.transform.GetChild(0).gameObject;
        bossHpObj = bossPanel.transform.GetChild(1).gameObject;
        currentBossImg = bossHpObj.GetComponent<Image>();
        bossHpText = bossHpObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

        //버프 종료 알림 텍스트 관련
        GameObject msgCanvas = GameObject.Find("MsgCanvas");
        msgPanel = msgCanvas.transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        startHit.OnHit.AddListener(() => UIStartGame()); //함수 연결
        endHit.OnHit.AddListener(() => UIExitGame());
        restartHit.OnHit.AddListener(() => UIRestart());

        bossPanel.SetActive(false);
        restartPanel.SetActive(false);
        pUiPivot.SetActive(false);
        shopPanel.SetActive(false);
        resultPanel.SetActive(false);
        timePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //코루틴으로 뺼 예정
        ClearMsg();
    }
  
    public void SendMsg()
    {
        Debug.Log("잘되나요?");
        currentTime = 0f;
        PopUpMsg("TEST SEND");
    }

   
    public void ClearMsg()
    {
        currentTime += Time.deltaTime;
        if (currentTime > clearTime)
        {
            for (int i = 1; i < chatText.Length; i++)
            {
                chatText[i - 1].text = chatText[i].text;
            }
            chatText[chatText.Length - 1].text = "";

            currentTime = 0f;
        }
    } //메시지가 2초뒤에 사라질 수 있도록 하는 함수입니다.
    //}메시지 차에 띄울 함수입니다.
    public void UIStartGame()
    {
        Debug.Log("게임을 시작합니다");
        startPanel.SetActive(false);
        endPanel.SetActive(false);
        pUiPivot.SetActive(true);
        bossPanel.SetActive(true);
        timePanel.SetActive(true);

        GameManager.Instance.StartGame();
        
    }//시작 함수는 완료
    public void UIExitGame()
    {
        Debug.Log("게임을 종료합니다");
        Application.Quit();
    }//종료 버튼 함수도 완료

    public void UIGameOver()
    {
        //TODO 게임 오버 패널 켜주고 2초뒤 리스타트 버튼 활성화 혹은 
        PopUpResult();
    }
    //결과창 띄우는 함수입니다.
    public void PopUpResult()
    {
        Debug.Log("게임 결과창 출력");
        pUiPivot.SetActive(false);

        shopPanel.SetActive(false);
        resultPanel.SetActive(true);
        restartPanel.SetActive(true);
        endPanel.SetActive(true);

        rTimeObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "시간 결과";
        rCoinObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "코인 결과";
        rKillObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "킬 결과";
    }

    public void UIRestart()
    {
        Debug.Log("게임 재시작 되나?");
        restartPanel.SetActive(false);
        resultPanel.SetActive(false);
        endPanel.SetActive(false);
        pUiPivot.SetActive(true);
        bossPanel.SetActive(true);

        GameManager.Instance.StartGame();
    }

    public void ChangeCoinText()
    {
        coinText.text = "" + GameManager.Instance.currentGold;
    }

    public void ChangeHpText()
    {
        //TODO hp 추가를 해줘야 합니다.
        //healthText.text = "" + playerHP;
    }

    public void ChangeBossHpText()
    {
        //TODO hp 추가를 해줘야 합니다.
        //bossHpText.text = "" + bossHP;
    }


    //텍스트 00:00 형식으로 보여주는 함수 
    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    //{메시지 창에 띄울 함수입니다.
    public void PopUpMsg(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++)
        {
            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;
                break;
            }
        }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < chatText.Length; i++)
            {
                chatText[i - 1].text = chatText[i].text;
            }
            chatText[chatText.Length - 1].text = msg;
        }
    }
    //}메시지 창에 띄울 함수입니다.

    public void ShowBuff()
    {
        buffImg.SetActive(true);

    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        isOpenShop = true;
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        isOpenShop = false;
    }
    public void GetUnit()
    {
        //unitList.Add(unit);
    }

}

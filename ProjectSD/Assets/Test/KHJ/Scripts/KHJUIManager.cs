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

    //[SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject endPanel;

    [SerializeField] private GameObject restartPanel;


    //{플레이어 관련 UI
    //[SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject pUiPivot; //playerUi 하위 패널입니다.

    [Header("RightPanel")]
    [SerializeField] private GameObject rightPanel; // 채팅과 버프 이미지 있는 패널
    
    public GameObject buffImg; //버프 이미지 오브젝트
    public TMP_Text[] chatText;  // 팝업 알림 텍스트 리스트 

    public Queue<TMP_Text> chatQueue; 

    [Header("LeftPanel")]
    [SerializeField] private GameObject leftPanel; // 코인과 시간 체력 있는 패널
    //체력
    [SerializeField] private GameObject healthObj;
    [SerializeField] private Image currentHpImg; // 변동하는 이미지
    [SerializeField] private TMP_Text healthText; //체력 수치 텍스트
    //코인
    [SerializeField] private GameObject coinObj; 
    [SerializeField] private TMP_Text coinText; // 코인 수치 텍스트
    //시간
    [SerializeField] private GameObject timeObj;
    [SerializeField] private TMP_Text timeText; // 시간 수치 텍스트
    //}플레이어 관련 UI

    [Header("ShopCanvas")]
    //[SerializeField] private GameObject shopCanvas; //상점 ui 입니다
    [SerializeField] private GameObject shopPanel; //상점 캔버스 하위 패널

    [Header("ResultCanvas")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject rTimeObj;
    [SerializeField] private GameObject rCoinObj;
    [SerializeField] private GameObject rKillObj;
    

    // Start is called before the first frame update
    void Awake()
    {
        //{시작 종료 재시작 캔버스
        GameObject startCanvas = GameObject.Find("StartCanvas");
        startPanel = startCanvas.transform.GetChild(0).gameObject;
        GameObject endCanvas = GameObject.Find("EndCanvas");
        endPanel = endCanvas.transform.GetChild(0).gameObject;
        GameObject restartCanvas = GameObject.Find("ReStartCanvas");
        restartPanel = restartCanvas.transform.GetChild(0).gameObject;
        //} 시작 종료 재시작 캔버스

        //{플레이어 
        GameObject playerUi = GameObject.Find("PlayerUICanvas");
        pUiPivot = playerUi.transform.GetChild(0).gameObject;

        leftPanel = pUiPivot.transform.GetChild(0).gameObject;
        //{체력
        healthObj = leftPanel.transform.GetChild(1).gameObject;
        currentHpImg = healthObj.GetComponent<Image>();
        healthText = healthObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

        coinObj = leftPanel.transform.GetChild(3).gameObject;
        coinText = coinObj.GetComponent<TMP_Text>();

        timeObj = leftPanel.transform.GetChild(4).gameObject;
        timeText = timeObj.GetComponent<TMP_Text>();

        //{오른쪽 패널
        rightPanel = pUiPivot.transform.GetChild(1).gameObject;
        buffImg = rightPanel.transform.GetChild(0).gameObject;
        //}오른쪽 패널

        //{shopCanvas 관련 변수
        GameObject shopCanvas = GameObject.Find("ShopCanvas");
        shopPanel = shopCanvas.transform.GetChild(0).gameObject;
        //}shopCanvas 관련 변수

        //{resultCanvas 관련 변수
        GameObject resultCanvas = GameObject.Find("ResultCanvas");
        resultPanel = resultCanvas.transform.GetChild(0).gameObject;
        rTimeObj = resultPanel.transform.GetChild(0).gameObject;
        rCoinObj = resultPanel.transform.GetChild(1).gameObject;
        rKillObj = resultPanel.transform.GetChild(2).gameObject;
        //}resultCanvas 관련 변수

        chatQueue = new Queue<TMP_Text>();
    }
    private void Start()
    {
        restartPanel.SetActive(false);
        pUiPivot.SetActive(false);
        shopPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > clearTime)
        {
            for (int i = 1; i < chatText.Length; i++)
            {
                chatText[i - 1].text = chatText[i].text;
            }
            chatText[chatText.Length - 1].text = "";
            
            currentTime = 0f;
        }
    }
  
    public void SendMsg()
    {
        Debug.Log("잘되나요?");
        currentTime = 0f;
        PopUpMsg("TEST SEND");
        
    }
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

    public void UIStartGame()
    {
        Debug.Log("게임을 시작합니다");
        startPanel.SetActive(false);
        endPanel.SetActive(false);
        pUiPivot.SetActive(true);
        shopPanel.SetActive(true);
    }
    public void UIExitGame()
    {
        Debug.Log("게임을 종료합니다");
        Application.Quit();
    }

    public void PopUpResult()
    {
        Debug.Log("게임 결과창 출력");
        pUiPivot.SetActive(false);
        shopPanel.SetActive(false);
        resultPanel.SetActive(true);
        restartPanel.SetActive(true);
        endPanel.SetActive(true);

        rCoinObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "시간 결과";
        rCoinObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "코인 결과";
        rCoinObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "킬 결과";
    }

    public void UIRestart()
    {
        Debug.Log("게임 재시작 되나?");
        restartPanel.SetActive(false);
        resultPanel.SetActive(false);
        endPanel.SetActive(false);
        pUiPivot.SetActive(true);
        shopPanel.SetActive(true);

    }

}

using Oculus.Interaction;
using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
//using UnityEditor.IMGUI.Controls;
using UnityEngine;


public class Aim : MonoBehaviour
{
    public bool isLeftHand = default;

    // 레이저 포인트를 발사할 라인 렌더러
    LineRenderer lineRenderer;
    // 레이저 포인터의 최대 거리
    [SerializeField]
    private float lrMaxDistance = 10f;

    //{커브 라인렌더러 변수 관련
    public static bool isChooseTower = false;
    public BuyUnit btn;
    private PreviewBase preview;

    //public int lineSmooth = 40;
    //public float lrCurveLength = 50f;
    //public float lrGravity = -60f;
    //곡선 시뮬레이션의 간격 및 시간?
    //public float simulateTime = 0.02f;
    ////곡선을 이루는 점들을 기억할 리스트
    //List<Vector3> lines = new List<Vector3>();
    //}커브 라인렌더러 변수 관련
    Vector3 storePos = default;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //width 수정
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        preview = FindObjectOfType<PreviewBase>();
    }

    // Update is called once per frame
    void Update()
    {
        //타워를 설치하기 전
        if (!isChooseTower)
        {
            //MouseDetect();
            if (isLeftHand)
            {
                DetectL();
            }       // if : 왼쪽 핸드 기준으로 레이저 포인터 만들기
            else
            {
                DetectR();
            }
        } 
        //타워를 설치한 이후
        else
        {
            ShowTowerCheck();
        }
    } //Update()

    void MouseDetect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // 충돌이 있다면?
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Ray가 부딪힌 지점에 라인 그리기
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, hitInfo.point);
            Debug.Log(hitInfo.collider.tag);
            if (hitInfo.collider.tag == "UiBtn")
            {
                Debug.Log(Input.GetAxis("Fire1"));
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();

                if (Input.GetAxisRaw("Fire1") == 1)
                {
                    Debug.Log("파이어1");
                    // 컨트롤러의 진동 재생
                    //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
            else if (hitInfo.collider.tag == "PlayerUi")
            {
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();
                if (Input.GetButtonDown("Fire1"))
                {
                    // 컨트롤러의 진동 재생
                    //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
        }
    }
  
    
    public void DetectL()
    {
        Vector3 startPos = ARAVRInput.LHandPosition;
        Vector3 endPos = startPos + ARAVRInput.LHandDirection * lrMaxDistance;
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
        {
            // 왼쪽 컨트롤러 기준으로 Ray를 만든다.
            Ray ray = new Ray(startPos, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;

            // 충돌이 있다면?
            if (Physics.Raycast(ray, out hitInfo, lrMaxDistance, GlobalFunction.GetLayerMask("UI")))
            {
                endPos = hitInfo.point;
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();
                if (hitInfo.collider.tag == "UiBtn")
                {
                    // 컨트롤러의 진동 재생
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.LTouch);
                    hitObject?.HitUI();
                }
                else if(hitInfo.collider.tag == "UnitBtn")
                {
                    Debug.Log("UnitBtn 핸드 트리거 찍히나요?");
                    hitObject?.HitUI();
                    btn = hitInfo.collider.gameObject.GetComponent<BuyUnit>();
                    
                }
            } 
        }
       /* //손만 있는 상태에서 왼쪽 컨트롤러 Y 버튼 누르면 상점창 출력 / 죽었을때는  출력 못하게끔
        else if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
        {
            ControlInPlay();
        }*/
        // Ray가 부딪힌 지점에 라인 그리기
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }
    public void DetectR()
    {

        Vector3 startPos = ARAVRInput.RHandPosition;
        Vector3 endPos = startPos + ARAVRInput.RHandDirection * lrMaxDistance;
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            // 왼쪽 컨트롤러 기준으로 Ray를 만든다.
            Ray ray = new Ray(startPos, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;

            // 충돌이 있다면?
            if (Physics.Raycast(ray, out hitInfo, lrMaxDistance, GlobalFunction.GetLayerMask("UI")))
            {
                //Debug.Log(hitInfo.transform.tag);
                endPos = hitInfo.point;
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();
                if (hitInfo.collider.tag == "UiBtn")
                {
                    // 컨트롤러의 진동 재생
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
                else if (hitInfo.collider.tag == "UnitBtn")
                {
                    Debug.Log("UnitBtn 핸드 트리거 찍히나요?");
                    hitObject?.HitUI();
                    btn = hitInfo.collider.gameObject.GetComponent<BuyUnit>();

                }
            }
        }


        // Ray가 부딪힌 지점에 라인 그리기
        else if(ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            Ray ray = new Ray(startPos, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;

            // 충돌이 있다면?
            if (Physics.Raycast(ray, out hitInfo, lrMaxDistance, GlobalFunction.GetLayerMask("UI")))
            {
                Debug.Log(hitInfo.transform.tag);
                endPos = hitInfo.point;
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();
                if (hitInfo.collider.tag == "UnitBtn")
                {
                    Debug.Log("HandTrigger입니다");
                    // 컨트롤러의 진동 재생
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
        }
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }       // else : 오른쪽 핸드 기준으로 레이저 포인터 만들기
    public void ShowTowerCheck()
    {
        //left
        Vector3 startPos = ARAVRInput.LHandPosition;
        Vector3 pos = ARAVRInput.LHandDirection;
        pos.y = 0;
        Vector3 endPos = (startPos + (pos.normalized * lrMaxDistance));
        endPos.y = 0;
        //{TEST
        storePos = endPos;

        //}TEST
        // 왼쪽 컨트롤러 기준으로 Ray를 만든다.
        Ray ray = new Ray(startPos, ARAVRInput.LHandDirection);
        RaycastHit hitInfo;

        preview.transform.position = endPos;
        preview.gameObject.SetActive(true);

        // 충돌이 있다면?
        if (Physics.Raycast(ray, out hitInfo, lrMaxDistance, GlobalFunction.GetLayerMask("Floor")))
        {
            endPos = hitInfo.point;
            storePos = hitInfo.point;
            Debug.Log(hitInfo.transform.name);
            preview.transform.position = endPos;
        }
        else
        {
            //예외처리
            //1. 아무것도 감지 못했을때 그 최대치의 바닥이 floor가 아닐때
            //2. Vector3.up이나 Vector3.down일때 위치

            //{TEST KHJ
            Debug.LogFormat("{0} : 이건 LHNADDirection", ARAVRInput.LHandDirection);

            Ray checkRay = new Ray(preview.transform.position+Vector3.up, -Vector3.up);

            //Debug.DrawRay(checkRay.origin, checkRay.direction * 200f,Color.red);

            RaycastHit hitCheck;
            if (Physics.Raycast(checkRay, out hitCheck, 100, GlobalFunction.GetLayerMask("Floor")))
            {
                //btn.OnPreview();
                //TODO 설치하는 함수 실행
            }
            else
            {
                Debug.Log("사라짐");
                preview.gameObject.SetActive(false);

                //btn.OffPreview();
            }
            //if(Physics.Raycast()
            //}TEST KHJ
        }


        // Ray가 부딪힌 지점에 라인 그리기
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch) && btn != null && preview.installable && preview.gameObject.activeSelf)
        {
            btn.SetInUnit(endPos);
     //   Ray checkRay = new Ray(preview.transform.position, -Vector3.up);
       // Debug.DrawRay(checkRay.origin, checkRay.direction * 200f,Color.red);
      //  RaycastHit hitCheck;
       // if(Physics.Raycast(checkRay, out hitCheck, 200f,GlobalFunction.GetLayerMask("Floor")))
        //{

            //preview.gameObject.SetActive(true);
          //  btn.OnPreview();
            //TODO 설치하는 함수 실행
          //  if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch) && btn != null && preview.installable)
          //  {
          //      btn.SetInUnit(endPos);
          //  }
       // }
       // else
       // {
            //preview.gameObject.SetActive(false);
        //    btn.OffPreview();
        }
    }


    public void ControlInPlay()
    {
        //TODO 플레이어의 상태에 따라서 실행조건을 다르게 해줘야할 것 같습니다.
        if (KHJUIManager.Instance.isOpenShop == false) //&& GameManager.Instance.playerState == PlayerState.PLAY)
        {
            KHJUIManager.Instance.OpenShop();
        }
        else if (KHJUIManager.Instance.isOpenShop == true)
        {
            KHJUIManager.Instance.CloseShop();
        }
    }
}

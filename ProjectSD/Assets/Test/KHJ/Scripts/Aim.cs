using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


public class Aim : MonoBehaviour
{
    public bool isLeftHand = default;

    // 레이저 포인트를 발사할 라인 렌더러
    LineRenderer lineRenderer;
    // 레이저 포인터의 최대 거리
    [SerializeField]
    private float lrMaxDistance = 3f;

    //{커브 라인렌더러 변수 관련
    public static bool isChooseTower = false;

    private PreviewBase preview;

    //public int lineSmooth = 40;
    //public float lrCurveLength = 50f;
    //public float lrGravity = -60f;
    //곡선 시뮬레이션의 간격 및 시간?
    //public float simulateTime = 0.02f;
    ////곡선을 이루는 점들을 기억할 리스트
    //List<Vector3> lines = new List<Vector3>();
    //}커브 라인렌더러 변수 관련

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
        else
        {
            //left
            Vector3 startPos = ARAVRInput.LHandPosition;
            Vector3 pos = ARAVRInput.LHandDirection;
            pos.y = 0;
            Vector3 endPos = (startPos + (pos.normalized * lrMaxDistance));
            endPos.y = 0;

            // 왼쪽 컨트롤러 기준으로 Ray를 만든다.
            Ray ray = new Ray(startPos, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;

            // 충돌이 있다면?
            if (Physics.Raycast(ray, out hitInfo, lrMaxDistance, GlobalFunction.GetLayerMask("Floor")))
            {
                endPos = hitInfo.point;
            }
            else
            {
                //예외처리
                //1. 아무것도 감지 못했을때 그 최대치의 바닥이 floor가 아닐때
                //2. Vector3.up이나 Vector3.down일때 위치
                
                
                if(hitInfo.transform.transform.gameObject)
                
            }

            preview.transform.position = endPos;

            // Ray가 부딪힌 지점에 라인 그리기
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
            
            //TODO 설치하는 함수 실행
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
            {

            }
        }
    
    } //Update()

    void MouseDetect()
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
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

                if (Input.GetAxisRaw("Fire1")==1)
                {
                    Debug.Log("파이어1");
                    // 컨트롤러의 진동 재생
                    //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
            else if(hitInfo.collider.tag == "PlayerUi")
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
    void DetectL()
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
                    
                }
               /* else if (hitInfo.collider.tag == "PlayerUi")
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        // 컨트롤러의 진동 재생
                        //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                        hitObject?.HitUI();
                    }
                }*/
            } 

        }
        // Ray가 부딪힌 지점에 라인 그리기
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }
    void DetectR()
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
                Debug.Log(hitInfo.transform.tag);
                endPos = hitInfo.point;
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();
                if (hitInfo.collider.tag == "UiBtn")
                {
                    // 컨트롤러의 진동 재생
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
                /*else if (hitInfo.collider.tag == "PlayerUi")
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        // 컨트롤러의 진동 재생
                        //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                        hitObject?.HitUI();
                    }
                }*/
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
}

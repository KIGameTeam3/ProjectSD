using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Aim : MonoBehaviour
{
    public bool isLeftHand = default;

    // 레이저 포인트를 발사할 라인 렌더러
    LineRenderer lineRenderer;
    // 레이저 포인터의 최대 거리
    [SerializeField]
    private float lrMaxDistance = 200f;

    //{커브 라인렌더러 변수 관련
    public bool isChooseTower = false;

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
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
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
            if (hitInfo.collider.tag == "Finish")
            {
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();

                if (Input.GetButtonDown("Fire1"))
                {
                    // 컨트롤러의 진동 재생
                    //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
            else if(hitInfo.collider.tag == "Respawn")
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
        // 왼쪽 컨트롤러 기준으로 Ray를 만든다.
        Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
        RaycastHit hitInfo;

        Vector3 endPos = ray.origin + ARAVRInput.LHandDirection * lrMaxDistance; 
        // 충돌이 있다면?
        if (Physics.Raycast(ray, out hitInfo))
        {
            endPos = hitInfo.point;
            if (hitInfo.collider.tag == "Finish")
            {
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();
                if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
                {
                    // 컨트롤러의 진동 재생
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
            else if (hitInfo.collider.tag == "Respawn")
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
        // 충돌이 없다면?
        else
        {
            UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();

            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
            {
                // 컨트롤러의 진동 재생
                //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                //hitObject?.Test();
                Debug.Log("현재 충돌 없습니다.");
            }

        }
        // Ray가 부딪힌 지점에 라인 그리기
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, endPos);
    }
    void DetectR()
    {
        // 오른쪽 컨트롤러 기준으로 Ray를 만든다.
        Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
        RaycastHit hitInfo;

        Vector3 endPos = ray.origin + ARAVRInput.LHandDirection * lrMaxDistance;
        // 충돌이 있다면?
        if (Physics.Raycast(ray, out hitInfo))
        {
            endPos = hitInfo.point;
            if (hitInfo.collider.tag == "Finish")
            {
                UIHitCollider hitObject = hitInfo.transform.GetComponent<UIHitCollider>();

                if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
                {
                    // 컨트롤러의 진동 재생
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                    hitObject?.HitUI();
                }
            }
            else if (hitInfo.collider.tag == "Respawn")
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
        // 충돌이 없다면?
        else
        {
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
            {
                // 컨트롤러의 진동 재생
                //ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                //hitObject?.Test();
                Debug.Log("현재 충돌 없습니다.");
            }
        }
        // Ray가 부딪힌 지점에 라인 그리기
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, hitInfo.point);
    }       // else : 오른쪽 핸드 기준으로 레이저 포인터 만들기
}

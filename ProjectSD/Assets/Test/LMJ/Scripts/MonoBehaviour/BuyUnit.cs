using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyUnit : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region 유닛 변수
    public UnitBase unitPrefab;   // 생성할 유닛 프리팹
    private GameObject unitObj;  // 드래그 중인 유닛을 저장할 변수
    private float unitDestroy = default; // 유닛 파괴 시간
    public int price = default;    // 유닛 가격

    private float installMaxDis = 10f;    // 유닛 최대 설치 거리
    //private Vector3 mousePosition = default;
    //private Vector3 unitPosition = default;
    #endregion

    #region 구매
    //{23.10.18 14:55 KHJ: TMP 텍스트로 사용하기 위해 잠시 변환합니다
    public TMP_Text _price;
    public TMP_Text _name;
    public TMP_Text _info;
    //public Text _price;
    //public Text _name;
    //} KHJ 변경
    #endregion
    //KHJ 콜라이더 체크 넣기 위한 이벤트 추가
    

    public GameObject preview = default;
    public int previewIdx = default;

    //KHJ 변수 선언 10.20
    WaitForSeconds luckyTime = new WaitForSeconds(10f);
    public void Awake()
    {
        _name.text = unitPrefab.unitData.unitName;
        _price.text = unitPrefab.unitData.unitPrice.ToString();
        price = unitPrefab.unitData.unitPrice;
        unitDestroy = unitPrefab.unitData.unitLifeTime;

        preview = FindObjectOfType<PreviewBase>().gameObject;
        Debug.Log(gameObject.name+ " "+preview+"!!!!!");
    }

    public void OnPointerDown(PointerEventData eventData)   // 버튼을 눌렀을 때
    {
        if (gameObject.CompareTag("UnitBtn") && unitPrefab != null) // 프리뷰 생성 조건
        {
            preview.GetComponent<PreviewBase>().previewObj[previewIdx].gameObject.SetActive(true);  // 프리뷰 활성화
            preview.GetComponent<PreviewBase>().PlaceCheck();   // 설치가능 체크 코루틴 켜기

            
            // [PSH] 231018 수정: GetComponent말고 Istance로
            preview.transform.position = GameManager.Instance.hitPosition;  
        }
    }
   
    public void OnDrag(PointerEventData eventData)  // 드래그 중일 때
    {
        // [PSH] 231018 수정 {
        Vector3 currCursorPos = GameManager.Instance.hitPosition;
        Vector3 cursorPosNoY = new Vector3(currCursorPos.x, 0, currCursorPos.z);
        if (cursorPosNoY.magnitude >= installMaxDis)    // 유닛 배치 최대 거리 제한
        {
            preview.transform.position = (cursorPosNoY.normalized * installMaxDis) + (Vector3.up * currCursorPos.y);
        }
        preview.transform.position = currCursorPos;
        // } [PSH] 231018 수정
    }

    public void OnPointerUp(PointerEventData eventData) // 유닛 설치: 클릭 중인 버튼에서 손을 뗄 때
    {
        Debug.Log("버튼에서 손 뗌");
        preview.GetComponent<PreviewBase>().StopPlaceCheck();   // 설치가능 체크 코루틴 끄기
        preview.GetComponent<PreviewBase>().previewObj[previewIdx].gameObject.SetActive(false); // 프리뷰 비활성화

        // 유닛을 생성
        if (preview.GetComponent<PreviewBase>().installable == true)
        {
            Debug.Log("유닛 설치");

            // [PSH] 231018 수정 {
            if (GameManager.Instance.hitPosition.z >= installMaxDis)    // 유닛 배치 최대 거리 제한
            {
                GameManager.Instance.hitPosition.z = installMaxDis;
            }
            unitObj = Instantiate(unitPrefab.gameObject, GameManager.Instance.hitPosition, Quaternion.identity);
            Destroy(unitObj, unitDestroy);  // 설치 후 일정 시간이 지나면 파괴
            GameManager.Instance.SubtractGold(price);    // 재화 소모 메서드 호출
            // } [PSH] 231018 수정
        }
        else { Debug.Log("설치 불가능 지역"); }

    }

    // LEGACY:
    //private void ScreentoWorld()    // 마우스 좌표 설정 메서드
    //{
    //    mousePosition = Input.mousePosition;
    //    unitPosition = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 10.0f));
    //    unitPosition.y = 0;
    //}

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(ray);
    }


    //KHJ 테스트를 위해 복사해서 사용해봅니다.
    //{KHJ 메서드 변환 테스트
    public void ClickUnit()   // 버튼을 눌렀을 때
    {
        if (GameManager.Instance.currentGold < price) return;

        if (gameObject.CompareTag("UnitBtn") && unitPrefab != null) // 프리뷰 생성 조건
        {
            preview.GetComponent<PreviewBase>().previewObj[previewIdx].gameObject.SetActive(true);  // 프리뷰 활성화
            preview.GetComponent<PreviewBase>().PlaceCheck();   // 설치가능 체크 코루틴 켜기

            //골드 감소 함수 불러오기
            GameManager.Instance.SubtractGold(price);

            // [KHJ] 231018 수정: 상점 닫기
            KHJUIManager.Instance.CloseShop();
            //타워 고르기 bool값 true로 
            Aim.isChooseTower = true;

            // [PSH] 231018 수정: GetComponent말고 Istance로
            //preview.transform.position = GameManager.Instance.hitPosition;
        }
    }
   
    public void SetInUnit(Vector3 pos) // 유닛 설치: 클릭 중인 버튼에서 손을 뗄 때
    {
        //Debug.Log(preview);
        //Debug.Log("설치 되는지 "+ preview.GetComponent<PreviewBase>());
        preview.GetComponent<PreviewBase>().StopPlaceCheck();   // 설치가능 체크 코루틴 끄기
        preview.GetComponent<PreviewBase>().previewObj[previewIdx].SetActive(false); // 프리뷰 비활성화

       
        // 유닛을 생성
        if (preview.GetComponent<PreviewBase>().installable == true)
        {
            if(preview.GetComponent<PreviewBase>().previewObj[previewIdx].activeSelf == true)
            {
                OffPreview();
            }
            Debug.Log("유닛 설치");
            unitObj = Instantiate(unitPrefab.gameObject, pos, Quaternion.identity);
            Aim.isChooseTower = false;
            GameManager.Instance.playerState = PlayerState.PLAY;
            PlayerBase.instance.ChangeHand(false);
            Destroy(unitObj, unitDestroy);
        }


            //    // [PSH] 231018 수정 {
            //    if (GameManager.Instance.hitPosition.z >= installMaxDis)    // 유닛 배치 최대 거리 제한
            //    {
            //        GameManager.Instance.hitPosition.z = installMaxDis;
            //    }
            //    unitObj = Instantiate(unitPrefab.gameObject, GameManager.Instance.hitPosition, Quaternion.identity);
            //    Destroy(unitObj, unitDestroy);  // 설치 후 일정 시간이 지나면 파괴
            //    //KHJ 감소하는 함수 주석처리
            //    //GameManager.Instance.SubtractGold(price);    // 재화 소모 메서드 호출
            //    // } [PSH] 231018 수정
            //}
            //else { Debug.Log("설치 불가능 지역"); }
    }
    public void OnLuckyPoint()
    {
        //TODO 예외처리 해줘야함   

        if (GameManager.Instance.currentGold < price) return;

        LuckyPointController.instance.LuuckyUint(luckyTime);
        GameManager.Instance.playerState = PlayerState.PLAY;
        PlayerBase.instance.ChangeHand(false);
        KHJUIManager.Instance.CloseShop();
    }
    public void SpeedUpWeapon()
    {
        if (GameManager.Instance.currentGold < price) return;
        //TODO 여기 안에다가 무기 실행하는 함수 넣으면 됩니다.
        PlayerBase.instance.EnhanceGun(true);
        
        Invoke("StopEnhance", 10);
        GameManager.Instance.playerState = PlayerState.PLAY;
        PlayerBase.instance.ChangeHand(false);
        KHJUIManager.Instance.CloseShop();
    }

    void StopEnhance()
    {
        PlayerBase.instance.EnhanceGun(false);
    }

    public void OnPreview()
    {
        preview.GetComponent<PreviewBase>().previewObj[previewIdx].SetActive(true);  // 프리뷰 활성화
    }
    public void OffPreview()
    {
        preview.GetComponent<PreviewBase>().previewObj[previewIdx].SetActive(false); // 프리뷰 비활성화
    }
}
    



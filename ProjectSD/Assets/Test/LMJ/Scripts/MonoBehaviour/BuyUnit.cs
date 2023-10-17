using System.Collections;
using System.Collections.Generic;
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
    private int price = default;    // 유닛 가격
    //private Vector3 mousePosition = default;
    //private Vector3 unitPosition = default;
    #endregion

    #region 구매
    public Text _price;
    public Text _name;
    #endregion

    public GameObject preview = default;
    public int previewIdx = default;
    public GameObject gameManager = default;

    public void Start()
    {
        _name.text = unitPrefab.GetComponent<UnitBase>().unitData.unitName;
        _price.text = unitPrefab.GetComponent<UnitBase>().unitData.unitPrice.ToString();
        price = unitPrefab.GetComponent<UnitBase>().unitData.unitPrice;
        unitDestroy = unitPrefab.GetComponent<UnitBase>().unitData.unitLifeTime;
    }

    public void OnPointerDown(PointerEventData eventData)   // 버튼을 눌렀을 때
    {
        if (gameObject.CompareTag("UnitBtn") && unitPrefab != null) // 프리뷰 생성 조건
        {
            preview.GetComponent<PreviewBase>().previewObj[previewIdx].gameObject.SetActive(true);  // 프리뷰 활성화
            preview.GetComponent<PreviewBase>().PlaceCheck();   // 설치가능 체크 코루틴 켜기
        }
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중일 때
    {
        preview.transform.position = gameManager.GetComponent<GameManager>().hitPosition;
    }

    public void OnPointerUp(PointerEventData eventData) // 유닛 설치: 클릭 중인 버튼에서 손을 뗄 때
    {
        Debug.Log("버튼에서 손 뗌");
        preview.GetComponent<PreviewBase>().StopPlaceCheck();   // 설치가능 체크 코루틴 끄기
        preview.GetComponent<PreviewBase>().previewObj[previewIdx].gameObject.SetActive(false); // 프리뷰 비활성화

        // 유닛을 생성
        if (preview.GetComponent<PreviewBase>().installable == true)
        {
            unitObj = Instantiate(unitPrefab.gameObject, gameManager.GetComponent<GameManager>().hitPosition, Quaternion.identity);
            Debug.Log("유닛 설치");
            Destroy(unitObj, unitDestroy);  // 설치 후 일정 시간이 지나면 파괴
            gameManager.GetComponent<GameManager>().SubtractGold(price);    // 재화 소모 메서드 호출
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
}

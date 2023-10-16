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
    private float unitDestroy = 7f; // 유닛 파괴 시간
    private Vector3 mousePosition = default;
    private Vector3 unitPosition = default;
    private int unitPrice = 200;    // 유닛 가격
    #endregion

    #region 구매
    public Text _price;
    public Text _name;
    #endregion

    public GameObject gameManager = default;

    public void Start()
    {

        //_name.text = unitPrefab.UnitData.unitName;
        //_price.text = unitPrefab.unitPrice.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)   // 버튼을 눌렀을 때
    {
        if (gameObject.CompareTag("UnitBtn") && unitPrefab != null) // 유닛 생성 조건
        {
            ScreentoWorld();

            gameManager.GetComponent<GameManager>().SubtractGold(unitPrice);    // 재화 소모 메서드 호출

            // 유닛을 생성
            unitObj = Instantiate(unitPrefab.gameObject, unitPosition, Quaternion.identity);

            unitObj.GetComponent<UnitBase>().PlaceCheck();
        }
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중일 때
    {
        ScreentoWorld();

        // 유닛을 마우스 위치로 이동
        unitObj.transform.position = unitPosition;
    }

    public void OnPointerUp(PointerEventData eventData) // 유닛 설치: 클릭 중인 버튼에서 손을 뗄 때
    {
        //unitObj.transform.GetChild(1).GetChild().GetComponent<BulletSpawner>().unitActive = true;  // bullet 발사 활성화
        Debug.Log("버튼에서 손 뗌");

        unitObj.GetComponent<UnitBase>().StopPlaceCheck();

        Destroy(unitObj, unitDestroy);  // 설치 후 일정 시간이 지나면 파괴
    }

    private void ScreentoWorld()    // 마우스 좌표 설정 메서드
    {
        mousePosition = Input.mousePosition;
        unitPosition = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 10.0f));
        unitPosition.y = 0;
    }
}

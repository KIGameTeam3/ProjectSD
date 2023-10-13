using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyUnit : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public GameObject unitPrefab;
    private GameObject unitObj;  // 드래그 중인 유닛을 저장할 변수

    private Vector3 mousePosition = default;
    private Vector3 unitPosition = default;

    public void OnPointerDown(PointerEventData eventData)   // 버튼을 눌렀을 때
    {
        if (gameObject.CompareTag("UnitBtn") && unitPrefab != null) // 유닛 생성 조건
        {
            ScreentoWorld();

            // 유닛을 생성
            unitObj = Instantiate(unitPrefab, unitPosition, Quaternion.identity);
        }
    }

    public void OnDrag(PointerEventData eventData)  // 드래그 중일 때
    {
        ScreentoWorld();

        // 유닛을 마우스 위치로 이동
        unitObj.transform.position = unitPosition;
    }

    private void ScreentoWorld()    // 마우스 좌표 설정 메서드
    {
        mousePosition = Input.mousePosition;
        unitPosition = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 10.0f));
        unitPosition.y = 0;
    }
}

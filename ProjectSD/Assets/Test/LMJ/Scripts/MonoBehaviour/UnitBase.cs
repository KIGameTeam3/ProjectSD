using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public UnitData unitData;

    #region 설치 가능여부 체크
    private Coroutine placeCheckCoroutine;  // 설치 가능여부 체크 코루틴
    //LayerMask unitLayer;
    #endregion



    // bulletPoint 배열
    //public BulletSpawner[] bulletSpawners = default;

    private void Start()
    {
    }

    public void PlaceCheck()
    {
        this.GetComponent<SphereCollider>().enabled = false;    // 오버랩 할때 본인은 체크 안하기 위해
        placeCheckCoroutine = StartCoroutine(PlaceChecking());
    }
    public void StopPlaceCheck()
    {
        StopCoroutine(placeCheckCoroutine);
        this.GetComponent<SphereCollider>().enabled = true;     // 설치하면 콜라이더 활성화
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 표시하기 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    IEnumerator PlaceChecking()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Unit"));   // 유닛 레이어만 판단하기 위해

        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, layerMask);

            //foreach (Collider collider in colliders)
            //{
            //    Debug.Log(collider.name);
            //}

            yield return null;
        }
    }
}

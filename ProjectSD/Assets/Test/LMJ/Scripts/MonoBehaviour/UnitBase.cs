using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    private UnitData unitData;
    public UnitData UnitData { set { unitData = value; } }

    private Coroutine placeCheckCoroutine;

    // bulletPoint 배열
    //public BulletSpawner[] bulletSpawners = default;

    public void PlaceCheck()
    {
        placeCheckCoroutine = StartCoroutine(PlaceChecking());
    }

    public void StopPlaceCheck()
    {
        StopCoroutine(placeCheckCoroutine);
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 표시하기 위한 Gizmos 코드
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    IEnumerator PlaceChecking()
    {

        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

            //Debug.Log("체크중");

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Debug.Log(collider.name);
                    Debug.Log("감지됨");
                }
            }

            yield return null;

        }
    }
}

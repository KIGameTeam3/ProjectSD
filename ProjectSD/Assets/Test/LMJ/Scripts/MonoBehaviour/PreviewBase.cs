using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBase : MonoBehaviour
{
    public GameObject[] previewObj = default;

    #region 설치 가능여부 체크
    private Coroutine placeCheckCoroutine;  // 설치 가능여부 체크 코루틴
    public bool installable = false;    // 설치 가능여부 bool값
    #endregion


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
        // 감지 범위를 표시하기 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    IEnumerator PlaceChecking()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");   // 유닛 레이어만 판단하기 위해

        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, layerMask);


            if (colliders.Length == 0)
            {
                Debug.Log("설치 가능함");
                installable = true;
            }
            else
            {
                foreach (Collider collider in colliders)
                {
                    Debug.Log("설치 불가능" + collider.name);
                    installable = false;
                }
            }

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public UnitData unitData = default;   // 유닛Data 스크립터블 오브젝트
    public GameObject unitHead = default;

    #region Bullet 관련 변수
    public GameObject[] bulletPoints = default;  // 총구 배열
    public GameObject bulletPrefab;     // 생성할 bullet 프리팹
    public float spawnRate = 2.0f;      // bullet 생성 주기
    protected Transform target = default;
    #endregion

    private void Start()
    {
        KHJUIManager.removeObject += Remove;
        target = FindObjectOfType<Golem>().transform;    // player 태그를 가진 오브젝트 찾아 타겟으로 설정

        for (int i = 0; i < bulletPoints.Length; i++)
        {
            StartCoroutine(BulletSpawn(((float)i / bulletPoints.Length)*spawnRate, i));
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        KHJUIManager.removeObject -= Remove;
    }

    protected virtual void Update()
    {
        unitHead.transform.LookAt(target.position+Vector3.up*50);    // Bullet의 정면방향이 target 향하도록 회전
    }

    IEnumerator BulletSpawn(float delayTime, int bulletIdx)
    {
        // 딜레이 시간
        yield return new WaitForSeconds(delayTime);

        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPoints[bulletIdx].transform.position, transform.rotation);
            bullet.transform.SetParent(bulletPoints[bulletIdx].transform); // spawner 하위에 생성
            bullet.transform.LookAt(target);    // Bullet의 정면방향이 target 향하도록 회전

            yield return new WaitForSeconds(spawnRate);
        }
    }
}

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
    private float timeAfterSpawn = default;   // 최근 생성 시점에서 지난 시간
    public Transform target = default;
    #endregion


    private void Start()
    {
        timeAfterSpawn = 0f;    // 생성 시간 초기화
        target = GameObject.FindObjectOfType<playerTest>().transform;    // player 태그를 가진 오브젝트 찾아 타겟으로 설정
        Debug.Log(target.name);
    }

    private void Update()
    {
        timeAfterSpawn += Time.deltaTime;   // 생성 시간 업데이트
        unitHead.transform.LookAt(target);    // Bullet의 정면방향이 target 향하도록 회전

        if (timeAfterSpawn >= spawnRate) // 최근 생성 시간이 생성주기보다 크거나 같을 때
        {
            timeAfterSpawn = 0f;    // 생성 시간 초기화


            for (int i = 0; i < bulletPoints.Length; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletPoints[i].transform.position, transform.rotation);
                bullet.transform.SetParent(bulletPoints[i].transform); // spawner 하위에 생성
                bullet.transform.LookAt(target);    // Bullet의 정면방향이 target 향하도록 회전
            }

        }
    }
}

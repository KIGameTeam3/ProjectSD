using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public bool unitActive = false; // 유닛 활성화 상태 체크

    public GameObject bulletPrefab; // 생성할 bullet 프리팹
    public float spawnRate = 5.0f;  // bullet 생성 주기

    private Transform target = default;
    private float timeAfterSpawn = default;   // 최근 생성 시점에서 지난 시간

    private void Start()
    {
        timeAfterSpawn = 0f;    // 생성 시간 초기화
        target = GameObject.FindWithTag("Player").transform;    // player 태그를 가진 오브젝트 찾아 타겟으로 설정
    }

    private void Update()
    {
        if (unitActive == true)
        {
            timeAfterSpawn += Time.deltaTime;   // 생성 시간 업데이트

            if (timeAfterSpawn >= spawnRate) // 최근 생성 시간이 생성주기보다 크거나 같을 때
            {
                timeAfterSpawn = 0f;    // 생성 시간 초기화

                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                bullet.transform.SetParent(this.transform); // spawner 하위에 생성

                bullet.transform.LookAt(target);    // Bullet의 정면방향이 target 향하도록 회전
            }
        }
    }
}

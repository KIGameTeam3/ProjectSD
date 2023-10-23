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
    public Bullet bulletPrefab;     // 생성할 bullet 프리팹
    public float spawnRate = 2.0f;      // bullet 생성 주기
    protected Transform target = default;
    public ParticleSystem[] flashParticleObj = default;    // 총구 화염 파티클
    #endregion

    private void Start()
    {
        target = FindObjectOfType<Golem>().transform;    // player 태그를 가진 오브젝트 찾아 타겟으로 설정

        for (int i = 0; i < bulletPoints.Length; i++)
        {
            StartCoroutine(BulletSpawn(((float)i / bulletPoints.Length) * spawnRate, i));
        }
    }

    protected virtual void Update()
    {
        unitHead.transform.LookAt(target.GetChild(0).GetChild(0).position);    // Bullet의 정면방향이 target 향하도록 회전
    }

    IEnumerator BulletSpawn(float delayTime, int bulletIdx)
    {

        // 딜레이 시간
        yield return new WaitForSeconds(delayTime);

        while (true)
        {
            Bullet bullet = Instantiate(bulletPrefab, bulletPoints[bulletIdx].transform.position, transform.rotation);
            bullet.transform.SetParent(bulletPoints[bulletIdx].transform); // spawner 하위에 생성
            bullet.Move(unitHead.transform.forward);    // Bullet의 정면방향이 target 향하도록 회전

            //if (flashParticleObj[bulletIdx].isPlaying)
            //{
            //    flashParticleObj[bulletIdx].Stop();    // 총구 화염 파티클 멈춤
            //}
            flashParticleObj[bulletIdx].Play();    // 총구 화염 파티클 재생

            yield return new WaitForSeconds(spawnRate);
        }
    }
}

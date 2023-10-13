using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 8f;  // Bullet 속력
    private Rigidbody bulletRigidbody = default;    // Bullet Rigidbody 컴포넌트

    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();    // Rigidbody 컴포넌트 할당
        bulletRigidbody.velocity = transform.forward * bulletSpeed; // 앞쪽 방향으로 날아가도록 속도 설정

        Destroy(gameObject, 3f);    // 5초 뒤 Bullet 오브젝트 파괴
    }

    // Bullet의 트리거 충돌시
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")   // 충돌한 태그가 Player인 경우
        {
            //Debug.Log("플레이어와 충돌");
        }
    }

}

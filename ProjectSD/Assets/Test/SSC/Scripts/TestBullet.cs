using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Rigidbody myRigid = default;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        myRigid.velocity = Camera.main.transform.forward * 30f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 몬스터들의 히트 포인트에 총알이 닿으면
        if(other.CompareTag("HitPoint"))
        {
            // 해당 게임오브젝트를 담고
            GameObject tempObj = other.gameObject;

            // 해당 게임오브젝트에 IDamage 인터페이스를 체크하고 (몬스터마다 최상위 스크립트에 있음) 
            while(tempObj.GetComponent<IDamage>() == null)
            {
                // 없다면 부모 게임오브젝트를 담는다
                // 이를 IDamage 찾을때까지 반복
                tempObj = tempObj.transform.parent.gameObject;
            }

            // IDamage인터페이스 게임오브젝트를 찾으면 데미지 메소드 실행
            tempObj.GetComponent<IDamage>().DamageAble(10f);
        }

        if(other.CompareTag("LuckyPoint"))
        {
            // 해당 게임오브젝트를 담고
            GameObject tempObj = other.gameObject;

            while (tempObj.GetComponent<LuckyPointController>() == null)
            {
                // 없다면 부모 게임오브젝트를 담는다
                // 이를 IDamage 찾을때까지 반복
                tempObj = tempObj.transform.parent.gameObject;
            }

            // IDamage인터페이스 게임오브젝트를 찾으면 데미지 메소드 실행
            tempObj.GetComponent<LuckyPointController>().ChangePoint(other.gameObject);
            tempObj.GetComponent<IDamage>().DamageAble(10 * 1.5f);
        }
    }
}

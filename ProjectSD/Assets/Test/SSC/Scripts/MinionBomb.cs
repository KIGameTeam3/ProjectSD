using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBomb : MinionBase, IDamage
{
    public float maxHp = 50f;               // 자폭졸개 초기체력 세팅값
    public float currentHp = default;       // 자폭졸개 현재 체력 체크
    public float attackSpeed = 3f;          // 자폭졸개 자폭 실행할 시간
    private float timeReset = 0f;           // 자폭실행 시간 체크
   // private bool attackStart = false;       // 

    protected override void Update()
    {
        base.Update();

        if(isAttack == true)
        {
            timeReset += Time.deltaTime;

            // 자폭실행 시간에 도달하면
            if (attackSpeed <= timeReset)
            {
                Debug.Log("자폭 실행");

                // 자폭졸개 위치 기준으로 일정크기의 Sphere 형태의 레이를 쏘고
                RaycastHit[] hit = Physics.SphereCastAll(transform.position, 10, Vector3.up);

                foreach(RaycastHit obj in hit)
                {
                    // 레이를 맞은 오브젝트에 IDamage 인터페이스가 구현되어 있다면
                    if(obj.transform.GetComponent<IDamage>() != null)
                    {
                        // 해당 오브젝트들에게 데미지를 입힌다.
                        obj.transform.GetComponent<IDamage>().DamageAble(50f);
                    }
                }
            }
        }

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // DeadZone 트리거시 오브젝트 풀 반환
        if (other.CompareTag("DeadZone"))
        {
            StartCoroutine(CoolObj(this.gameObject, PoolObjType.MINION_BOMB));
        }
    }

    // 풀링 오브젝트에 의한 활성화시 초기값 세팅
    protected override void OnEnable()
    {
        base.OnEnable();
        Initilize();
    }


    public void Initilize()
    {
        currentHp = maxHp;
    }

    public void DamageAble(float damage)
    {

        currentHp -= damage;

        if (currentHp <= 0)
        {
            // TODO : 자폭졸개는 사망시에도 자폭을 해야한다. 현재 자폭 기능을 똑같이 가져왔는데 해당 로직이 추가된 후 유니티의 렉이 심해짐
                     
            //RaycastHit[] hit = Physics.SphereCastAll(transform.position, 10, Vector3.up);

            //foreach (RaycastHit obj in hit)
            //{
            //    // 레이를 맞은 오브젝트에 IDamage 인터페이스가 구현되어 있다면
            //    if (obj.transform.GetComponent<IDamage>() != null)
            //    {
            //        // 해당 오브젝트들에게 데미지를 입힌다.
            //        obj.transform.GetComponent<IDamage>().DamageAble(50f);
            //    }
            //}

            //Debug.Log("죽었을시 자폭실행");
            ObjectPoolManager.instance.CoolObj(this.gameObject, PoolObjType.MINION_BOMB);

        }
    }
}

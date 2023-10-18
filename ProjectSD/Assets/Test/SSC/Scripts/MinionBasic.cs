using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MinionBasic : MinionBase, IHitObject
{
    public float attackSpeed = 1f;              // 일반졸개 근접공격 시작 타이머
    private float timeReset = 0f;               // 근접공격 시작 체크값
    private bool atkReset = false;              // 근접공격 업데이트 반복호출 방지용 불값
    public float maxHp = 30f;                   // 일반졸개 체력 세팅값
    public float currentHp = default;           // 일반졸개 현재 체력 체크


    private WaitForSeconds atkcoolTime = new WaitForSeconds(1f);    // 공격 실행시 쿨타임
    [SerializeField] private BoxCollider atkRange;      // 일반졸개 근접공격 콜라이더

    protected override void Update()
    {
        base.Update();

        if(Golem.G_insance.minionRestart == true)
        {
            ObjectPoolManager.instance.CoolObj(this.gameObject, PoolObjType.MINION_BASIC);
        }

        // 부모클래스상에서 추적을 멈추고 공격범위에 들어섰다면
        if (isAttack == true && Golem.G_insance.restart == false)
        {
            // 공격 시간 누적
            timeReset += Time.deltaTime;

            // 시간 누적치가 도달했다면 
            if (attackSpeed <= timeReset && atkReset == false)
            {
                // 업데이트상 공격 반복 호출 방지용
                atkReset = true;

                // 애니메이션 선택
                int randAttack = Random.Range(0, 2);

                switch (randAttack)
                {
                    case 0:
                        myAni.SetTrigger("AttackLeft");
                        break;
                    case 1:
                        myAni.SetTrigger("AttackRight");
                        break;
                }

            }
        }

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // DeadZone(절벽 뒤) 트리거시 오브젝트 풀 반환
        if (other.CompareTag("DeadZone"))
        {
            StartCoroutine(CoolObj(this.gameObject, PoolObjType.MINION_BASIC));
        }
    }

    // 풀링오브젝트 반환에 의한 활성화시 초기 체력 세팅
    protected override void OnEnable()
    {
        base.OnEnable();
        Initilize();
    }

    // 애니메이션 이벤트 호출 메소드 (공격 발생)
    // 일반 메소드시 콜라이더가 인식을 못함 발생, 코루틴으로 1프레임이라도 딜레이를 줘봤음
    IEnumerator Attack()
    {
        // 팔을 휘두르는 모션이 나오면 근접공격 콜라이더를 활성화, 비활성화
        atkRange.enabled = true;

        yield return null;

        atkRange.enabled = false;

    }

    // 애니메이션 이벤트 호출 메소드 (공격이 끝난 뒤)
    IEnumerator AttackCooltime()
    {
        yield return atkcoolTime;
        // 공격 재진입을 위해 불값 초기화
        atkReset = false;
    }

    public void Initilize()
    {
        currentHp = maxHp;
        atkReset = false;
    }

    public void Hit(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            ObjectPoolManager.instance.CoolObj(this.gameObject, PoolObjType.MINION_BASIC);
        }
    }
}

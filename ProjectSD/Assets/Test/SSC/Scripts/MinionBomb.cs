using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBomb : MinionBase, IHitObject
{
    public float maxHp = 50f;               // 자폭졸개 초기체력 세팅값
    public float attackSpeed = 3f;          // 자폭졸개 자폭 실행할 시간
    private float currentHp = default;      // 자폭졸개 현재 체력 체크
    private float timeReset = 0f;           // 자폭실행 시간 체크
    public float explosiveDamage = 10f;     // 자폭 데미지
    public float explosionArea = 10f;       // 자폭 범위

    public enum State { ALIVE, DIE }            // 자폭졸개의 스테이트 상태
    public State state {  get; private set; }   // 스테이트 프로퍼티

    private bool atkReset = false;              // 공격 제한조건

    protected override void Update()
    {
        base.Update();

        // TODO : 게임 재시작과 이어야함, 현재 임시 불값으로 테스트중
        if(GameManager.Instance.playerState == PlayerState.DEAD)
        {
            StopAllCoroutines();
            ObjectPoolManager.instance.CoolObj(this.gameObject, PoolObjType.MINION_BOMB);
        }

        // 부모클래스에서 공격진입에 들어갔다면
        if(isAttack == true)
        {
            // 자폭실행시간까지 시간 누적      
            timeReset += Time.deltaTime;

            // 자폭실행 시간에 도달하면
            if (attackSpeed <= timeReset && atkReset == false)
            {
                timeReset = 0f;     // 자폭 시간 초기화 (풀링오브젝트라 값이 남아있음)
                atkReset = true;    // 자폭진입 제한
                Explosive();        // 자폭실행 메소드
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

    // 자폭실행
    private void Explosive()
    {
        // 자신을 기준으로 일정범위 구체크기만큼 충돌 감지하여 
        Collider[] hitObj = Physics.OverlapSphere(transform.position, explosionArea);

        foreach (Collider info in hitObj)
        {
            if (info.GetComponent<IHitObject>() != null)
            {
                info.GetComponent<IHitObject>().Hit(explosiveDamage);
            }
        }

        ObjectPoolManager.instance.CoolObj(this.gameObject, PoolObjType.MINION_BOMB);
    }


    // 풀링 오브젝트에 의한 활성화시 초기값 세팅
    protected override void OnEnable()
    {
        base.OnEnable();
        Initilize();
    }

    public void Initilize()
    {
        state = State.ALIVE;
        atkReset = false;
        currentHp = maxHp;
    }

    public void Hit(float damage)
    {
        currentHp -= damage;

        // 제한조건을 안달아두면 스택 오버플로우가 발생했었음 => 스테이트 패턴으로 조건문 진입제한을 두겠음.
        if (currentHp <= 0 && state == State.ALIVE)
        {
            state = State.DIE;
            Explosive();
            ObjectPoolManager.instance.CoolObj(this.gameObject, PoolObjType.MINION_BOMB);
        }
    }
}

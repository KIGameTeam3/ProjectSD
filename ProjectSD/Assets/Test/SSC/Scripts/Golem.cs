using Oculus.Interaction.PoseDetection.Debug.Editor.Generated;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;

public class Golem : MonoBehaviour, IDamage
{
    // {괴수의 페이즈를 체크할 enum 스테이트
    public enum Phase 
    {  
        READY,
        PHASE_1, 
        PHASE_2,  
        PHASE_LAST, 
        GAMEOVER
    }

    public Phase golemCheck { get; private set;}
    // }괴수의 페이즈를 체크할 enum 스테이트

    private bool isAttack = false;

    // {괴수의 각종 변수

    public float golemMaxHp = 100f;     // 괴수의 초기 체력
    private float currentHp = default;   // 괴수의 현재 체력    
    public float startTime = 5f;        // 게임 시작시간 체크
                                        // TODO : 추후에는 게임시작 버튼을 누를시 괴수가 행동진입 예정

    public float phase1Ratio = 0.7f;    // 페이즈 1 진입하는 거리비율 = 플레이어로부터 70% 지점
    public float phase1Time = 30f;       // 페이즈 1 제한시간
    public float phase2Ratio = 0.4f;    // 페이즈 2 진입하는 거리비율 = 플레이어로부터 40% 지점
    public float phase2Time = 30f;       // 페이즈 2 제한시간
    private Transform player = default; // 플레이어를 캐싱할 변수
    private float firstPos = default;   // 괴수와 플레이어의 최초 거리 캐싱할 변수
    public float golemSpeed = 5f;       // 괴수의 PC 추적 속도

    private Vector3 target = default;

    [SerializeField] private GameObject RHandBomb = default;     // 괴수의 원거리공격 투사체 소환 포지션 : 오른손
    [SerializeField] private GameObject LHandBomb = default;     // 괴수의 원거리공격 투사체 소환 포지션 : 왼손
    [SerializeField] private Transform MinionSpawn = default;   // 졸개 소환 위치

    // }괴수의 각종 변수

    private Rigidbody golemRigid = default;     // 괴수의 속력을 입력할 컴포넌트
    private Animator golemAni = default;        // 괴수의 애니메이션을 관리할 컴포넌트

    private WaitForSeconds ballThrowcooltime = new WaitForSeconds(3f);
    private WaitForSeconds minionSpawncooltime = new WaitForSeconds(5f);
    // Start is called before the first frame update
    void Start()
    {
        // {게임 입장시 골렘이 가져올 정보들
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();        // 플레이어 태그를 찾아 PC 위치정보 캐싱
        golemCheck = Phase.READY;       // 괴수의 시작 스테이트패턴 READY : 유저의 게임 시작 입력 전까지는 대기를 취함
        golemRigid = GetComponent<Rigidbody>();     // 괴수의 리지드바디
        golemAni = GetComponent<Animator>();        // 괴수의 애니메이터
        Initilize();

        target = (player.transform.position - transform.position).normalized;       // 괴수의 진행할 방향을 체크하기 위한 노말라이즈
        firstPos = Vector3.Distance(transform.position, player.transform.position); // 괴수의 초기 위치와 PC의 거리 체크

        // }게임 입장시 골렘이 가져올 정보들


        // 현재 게임시작을 입력받았을시 정지해있던 골렘이 5초뒤에 골렘이 1페이즈 구간까지 이동하게 설계 해뒀음
        StartCoroutine(GameStart());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 괴수가 PC에 닿는 순간 PC는 즉사처리
        if (collision.collider.CompareTag("Player"))
        {
            // { 게임종료에 따른 괴수의 기능동작 정지 행동들

            golemCheck = Phase.GAMEOVER;            // 스테이트 : 게임오버상태
            golemRigid.velocity = Vector3.zero;     // 괴수는 그자리에서 정지
            golemAni.SetBool("isWalk", false);      // 애니메이션은 IDLE로 돌아간다.

            // } 게임종료에 따른 괴수의 기능동작 정지 행동들

            Debug.Log("게임 종료");
        }
    }
    private void Update()
    {
        // 괴수는 항상 플레이어를 바라본다.
        transform.LookAt(player.transform.position);

        //  괴수는 라스트페이즈에 진입하면 PC를 향해 멈추지않고 다가오게 된다.
        if (golemCheck == Phase.PHASE_LAST)
        {
            golemRigid.velocity = target * golemSpeed;
        }

    }

    IEnumerator GameStart()
    {
        // 대기시간을 체크할 타이머
        float gameStartTimer = 0;

        // 현재 골렘은 5초의 대기시간을 가진뒤 1페이즈 구간까지 걸어간다.
        while(gameStartTimer < startTime)
        {
            gameStartTimer += Time.deltaTime;

            yield return null;
        }

        golemAni.SetBool("isWalk", true);

        // 골렘은 처음 대기 상태에서
        while (golemCheck == Phase.READY)
        {
            // 1페이즈 지점에 도달하게 되면
            if (firstPos * 0.7f >= Vector3.Distance(transform.position, player.transform.position))
            {
                // { 골렘의 페이즈상태 변경
                golemCheck = Phase.PHASE_1;
                golemRigid.velocity = Vector3.zero;
                golemAni.SetBool("isWalk", false);
                // } 골렘의 페이즈상태 변경

                // 1페이즈 코루틴 패턴에 진입하며
                StartCoroutine(Phase1());

                // 해당 코루틴 파괴
                yield break;
            }

            // 골렘은 1페이즈 지점까지 등속운동으로 움직임
            golemRigid.velocity = target * golemSpeed;

            yield return null;
        }

    }

    IEnumerator Phase1()
    {
        // 1페이즈 제한시간을 체크할 타이머
        float phaseTimer = 0f;

        // { 제한시간이 다 되거나 일정체력이 깎이면 1페이즈 탈출
        while(phaseTimer < phase1Time && currentHp >= golemMaxHp * 0.7f)
        {
            phaseTimer += Time.deltaTime;

            // 어느 한 공격이 진행중이라면 다른 공격 코루틴 진입을 못하게 불값으로 체크
            if (isAttack == false)
            {
                isAttack = true;
                // 2가지 공격패턴중 랜덤한 공격 패턴을 선정하기 위한 랜덤값
                int Attack = Random.Range(0, 2);

                //  입력받은 랜덤값을 토대로 행동을 취한다.
                switch (Attack)
                {
                    // 원거리 공격 진입
                    case 0:
                        ThrowBallStart();
                        break;

                    // 졸개 소환 진입
                    case 1:
                        golemAni.SetTrigger("SpawnMinion");
                        break;
                }

            }

            yield return null;
        }
        // } 제한시간이 다 되거나 일정체력이 깎이면 1페이즈 탈출


        // { 2페이즈 돌입을 위한 현재 동작들 정지
        isAttack = false;                   // 공격 행동상태 체크 불값 초기화
        golemAni.SetTrigger("isAttackStop");
        // } 2페이즈 돌입을 위한 현재 동작들 정지

        golemAni.SetBool("isWalk", true);

        // { 1페이즈가 끝난 시점에서 2페이즈로 넘어가는 상태를 체크중
        while (golemCheck == Phase.PHASE_1)
        {
            // 2페이즈 지점에 도달하게 되면
            if (firstPos * 0.4f >= Vector3.Distance(transform.position, player.transform.position))
            {
                // 골렘의 상태 변경
                golemCheck = Phase.PHASE_2;
            }

            // 2페이즈 지점 도달 전까지는 플레이어 방향으로 등속운동 진행
            golemRigid.velocity = target * golemSpeed;

            yield return null;
        }
        // } 1페이즈가 끝난 시점에서 2페이즈로 넘어가는 상태를 체크중

        // { 2페이즈 돌입
        golemRigid.velocity = Vector3.zero;
        golemAni.SetBool("isWalk", false);
        StartCoroutine(Phase2());
        // } 2페이즈 돌입
    }

    IEnumerator Phase2()
    {
        // 2페이즈 제한시간을 체크할 타이머
        float phaseTimer = 0f;

        // { 제한시간이 다 되거나 일정체력이 깎이면 2페이즈 탈출
        while (phaseTimer < phase2Time && currentHp >= golemMaxHp * 0.4f)
        {
            phaseTimer += Time.deltaTime;

            // { 상단에 기술한 1페이즈 공격패턴과 동일한 동작
            if (isAttack == false)
            {
                isAttack = true;

                int Attack = Random.Range(0, 2);

                switch (Attack)
                {
                    case 0:
                        ThrowBallStart();
                        break;
                    case 1:
                        golemAni.SetTrigger("SpawnMinion");
                        break;
                }

            }
            // } 상단에 기술한 1페이즈 공격패턴과 동일한 동작

            yield return null;
        }
        // } 제한시간이 다 되거나 일정체력이 깎이면 2페이즈 탈출

        // 현재 본 클라인트의 임의대로 2페이즈가 끝나면 라스트 페이즈로 진입하게 되어
        // 아무런 동작없이 플레이어를 향해 등속운동하게 설계되어 있음


        // 라스트 페이즈는 업데이트상에서 플레이어를 향해 등속운동 하는것으로 동작함
        // { 라스트 페이즈 진입
        golemCheck = Phase.PHASE_LAST;
        golemAni.SetBool("isWalk", true);
        golemAni.SetTrigger("isAttackStop");
        // } 라스트 페이즈 진입
    }

    private void ThrowBallStart()
    {
        // 원거리 공격 중 왼팔,오른팔을 정할 랜덤값
        int attackPos = Random.Range(0, 2);

        // { 오브젝트풀링 및 투사체의 포물선 운동에 접근하기 위한 각 장치

        // 랜덤값에 의한 왼팔, 오른팔 동작 스위치문
        switch (attackPos)
        {
            // 0이 입력되면 왼팔
            case 0:
                golemAni.SetTrigger("isLeftAttack");                // 왼팔 애니메이터 동작
                LHandBomb.SetActive(true);
                break;

            // 1이 입력되면 오른팔
            case 1:
                golemAni.SetTrigger("isRightAttack");                   // 오른팔 애니메이터 동작=
                RHandBomb.SetActive(true);
                break;
            
        }

    }

    private void FireLeft()
    {
        GameObject obj = null;
        obj = ObjectPoolManager.instance.GetPoolObj(PoolObjType.BOMB);
        obj.transform.position = LHandBomb.transform.position;
        obj.SetActive(true);
        LHandBomb.SetActive(false);
    }

    private void FireRight()
    {
        GameObject obj = null;
        obj = ObjectPoolManager.instance.GetPoolObj(PoolObjType.BOMB);
        obj.transform.position = RHandBomb.transform.position;
        obj.SetActive(true);
        RHandBomb.SetActive(false);
    }

    private void SpawnMinion()
    {
        Vector3 spawnPoint = Vector3.zero;

        GameObject minion = null;

        for (int i = 0; i < 10; i++)
        {
            int randomMinion = Random.Range(0, 2);

            switch (randomMinion)
            {
                case 0:
                    minion = ObjectPoolManager.instance.GetPoolObj(PoolObjType.MINION_BASIC);
                    break;
                case 1:
                    minion = ObjectPoolManager.instance.GetPoolObj(PoolObjType.MINION_BOMB);
                    break;
            }

            spawnPoint.x = MinionSpawn.transform.position.x + Random.Range(-20f, 20f);
            spawnPoint.z = MinionSpawn.transform.position.z + Random.Range(-3f, -1f);
            spawnPoint.y = MinionSpawn.transform.position.y;

            minion.transform.position = spawnPoint;
            minion.SetActive(true);

        }
    }

    // 공격애니메이션이 끝나고 지정할 대기 시간
    IEnumerator FireCooltime()
    {
        yield return ballThrowcooltime;
        isAttack = false;
    }

    IEnumerator MinionCollTime()
    {
        yield return minionSpawncooltime;
        isAttack = false;
    }

    // 괴수에게 데미지를 입히는 메소드
    // TODO : 다른 클라이언트들의 공격체에 괴수 접촉시 Golem 스크립트를 가져와서 해당 메소드를 실행 요청
    public void DamageAble(float damage)
    {
        currentHp -= damage;

        Debug.Log($"괴수 현재 체력 : {currentHp}");
        if(currentHp <= 0)
        {
            StopAllCoroutines();
            golemCheck = Phase.GAMEOVER;
            golemAni.SetTrigger("isAttackStop");
        }
        
    }

    public void Initilize()
    {
         currentHp = golemMaxHp;                     //  괴수의 초기 체력은 설정한 Max체력값 
    }

    // LEGACY : 개발일지에 쓰일 공격패턴 애니메이션 설정 오류부분 (애니메이션 이벤트가 아닌 코루틴으로 접근하려 했음)

    //// 원거리 공격 코루틴
    //IEnumerator ThrowBall()
    //{
    //    // 원거리 공격 중 왼팔,오른팔을 정할 랜덤값
    //    int attackPos = Random.Range(0, 2);

    //    // 코루틴 진행 중 다른 공격행동 진입을 방지하기 위한 불값 변경
    //    isAttack = true;

    //    // { 오브젝트풀링 및 투사체의 포물선 운동에 접근하기 위한 각 장치
    //    GameObject obj = ObjectPoolManager.instance.GetPoolObj(PoolObjType.BOMB);       // 풀링 오브젝트 호출
    //    Bomb objBomb = null;            // 투사체의 포물선 운동이 입력된 Cs 캐싱
    //    Rigidbody objRigid = null;      // 투사체의 포물선 운동을 위한 Rigidbody 컴포넌트 캐싱
    //    Vector3 shoot = Vector3.zero;   // 투사체의 포물선 운동값을 캐싱할 변수

    //    // 호출한 오브젝트가 입력 되었다면 각 컴포넌트 가져오기
    //    if(obj != null)
    //    {
    //        objBomb = obj.GetComponent<Bomb>();
    //        objRigid = obj.GetComponent<Rigidbody>();

    //    }
    //    // }오브젝트풀링 및 투사체의 포물선 운동에 접근하기 위한 각 장치

    //    // 랜덤값에 의한 왼팔, 오른팔 동작 스위치문
    //    switch (attackPos)
    //    {
    //        // 0이 입력되면 왼팔
    //        case 0:
    //            golemAni.SetTrigger("isLeftAttack");                // 왼팔 애니메이터 동작
    //            obj.transform.position = LHand.transform.position;  // 왼손 위치에 투사체 소환
    //            obj.transform.parent = LHand.transform;             // 애니메이션에 따른 오브젝트 이동을 위해 잠시 왼손에 종속
    //            obj.SetActive(true);                                // 소환된 투사체 활성화

    //            yield return new WaitForSeconds(0.95f);     // 왼손이 내지르는데 걸리는 시간

    //            obj.transform.parent = null;                // 왼손을 내지르면 투사체 종속 해제

    //            // { 투사체의 포물선 운동 동작
    //            shoot = objBomb.GetVelocity(obj.transform.position, player.transform.position, 30f);
    //            objRigid.velocity = shoot;
    //            // } 투사체의 포물선 운동 동작
    //            break;

    //        // 1이 입력되면 오른팔
    //        case 1:
    //        golemAni.SetTrigger("isRightAttack");                   // 오른팔 애니메이터 동작
    //            obj.transform.position = RHand.transform.position;  // 오른손 위치에 투사체 소환
    //            obj.transform.parent = RHand.transform;             // 애니메이션에 따른 오브젝트 이동을 위해 잠시 오른손에 종속
    //            obj.SetActive(true);                                // 소환된 투사체 활성화

    //            yield return new WaitForSeconds(1.5f);      // 오른손이 내지르는데 걸리는 시간

    //            obj.transform.parent = null;                // 오른손을 내지르면 투사체 종속 해제

    //            // { 투사체의 포물선 운동 동작
    //            shoot = objBomb.GetVelocity(obj.transform.position, player.transform.position, 30f);
    //            objRigid.velocity = shoot;
    //            // } 투사체의 포물선 운동 동작
    //            break;
    //    }

    //    // 원거리 공격의 쿨타임
    //    yield return new WaitForSeconds(5f);

    //    // 쿨타임이 끝나면 공격행동 재진입을 위해 불값 초기화
    //    isAttack = false;

    //    // 원거리 공격 코루틴 재진입을 위해 코루틴 초기화
    //    throwball = ThrowBall();
    //}

    //IEnumerator SpawnMinion22()
    //{
    //    Vector3 spawnPoint = Vector3.zero;

    //    GameObject minion = null;

    //    for(int i = 0; i < 10; i++)
    //    {
    //        int randomMinion = Random.Range(0,2);

    //        switch(randomMinion)
    //        {
    //            case 0:
    //                minion = ObjectPoolManager.instance.GetPoolObj(PoolObjType.MINION_BASIC);
    //                break;
    //            case 1:
    //                minion = ObjectPoolManager.instance.GetPoolObj(PoolObjType.MINION_BOMB);
    //                break;
    //        }

    //        spawnPoint.x = MinionSpawn.transform.position.x + Random.Range(-20f, 20f);
    //        spawnPoint.z = MinionSpawn.transform.position.z + Random.Range(-3f, -1f);
    //        spawnPoint.y = MinionSpawn.transform.position.y;

    //        minion.transform.position = spawnPoint;
    //        minion.SetActive(true);

    //    }

    //    yield return new WaitForSeconds(5f);

    //    isAttack = false;
    //}

}

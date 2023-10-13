using Oculus.Interaction.PoseDetection.Debug.Editor.Generated;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Golem : MonoBehaviour
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

    public float phase1Time = 5f;      // 페이즈 1 제한시간
    public float phase2Time = 5f;      // 페이즈 2 제한시간
    private Transform player = default; // 플레이어를 캐싱할 변수
    private float firstPos = default;   // 괴수와 플레이어의 최초 거리 캐싱할 변수
    public float golemSpeed = 5f;       // 괴수의 PC 추적 속도
    private bool phaseStart = false;

    private Vector3 target = default;

    // }괴수의 각종 변수

    private Rigidbody golemRigid = default;     // 골렘의 속력을 입력할 컴포넌트
    private Animator golemAni = default;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        golemCheck = Phase.READY;
        golemRigid = GetComponent<Rigidbody>();
        golemAni = GetComponent<Animator>();

        currentHp = golemMaxHp;

        target = (player.transform.position - transform.position).normalized;
        firstPos = Vector3.Distance(transform.position, player.transform.position);

        Debug.Log($"거리계산 : {firstPos}, 거리 비율 : {firstPos * 0.7f}");

        StartCoroutine(GameStart());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            golemRigid.velocity = Vector3.zero;
            golemAni.SetBool("isWalk", false);
            golemCheck = Phase.GAMEOVER;

            Debug.Log("게임 종료");
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnDamageble(10f);
        }

        if (golemCheck == Phase.PHASE_LAST)
        {
            golemRigid.velocity = target * golemSpeed;
        }
    }

    IEnumerator GameStart()
    {
        float gameStartTimer = 0;

        while(gameStartTimer < startTime)
        {
            gameStartTimer += Time.deltaTime;

            Debug.Log($"게임 체크 시간 : {gameStartTimer}");
            yield return null;
        }

        golemAni.SetBool("isWalk", true);

        while (golemCheck == Phase.READY)
        {
            if (firstPos * 0.7f >= Vector3.Distance(transform.position, player.transform.position))
            {
                Debug.Log("페이즈 1 조건 달성 ");
                golemCheck = Phase.PHASE_1;
            }

            golemRigid.velocity = target * golemSpeed;

            yield return null;
        }

        golemRigid.velocity = Vector3.zero;
        golemAni.SetBool("isWalk", false);
        StartCoroutine(Phase1());
    }

    IEnumerator Phase1()
    {
        Debug.Log("페이즈 1 진입");
        float phaseTimer = 0f;

        // 제한시간동안 골렘의 일정 체력을 깎기 전까지는 계속해서 시간을 누적한다
        // 제한시간이 다 되거나 일정체력이 깎이면 while문 탈출
        while(phaseTimer < phase1Time &&  currentHp <= golemMaxHp * 0.7f)
        {
            phaseTimer += Time.deltaTime;

            //if (isAttack == false)
            //{
            //    int Attack = Random.Range(0, 2);

            //    switch (Attack)
            //    {
            //        case 0:
            //            StartCoroutine(ThrowBall());
            //            Attack = 
            //            break;
            //        case 1:
            //            StartCoroutine(SpawnMinion());
            //            break;
            //    }

            //}

            yield return null;
        }

        Debug.Log("페이즈 1 끝");
        golemAni.SetBool("isWalk", true);

        while (golemCheck == Phase.PHASE_1)
        {
            if (firstPos * 0.4f >= Vector3.Distance(transform.position, player.transform.position))
            {
                golemCheck = Phase.PHASE_2;
            }

            golemRigid.velocity = target * golemSpeed;

            yield return null;
        }

        golemRigid.velocity = Vector3.zero;
        golemAni.SetBool("isWalk", false);
        StartCoroutine(Phase2());
        // TODO : 페이즈2 진입
    }

    IEnumerator Phase2()
    {
        Debug.Log("2페이즈 진입");

        float phaseTimer = 0f;

        // 제한시간동안 골렘의 일정 체력을 깎기 전까지는 계속해서 시간을 누적한다
        // 제한시간이 다 되거나 일정체력이 깎이면 while문 탈출
        while (phaseTimer < phase2Time && currentHp <= golemMaxHp * 0.4f)
        {
            phaseTimer += Time.deltaTime;

            //int Attack = Random.Range(0, 2);

            //switch(Attack)
            //{
            //    case 0:
            //        StartCoroutine(ThrowBall());
            //        break;
            //    case 1:
            //        StartCoroutine(SpawnMinion());
            //        break;
            //}

            yield return null;
        }

        golemCheck = Phase.PHASE_LAST;
        golemAni.SetBool("isWalk", true);

        Debug.Log("라스트 페이즈");
    }

    IEnumerator ThrowBall()
    {
        isAttack = true;
        Debug.Log("원거리 공격 시작");
        yield return new WaitForSeconds(5f);

        Debug.Log("원거리 공격 종료");
        isAttack = false;
    }

    IEnumerator SpawnMinion()
    {
        isAttack = true;
        Debug.Log("졸개 소환 시작");
        yield return new WaitForSeconds(5f);

        Debug.Log("졸개 소환 종료");
        isAttack = false;
    }

    private void OnDamageble(float damage)
    {
        currentHp -= damage;

        Debug.Log($"괴수 현재 체력 : {currentHp}");
        if(currentHp <= 0)
        {
            StopAllCoroutines();
            golemCheck = Phase.GAMEOVER;
        }
    }
}

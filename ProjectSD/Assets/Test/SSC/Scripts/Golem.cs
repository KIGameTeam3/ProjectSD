using Oculus.Interaction.PoseDetection.Debug.Editor.Generated;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // {������ ����� üũ�� enum ������Ʈ
    public enum Phase 
    {  
        READY,
        PHASE_1, 
        PHASE_2,  
        PHASE_LAST, 
        GAMEOVER
    }

    public Phase golemCheck { get; private set;}
    // }������ ����� üũ�� enum ������Ʈ

    private bool isAttack = false;

    // {������ ���� ����

    public float golemMaxHp = 100f;     // ������ �ʱ� ü��
    private float currentHp = default;   // ������ ���� ü��    
    public float startTime = 5f;        // ���� ���۽ð� üũ
                                        // TODO : ���Ŀ��� ���ӽ��� ��ư�� ������ ������ �ൿ���� ����

    public float phase1Time = 5f;      // ������ 1 ���ѽð�
    public float phase2Time = 5f;      // ������ 2 ���ѽð�
    private Transform player = default; // �÷��̾ ĳ���� ����
    private float firstPos = default;   // ������ �÷��̾��� ���� �Ÿ� ĳ���� ����
    public float golemSpeed = 5f;       // ������ PC ���� �ӵ�
    private bool phaseStart = false;

    private Vector3 target = default;

    // }������ ���� ����

    private Rigidbody golemRigid = default;     // ���� �ӷ��� �Է��� ������Ʈ
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

        Debug.Log($"�Ÿ���� : {firstPos}, �Ÿ� ���� : {firstPos * 0.7f}");

        StartCoroutine(GameStart());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            golemRigid.velocity = Vector3.zero;
            golemAni.SetBool("isWalk", false);
            golemCheck = Phase.GAMEOVER;

            Debug.Log("���� ����");
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

            Debug.Log($"���� üũ �ð� : {gameStartTimer}");
            yield return null;
        }

        golemAni.SetBool("isWalk", true);

        while (golemCheck == Phase.READY)
        {
            if (firstPos * 0.7f >= Vector3.Distance(transform.position, player.transform.position))
            {
                Debug.Log("������ 1 ���� �޼� ");
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
        Debug.Log("������ 1 ����");
        float phaseTimer = 0f;

        // ���ѽð����� ���� ���� ü���� ��� �������� ����ؼ� �ð��� �����Ѵ�
        // ���ѽð��� �� �ǰų� ����ü���� ���̸� while�� Ż��
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

        Debug.Log("������ 1 ��");
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
        // TODO : ������2 ����
    }

    IEnumerator Phase2()
    {
        Debug.Log("2������ ����");

        float phaseTimer = 0f;

        // ���ѽð����� ���� ���� ü���� ��� �������� ����ؼ� �ð��� �����Ѵ�
        // ���ѽð��� �� �ǰų� ����ü���� ���̸� while�� Ż��
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

        Debug.Log("��Ʈ ������");
    }

    IEnumerator ThrowBall()
    {
        isAttack = true;
        Debug.Log("���Ÿ� ���� ����");
        yield return new WaitForSeconds(5f);

        Debug.Log("���Ÿ� ���� ����");
        isAttack = false;
    }

    IEnumerator SpawnMinion()
    {
        isAttack = true;
        Debug.Log("���� ��ȯ ����");
        yield return new WaitForSeconds(5f);

        Debug.Log("���� ��ȯ ����");
        isAttack = false;
    }

    private void OnDamageble(float damage)
    {
        currentHp -= damage;

        Debug.Log($"���� ���� ü�� : {currentHp}");
        if(currentHp <= 0)
        {
            StopAllCoroutines();
            golemCheck = Phase.GAMEOVER;
        }
    }
}

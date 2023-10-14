using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MinionBase : MonoBehaviour
{
    private Transform player = default;
    private Rigidbody myRigid = default;
    private WaitForSeconds readyTime = new WaitForSeconds(1f);
    private Vector3 target = Vector3.zero;
    public Animator myAni = default;
    private bool isDetected = false;
    private float distance = default;

    public float minionSpeed = 5f;
    public bool isAttack = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        target = (player.transform.position - transform.position).normalized;
        myRigid = GetComponent<Rigidbody>();
        distance = Vector3.Distance(transform.position, player.transform.position);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.Log($"벡터값으로 확인하기 : {player.transform.position - transform.position}");
        transform.LookAt(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= distance * 0.13f)
        {
            myRigid.velocity = Vector3.zero;
            isDetected = false;
            myAni.SetBool("isWalk", false);
            isAttack = true;
            return;
        }

        if(isDetected == true)
        {
            myAni.SetBool("isWalk", true);
            myRigid.velocity = target * minionSpeed;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DetectedStart());
    }

    IEnumerator DetectedStart()
    {
        yield return readyTime;
        isDetected = true;
    }
}

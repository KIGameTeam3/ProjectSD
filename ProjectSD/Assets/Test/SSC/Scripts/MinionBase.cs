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
    private Animator myAni = default;
    private bool isAttack = false;
    private float distance = default;

    public float minionSpeed = 5f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        target = (player.transform.position - transform.position).normalized;
        myRigid = GetComponent<Rigidbody>();
        myAni = GetComponent<Animator>();
        distance = Vector3.Distance(transform.position, player.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= distance * 0.13f)
        {
            myRigid.velocity = Vector3.zero;
            return;
        }

        if(isAttack == true)
        {
            myAni.SetTrigger("isWalk");
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
        isAttack = true;
    }
}

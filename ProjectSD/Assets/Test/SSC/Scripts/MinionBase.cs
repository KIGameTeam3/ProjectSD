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
    private bool isLimit = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        myRigid = GetComponent<Rigidbody>();
        distance = Vector3.Distance(transform.position, player.transform.position);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(isLimit == true)
        {
            return;
        }
        transform.LookAt(player.transform.position);
        target = (player.transform.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, player.transform.position) <= distance * 0.13f)
        {
            myRigid.velocity = Vector3.zero;
            //myRigid.isKinematic = true;
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DeadZone"))
        {
            myRigid.velocity = Vector3.zero;
            isLimit = true;
        }
    }

    protected virtual IEnumerator CoolObj(GameObject obj, PoolObjType type)
    {
        yield return new WaitForSeconds(3f);

        ObjectPoolManager.instance.CoolObj(obj, type);
    }

    private void OnEnable()
    {
        isLimit = false;
        StartCoroutine(DetectedStart());
    }

    IEnumerator DetectedStart()
    {
        yield return readyTime;
        isDetected = true;
    }
}

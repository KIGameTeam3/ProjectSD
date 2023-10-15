using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MinionBasic : MinionBase
{
    public float attackSpeed = 3f;
    private float timeReset = 0f;
    private bool atkReset = false;

    [SerializeField] private BoxCollider atkRange;


    protected override void Update()
    {
        base.Update();

        if (isAttack == true)
        {
            timeReset += Time.deltaTime;

            if (attackSpeed <= timeReset && atkReset == false)
            {
                atkReset = true;
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

        if (other.CompareTag("DeadZone"))
        {
            StartCoroutine(CoolObj(this.gameObject, PoolObjType.MINION_BASIC));
        }
    }

    private void Attack()
    {
        atkRange.enabled = true;
        atkRange.enabled = false;
    }

    IEnumerator AttackCooltime()
    {
        yield return new WaitForSeconds(1f);
        atkReset = false;
    }
}

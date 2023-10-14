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
                        StartCoroutine(LeftAttack());
                        break;
                    case 1:
                        StartCoroutine(RightAttack());
                        break;
                }

            }
        }

    }

    IEnumerator LeftAttack()
    {
        myAni.SetTrigger("AttackLeft");
        yield return new WaitForSeconds(1f);

        atkRange.enabled = true;
        atkRange.enabled = false;

        atkReset = false;
        timeReset = 0f;

    }

    IEnumerator RightAttack()
    {
        myAni.SetTrigger("AttackRight");
        yield return new WaitForSeconds(1.5f);

        atkRange.enabled = true;
        atkRange.enabled = false;

        atkReset = false;
        timeReset = 0f;

    }
}

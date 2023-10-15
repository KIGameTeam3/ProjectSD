using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBomb : MinionBase
{
    //protected override void Update()
    //{
    //    base.Update();
    //}

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("DeadZone"))
        {
            StartCoroutine(CoolObj(this.gameObject, PoolObjType.MINION_BOMB));
        }
    }
}

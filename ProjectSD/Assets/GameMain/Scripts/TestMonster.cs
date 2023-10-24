using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour, IHitObject
{
    int damage = 0;
    public void Hit(float damage)
    {
        this.damage += (int)Mathf.Round(damage);
        Debug.Log(this.damage);

    }
}

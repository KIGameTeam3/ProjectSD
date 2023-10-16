using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBulletBase : MonoBehaviour
{
    [SerializeField]
    protected GunBulletStatus status;
    protected Rigidbody bulletRigidbody;
    protected Collider bulletCollider;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        status = Instantiate(status);

        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
        bulletRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        bulletCollider.isTrigger = true;
        Destroy(gameObject, status.lifeTime);
    }

    public void Move(Vector3 direction)
    {
        bulletRigidbody.velocity = direction * status.bulletSpeed*0.2f;
    }

    protected float GetDamage()
    {
        float criticalChance = Random.Range(0f,100f);
        float criticalMultiple = 1;
        if(criticalChance<= status.criticalChance)
        {
            criticalMultiple = status.criticalMultiple;
        }

        return status.bulletDamage*(1+criticalMultiple);
    }

    public float GetRate()
    {
        return status.fireRate;
    }
    private void OnTriggerEnter(Collider other)
    {
        IHitObject enemy = other.GetComponent<IHitObject>();
        if (enemy != null)
        {
            enemy.Hit(GetDamage());
        }
    }
}

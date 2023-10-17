using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBulletBase : MonoBehaviour
{
    [SerializeField]
    protected GunBulletStatus status;
    protected Rigidbody bulletRigidbody;
    protected Collider bulletCollider;

    private AudioSource bulletAudioSource;
    private ParticleSystem bulletParticle;

    private bool isAttack = false;


    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        status = Instantiate(status);
        bulletAudioSource = GetComponent<AudioSource>();
        bulletParticle = GetComponent<ParticleSystem>();
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
        bulletRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        bulletCollider.isTrigger = true;
        Invoke("AutoRemove", status.lifeTime);
    }

    public void Move(Vector3 direction)
    {
        bulletRigidbody.velocity = direction * status.bulletSpeed*0.3f;
    }

    protected void Remove()
    {
        transform.localScale = Vector3.zero;
        Destroy(gameObject, 1f);
    }

    protected void AttackReaction()
    {
        isAttack = true;
        bulletAudioSource.Play();
        bulletParticle.Play();
        Remove();
    }

    protected void AutoRemove()
    {
        if (!isAttack)
        {
            Remove();
        }
    }

    protected float GetDamage(bool isCritical)
    {
        /*
        float criticalChance = Random.Range(0f,100f);
        float criticalMultiple = 1;
        if(criticalChance<= status.criticalChance)
        {
            criticalMultiple = status.criticalMultiple;
        }

        return status.bulletDamage*(1+criticalMultiple);
        */

        if (isCritical)
        {
            return status.bulletDamage * (1 + status.criticalRate);
        }
        else
        {
            return status.bulletDamage;
        }
    }

    public float GetRate()
    {
        return status.fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        IHitObject enemy = other.GetComponent<IHitObject>();
        if (enemy != null && !isAttack)
        {
            enemy.Hit(GetDamage(!other.gameObject.CompareTag("Untagged")));
            AttackReaction();
        }
    }
}

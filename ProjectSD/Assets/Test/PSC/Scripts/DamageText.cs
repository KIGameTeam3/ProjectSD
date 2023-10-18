using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    TMP_Text mText;
    int damage = 0;
    Camera m_Camera;

    float lifeTime = 3f;
    float speed = 10;

    private void Awake()
    {
        mText = GetComponent<TMP_Text>();
        m_Camera = Camera.main;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
        mText.text = damage.ToString();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveUpRoutine());
    }

    private void Update()
    {
        transform.forward = m_Camera.transform.forward;
    }

    public void SetTextSize(float size)
    {
        mText.fontSize = size;
    }

    IEnumerator MoveUpRoutine()
    {
        float time = 0;
        while(time<= lifeTime)
        {
            transform.position = transform.position + speed * Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        Destroy(transform.parent?.gameObject);
        Destroy(gameObject);
    }

}

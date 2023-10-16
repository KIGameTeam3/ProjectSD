using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTest : MonoBehaviour
{
    public GameObject bullet;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}

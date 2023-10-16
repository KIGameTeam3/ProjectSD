using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public GunBulletBase defaultBullet;
    public GunBulletBase enhanceBullet;
    public HandPosition handPosition;
    public LaserPoint point;

    private ARAVRInput.Controller controller;
    private bool isEnhance = false;
    private bool canShot = true;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, controller) && canShot)
        {
            GunBulletBase currBullet;
            if (!isEnhance)
            {
                currBullet = Instantiate(defaultBullet, point.startPos.position, Quaternion.identity);
            }
            else
            {
                currBullet = Instantiate(enhanceBullet, point.startPos.position, Quaternion.identity);
            }


            if (handPosition == HandPosition.RIGHT)
            {
                currBullet.Move(ARAVRInput.RHandDirection);
            }
            else
            {
                currBullet.Move(ARAVRInput.LHandDirection);
            }

            StartCoroutine(GunDelayRoutine(currBullet.GetRate()));
        }
    }

    void Init()
    {

        if (handPosition == HandPosition.RIGHT)
        {
            controller = ARAVRInput.Controller.RTouch;
        }
        else
        {
            controller = ARAVRInput.Controller.LTouch;
        }
    }

    IEnumerator GunDelayRoutine(float time)
    {
        canShot = false;
        yield return new WaitForSeconds(time);
        canShot = true;
    }

}

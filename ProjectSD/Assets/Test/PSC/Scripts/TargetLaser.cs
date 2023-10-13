using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLaser : MonoBehaviour
{

    LineRenderer lineRenderer;
    HandPosition handPosition;
    enum HandPosition
    {
        RIGHT,
        LEFT
    }

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = 2;
        //lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if()
        lineRenderer.SetPosition(0, ARAVRInput.RHandPosition);
        lineRenderer.SetPosition(1, ARAVRInput.RHandPosition+(ARAVRInput.RHandDirection * 100));
        if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
        }
    }
}

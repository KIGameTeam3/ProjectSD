using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLaser : MonoBehaviour
{

    private LineRenderer lineRenderer;
    public HandPosition handPosition;

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
        if(handPosition == HandPosition.RIGHT)
        {
            lineRenderer.SetPosition(0, ARAVRInput.RHandPosition);
            lineRenderer.SetPosition(1, ARAVRInput.RHandPosition + (ARAVRInput.RHandDirection * 1000));
            if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
            {
            }
        }

        else if (handPosition == HandPosition.LEFT)
        {
            lineRenderer.SetPosition(0, ARAVRInput.LHandPosition);
            lineRenderer.SetPosition(1, ARAVRInput.LHandPosition + (ARAVRInput.LHandDirection * 1000));
            if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
            {
            }
        }

    }
}
public enum HandPosition
{
    RIGHT,
    LEFT
}

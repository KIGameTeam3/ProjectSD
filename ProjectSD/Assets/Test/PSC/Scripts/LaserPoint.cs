using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserPoint : MonoBehaviour
{
    #region 상수
    const float LASER_LENGTH = 1000;
    #endregion

    #region private 변수
    private LineRenderer lineRenderer;
    private Material originMaterial;
    private Material cloneMaterial;
    #endregion

    #region public 변수
    public HandPosition handPosition;
    public GameObject hitPointer;
    #endregion


    void Awake()
    {
        Init();
    }

    void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position;
        ARAVRInput.Controller controller = ARAVRInput.Controller.RTouch;

        ChangeControllerData(out startPos, out endPos, out controller);

        lineRenderer.SetPosition(0, startPos);

        Ray ray = new Ray(startPos, endPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hitPointer.SetActive(true);
            hitPointer.transform.localScale = Vector3.one* Vector3.Distance(endPos,hit.point)* 0.1f;
            hitPointer.transform.position = hit.point;
            endPos = hit.point;
            
        }
        else if(hitPointer.activeSelf)
        {
            hitPointer.SetActive(false);
        }

        lineRenderer.SetPosition(1, endPos);
        if (ARAVRInput.Get(ARAVRInput.Button.IndexTrigger, controller))
        {
            cloneMaterial.color = Color.blue;
        }
        else if(ARAVRInput.GetUp(ARAVRInput.Button.IndexTrigger, controller))
        {
            cloneMaterial.color =  originMaterial.color;
        }
    }

    private void Init()
    {
        hitPointer = Instantiate(hitPointer, transform.parent);
        lineRenderer = GetComponentInChildren<LineRenderer>();
        originMaterial = lineRenderer.material;
        cloneMaterial = Instantiate(originMaterial);

        lineRenderer.material = cloneMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.015f;
    }

    private void ChangeControllerData(out Vector3 startPos, out Vector3 endPos, out ARAVRInput.Controller controller)
    {
        if (handPosition == HandPosition.RIGHT)
        {
            startPos = ARAVRInput.RHandPosition;
            endPos = startPos + (ARAVRInput.RHandDirection * LASER_LENGTH);
            controller = ARAVRInput.Controller.RTouch;
            return;
        }

        else if (handPosition == HandPosition.LEFT)
        {
            startPos = ARAVRInput.LHandPosition;
            endPos = startPos + (ARAVRInput.LHandDirection * LASER_LENGTH);
            controller = ARAVRInput.Controller.LTouch;
            return;
        }

        else
        {
            startPos = transform.position;
            endPos = transform.position;
            controller = ARAVRInput.Controller.RTouch;
            return;
        }

    }

}
public enum HandPosition
{
    RIGHT,
    LEFT
}

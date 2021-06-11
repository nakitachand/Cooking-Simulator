using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum RotationAxis
{
    X,
    Y,
    Z
}

public class Door : XRSimpleInteractable
{
    [SerializeField]
    private Transform doorPivot;            //starting position of object

    [SerializeField]
    private Transform movablePart;          //object that will move

    [SerializeField]
    private RotationAxis doorRotationAxis;  //user-defined rotational axis of object

    [SerializeField]
    private Vector3 localStartDirection;    //starting position vector of object

    [SerializeField]
    private Vector3 localEndDirection;      //ending position vector of object

    [SerializeField]
    private bool reverseDirection;

    private float maxLocalAngle;

    private Quaternion initialLocalRotation;

    private Transform currentInteractorTransform = null;

    [SerializeField]
    protected ProxyHandsVisuals proxyHand;


    // Start is called before the first frame update
    protected override void Awake()
    {
        onSelectEntered.AddListener(OnBeginInteraction);
        onSelectExited.AddListener(OnEndInteraction);

        initialLocalRotation = movablePart.localRotation;
        maxLocalAngle = Vector3.Angle(localStartDirection, localEndDirection);

        if(reverseDirection)
        {
            maxLocalAngle *= -1;
        }

        base.Awake();
    }

    
    private void OnBeginInteraction(XRBaseInteractor interactor)
    {
        
        currentInteractorTransform = interactor.transform;
        proxyHand.EnableProxyHandVisual(interactor.GetComponent<ActionBasedController>(), interactor);
        
        //interactor.GetComponent<XRBaseController>().hideControllerModel = true;
        //proxyHand.Activate();
    }

    private void OnEndInteraction(XRBaseInteractor interactor)
    {
        proxyHand.DisableProxyHandVisual();
        currentInteractorTransform = null;
        //interactor.GetComponent<XRBaseController>().hideControllerModel = false;
        //proxyHand.Deactivate();
    }

    private void Interacting()
    {
        
        Vector3 doorDirection = ConvertWorldPointtoLocalDirection(currentInteractorTransform.transform.position);
        Vector3 startDirection = doorPivot.TransformDirection(localStartDirection);
        
        float currentAngle = Vector3.Angle(doorDirection, startDirection);

        Vector3 perpendicularToDoorAngle = Vector3.Cross(startDirection, doorDirection);

        float cosine = Vector3.Dot(perpendicularToDoorAngle, doorPivot.TransformDirection(AxisDirectionToVector(doorRotationAxis)));
        
        if(cosine < 0)
        {
            currentAngle *= -1;
        }

        if(maxLocalAngle > 0)
        {
            SetPercentageOpen(Mathf.Clamp(currentAngle, 0, maxLocalAngle) / maxLocalAngle);
        }
        else
        {
            SetPercentageOpen(Mathf.Clamp(currentAngle, maxLocalAngle, 0) / maxLocalAngle);
        }
    }

    private void SetPercentageOpen(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        Vector3 pivotAxis = doorPivot.TransformDirection(AxisDirectionToVector(doorRotationAxis));
        movablePart.localRotation = initialLocalRotation;
        movablePart.RotateAround(doorPivot.position, pivotAxis, percentage * maxLocalAngle);
    }

    
    //pass in world point of interactor's location and returns projected vector on the plane of motion
    private Vector3 ConvertWorldPointtoLocalDirection(Vector3 point)
    {
        Vector3 pivotAxis = doorPivot.TransformDirection(AxisDirectionToVector(doorRotationAxis).normalized);
        return Vector3.ProjectOnPlane(point - doorPivot.position, pivotAxis).normalized;
    }

    private Vector3 AxisDirectionToVector(RotationAxis axis)
    {
        Vector3 returnValue = Vector3.up;

        switch (axis)
        {
            case RotationAxis.X:
                returnValue = Vector3.right;
                break;
            case RotationAxis.Y:
                returnValue = Vector3.up;
                break;
            case RotationAxis.Z:
                returnValue = Vector3.forward;
                break;
        }

        return returnValue;
    }

    public void Update()
    {
        if (currentInteractorTransform != null)
        {
            Interacting();
        }
    }
}


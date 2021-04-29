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
    private Transform doorPivot;

    [SerializeField]
    private Transform movablePart;

    [SerializeField]
    private RotationAxis doorRotationAxis;

    [SerializeField]
    private Vector3 localStartDirection;

    [SerializeField]
    private Vector3 localEndDirection;

    [SerializeField]
    private bool reverseDirection;

    private float maxLocalAngle;

    private Quaternion initialLocalRotation;

    private Transform currentInteractorTransform = null;

    [SerializeField]
    private ProxyHand proxyHand;


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
        interactor.GetComponent<XRBaseController>().hideControllerModel = true;
        proxyHand.Activate();
    }

    private void OnEndInteraction(XRBaseInteractor interactor)
    {
        
        currentInteractorTransform = null;
        interactor.GetComponent<XRBaseController>().hideControllerModel = false;
        proxyHand.Deactivate();
    }

    private void Interacting()
    {
        
        Vector3 doorDirection = ConvertWorldPointtoLocalDirection(currentInteractorTransform.position);
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
            SetPercentageOpen(Mathf.Clamp(currentAngle, 0, maxLocalAngle) / maxLocalAngle);
        }
    }

    private void SetPercentageOpen(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        Vector3 pivotAxis = doorPivot.TransformDirection(AxisDirectionToVector(doorRotationAxis));
        movablePart.localRotation = initialLocalRotation;
        movablePart.RotateAround(doorPivot.position, pivotAxis, percentage * maxLocalAngle);
    }

    
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


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class ProxyHandsVisuals //: MonoBehaviour
{
    [SerializeField]
    [Tooltip("A reference to the right hand ProxyHandVisual component. \nSet this if you want to display a geometric matched hand when the proxy is grabbed")]
    private ProxyHand rightProxyHandVisuals;

    [SerializeField]
    [Tooltip("A reference to the left hand ProxyHandVisual component. \nSet this if you want to display a geometric matched hand when the proxy is grabbed")]
    private ProxyHand leftProxyHandVisuals;

    //returns true if rightProxyHandVisuals var has been assigned
    private bool HasProxyVisuals => rightProxyHandVisuals != null;

    //references to the controller (right or left) & interactor (right or left hand)
    private XRBaseInteractor currentInteractor;
    private ActionBasedController currentController;

    public void EnableProxyHandVisual(ActionBasedController controller, XRBaseInteractor interactor)
    {
        if(!HasProxyVisuals)
        {
            return;
        }

        currentInteractor = interactor;
        currentController = controller;

        GetMatchingControllerProxy().Activate();
        currentController.hideControllerModel = true;
    }

    public void DisableProxyHandVisual()
    {
        if (!HasProxyVisuals)
        {
            return;
        }

        GetMatchingControllerProxy().Deactivate();
        currentController.hideControllerModel = false;
    }

    public void Setup()
    {
        if (HasProxyVisuals)
        {
            rightProxyHandVisuals.Deactivate();
            leftProxyHandVisuals.Deactivate();
        }
    }
    
    //determines which controller is current controller and returns the controller side reference accordingly
    private ProxyHand GetMatchingControllerProxy()
    {
        var isRightController = currentController.activateAction.action.actionMap.name.Contains("Right");
        return isRightController ? rightProxyHandVisuals : leftProxyHandVisuals;
    }
}


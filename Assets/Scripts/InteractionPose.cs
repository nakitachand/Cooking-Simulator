using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionPose : MonoBehaviour
{
    [SerializeField]
    private HandPoses interactionPose;

    private XRBaseInteractable grabInteractable;

    private bool hideController = false;

    // Start is called before the first frame update
    private void Awake()
    {
        grabInteractable = GetComponent<XRBaseInteractable>();
    }

    void Start()
    {
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
    }

    public void OnGrab(XRBaseInteractor interactor)
    {
        interactor.GetComponent<XRBaseController>().hideControllerModel = hideController;
        ChangePose(interactionPose, interactor);
    }

    public void OnRelease(XRBaseInteractor interactor)
    {
        interactor.GetComponent<XRBaseController>().hideControllerModel = false;
        ChangePose(HandPoses.NoPose, interactor);
    }

    public void ChangePose(HandPoses newPose, XRBaseInteractor interactor)
    {
        HandVisuals visuals = interactor.GetComponentInChildren<HandVisuals>();
        if (visuals != null)
        {
            visuals.LockPose(newPose);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



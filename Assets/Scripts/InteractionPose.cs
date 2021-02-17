using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionPose : MonoBehaviour
{
    [SerializeField]
    private HandPoses interactionPose;

    private XRBaseInteractable grabInteractable;



    // Start is called before the first frame update
    private void Awake()
    {
        grabInteractable = GetComponent<XRBaseInteractable>();
    }

    private void Start()
    {
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
    }

    public void OnGrab(XRBaseInteractor interactor)
    {
        ChangePose(interactionPose, interactor);
    }

    public void OnRelease(XRBaseInteractor interactor)
    {
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


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class InteractionPose : MonoBehaviour
//{

//    [SerializeField]
//    private HandPoses interactionPose;

//    private XRGrabInteractable grabInteractible;

//    private void Awake()
//    {
//        grabInteractible = GetComponent<XRGrabInteractable>();
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        grabInteractible.onSelectEnter.AddListener(OnGrab);
//        grabInteractible.onSelectExit.AddListener(OnRelease);
//    }

//    public void OnGrab(XRBaseInteractor interactor)
//    {

//        ChangePose(interactionPose, interactor);
//    }

//    public void OnRelease(XRBaseInteractor interactor)
//    {
//        ChangePose(HandPoses.NoPose, interactor);
//    }

//    private void ChangePose(HandPoses newPose, XRBaseInteractor interactor)
//    {
//        HandVisuals visuals = interactor.GetComponentInChildren<HandVisuals>();
//        if (visuals != null)
//        {
//            visuals.LockPose(newPose);
//        }
//    }

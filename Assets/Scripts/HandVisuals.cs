﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HandPoses
{
    NoPose,
    SoftGrab = 20,
    MediumGrab = 21,
    HardGrab = 22,
    SoftPinch = 30,
    MediumPinch = 31,
    HardPinch = 32
}

public class HandVisuals : MonoBehaviour
{
    protected static readonly int LockPoseHash = Animator.StringToHash("LockedPose");
    protected static readonly int ControllerSelectHash = Animator.StringToHash("ControllerSelectValue");

    protected Animator animator;

    [SerializeField]
    private InputActionProperty flex;

    // Start is called before the first frame update
    protected void Awake()
    {
        animator = this.GetComponent<Animator>();
        
    }

    //Update is called once per frame
    public void Start()
    {
        //    //LockPose(HandPoses.MediumGrab);
    }

public void Update()
    {
        SetAnimatorInputValue(flex.action, ControllerSelectHash);
    }

    private void SetAnimatorInputValue(InputAction action, int hashAnimator)
    {
        if(action != null)
        {
            animator.SetFloat(hashAnimator, action.ReadValue<float>());
        }
    }

    public void LockPose(HandPoses newPose)
    {
        animator.SetInteger(LockPoseHash, (int)newPose);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using Valve.VR;

enum OverrideTarget
{
    Tracker,
    Fallback
}

public class IKOverrideManager : MonoBehaviour
{
    [SerializeField] GameObject rootPoseTracker, midPoseTracker, tipPoseTracker, rootBone, midBone, tipBone;

    [SerializeField] RigBuilder rigBuilder;

    [SerializeField] bool overrideTip = false;

    private SteamVR_Behaviour_Pose rootPoseNode, midPoseNode, tipPoseNode;
    private TwoBoneIKConstraint localIK;
    private ParentConstraint rootOverrideScript, midOverrideScript, tipOverrideScript;
    private Transform rootBoneTransform, midBoneTransform, tipBoneTransform;

    private void SwapTargets(ParentConstraint overrideScript, OverrideTarget overrideTarget)
    {
        ConstraintSource[] sources = new ConstraintSource[2];
        sources[(int)OverrideTarget.Tracker] = overrideScript.GetSource((int)OverrideTarget.Tracker);
        sources[(int)OverrideTarget.Fallback] = overrideScript.GetSource((int)OverrideTarget.Fallback);
        switch (overrideTarget)
        {
            case OverrideTarget.Tracker:
                sources[(int)OverrideTarget.Tracker].weight = 1;
                sources[(int)OverrideTarget.Fallback].weight = 0;
                break;
            case OverrideTarget.Fallback:
                sources[(int)OverrideTarget.Fallback].weight = 1;
                sources[(int)OverrideTarget.Tracker].weight = 0;
                break;
        }
        overrideScript.SetSources(new List<ConstraintSource>(sources));
        overrideScript.constraintActive = true;
    }

    private void EnableAndCalibrate(ParentConstraint overrideScript, SteamVR_Behaviour_Pose poseNode, bool calibrateOverride)
    {
        SwapTargets(overrideScript, OverrideTarget.Tracker);
        // TODO: This might be need removing because spinning the tracker around its axis fixes stuff?
        // if (calibrateOverride)
        // {
        //     Debug.Log(Vector3.zero - poseNode.transform.rotation.eulerAngles);
        //     overrideScript.SetRotationOffset((int)OverrideTarget.Tracker, Vector3.zero - poseNode.transform.rotation.eulerAngles);
        // }
        overrideScript.constraintActive = true;
    }


    private void ModelReset()
    {
        rootBoneTransform.localPosition = rootOverrideScript.translationAtRest;
        midBoneTransform.localPosition = midOverrideScript.translationAtRest;
        tipBoneTransform.localPosition = tipOverrideScript.translationAtRest;
        rootBoneTransform.localRotation = midBoneTransform.localRotation = tipBoneTransform.localRotation = Quaternion.Euler(0, 0, 0);
        rigBuilder.Build();
    }

    private void ConfigureIK(bool calibrateOverride = false)
    {
        if (!rootPoseNode.isValid && !midPoseNode.isValid && !overrideTip)
        {
            rootOverrideScript.constraintActive = midOverrideScript.constraintActive = tipOverrideScript.constraintActive = false;
            ModelReset();
            localIK.weight = 1;
            return;
        }

        if (rootPoseNode.isValid || midPoseNode.isValid || overrideTip)
        {
            localIK.weight = 0;
            ModelReset();
            if (!overrideTip) tipOverrideScript.constraintActive = true;
        }

        if (rootPoseNode.isValid) EnableAndCalibrate(rootOverrideScript, rootPoseNode, calibrateOverride);
        else SwapTargets(rootOverrideScript, OverrideTarget.Fallback);
        if (midPoseNode.isValid) EnableAndCalibrate(midOverrideScript, midPoseNode, calibrateOverride);
        else SwapTargets(midOverrideScript, OverrideTarget.Fallback);
        if (overrideTip && tipPoseNode.isValid) EnableAndCalibrate(tipOverrideScript, tipPoseNode, calibrateOverride);
        else if (overrideTip) SwapTargets(tipOverrideScript, OverrideTarget.Fallback);

        rigBuilder.Build();
    }

    public void TriggerCalibration(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        ConfigureIK(true);
    }

    void Start()
    {
        SteamVR_Actions._default.Scale.onStateDown += TriggerCalibration;

        localIK = GetComponent<TwoBoneIKConstraint>();

        rootPoseNode = rootPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        midPoseNode = midPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        if (overrideTip) tipPoseNode = tipPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();

        rootOverrideScript = rootBone.GetComponent<ParentConstraint>();
        midOverrideScript = midBone.GetComponent<ParentConstraint>();
        tipOverrideScript = tipBone.GetComponent<ParentConstraint>();

        rootBoneTransform = rootBone.transform;
        midBoneTransform = midBone.transform;
        tipBoneTransform = tipBone.transform;
    }
}

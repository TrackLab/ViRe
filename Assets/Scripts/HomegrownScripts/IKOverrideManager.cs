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

    [SerializeField] MonoBehaviour scriptToToggle;

    [SerializeField] bool rootNoFallbackWhenNoTracking, midNoFallbackWhenNoTracking, tipNoFallbackWhenNoTracking;

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

    private void EnableAndCalibrate(ParentConstraint overrideScript, SteamVR_Behaviour_Pose poseNode, bool calibrateOverride, bool doSwap)
    {
        if (doSwap) SwapTargets(overrideScript, OverrideTarget.Tracker);
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
        if ((!rootPoseNode || !rootPoseNode.isValid) && (!midPoseNode || !midPoseNode.isValid) && (!tipPoseNode || !tipPoseNode.isValid))
        {
            rootOverrideScript.constraintActive = midOverrideScript.constraintActive = tipOverrideScript.constraintActive = false;
            ModelReset();
            localIK.weight = 1;
            if (scriptToToggle) scriptToToggle.enabled = true;
            return;
        }

        if ((rootPoseNode && rootPoseNode.isValid) || (midPoseNode && midPoseNode.isValid) || (tipPoseNode && tipPoseNode.isValid))
        {
            if (scriptToToggle) scriptToToggle.enabled = false;
            localIK.weight = 0;
            ModelReset();
        }

        if (rootPoseNode && rootPoseNode.isValid) EnableAndCalibrate(rootOverrideScript, rootPoseNode, calibrateOverride, !rootNoFallbackWhenNoTracking);
        else if (rootNoFallbackWhenNoTracking) rootOverrideScript.constraintActive = false;
        else if (rootPoseNode) SwapTargets(rootOverrideScript, OverrideTarget.Fallback);
        else rootOverrideScript.constraintActive = true;

        if (midPoseNode && midPoseNode.isValid) EnableAndCalibrate(midOverrideScript, midPoseNode, calibrateOverride, !midNoFallbackWhenNoTracking);
        else if (midNoFallbackWhenNoTracking) midOverrideScript.constraintActive = false;
        else if (midPoseNode) SwapTargets(midOverrideScript, OverrideTarget.Fallback);
        else midOverrideScript.constraintActive = true;

        if (tipPoseNode && tipPoseNode.isValid) EnableAndCalibrate(tipOverrideScript, tipPoseNode, calibrateOverride, !tipNoFallbackWhenNoTracking);
        else if (tipNoFallbackWhenNoTracking) tipOverrideScript.constraintActive = false;
        else if (tipPoseNode) SwapTargets(tipOverrideScript, OverrideTarget.Fallback);
        else tipOverrideScript.constraintActive = true;

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

        if (rootPoseTracker) rootPoseNode = rootPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        if (midPoseTracker) midPoseNode = midPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        if (tipPoseTracker) tipPoseNode = tipPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();

        rootOverrideScript = rootBone.GetComponent<ParentConstraint>();
        midOverrideScript = midBone.GetComponent<ParentConstraint>();
        tipOverrideScript = tipBone.GetComponent<ParentConstraint>();

        rootBoneTransform = rootBone.transform;
        midBoneTransform = midBone.transform;
        tipBoneTransform = tipBone.transform;
    }
}

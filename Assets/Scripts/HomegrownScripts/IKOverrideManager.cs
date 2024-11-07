using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using Valve.VR;

public class IKOverrideManager : MonoBehaviour
{
    [SerializeField] GameObject rootPoseTracker, midPoseTracker, tipPoseTracker, rootBone, midBone, tipBone;

    [SerializeField] RigBuilder rigBuilder;

    private SteamVR_Behaviour_Pose rootPoseNode, midPoseNode, tipPoseNode;
    private TwoBoneIKConstraint localIK;
    private ParentConstraint rootOverrideScript, midOverrideScript, tipOverrideScript;

    private void ConfigureIK(bool calibrateOverride = false)
    {
        localIK.weight = 1;

        rootOverrideScript.constraintActive = midOverrideScript.constraintActive = tipOverrideScript.constraintActive = false;

        if (rootPoseNode.isValid || midPoseNode.isValid)
        {
            localIK.weight = 0;
            tipOverrideScript.constraintActive = true;
        }
        // if (rootPoseNode.isValid || midPoseNode.isValid || tipPoseNode.isActive) localIK.weight = 0;

        if (rootPoseNode.isValid)
        {
            rootOverrideScript.constraintActive = true;
            if (calibrateOverride)
            {
                // TODO: Can this be done cleaner?
                rootOverrideScript.rotationOffsets[0] = new Vector3(0 - rootPoseNode.transform.rotation.eulerAngles.x, 0 - rootPoseNode.transform.rotation.eulerAngles.y, 0 - rootPoseNode.transform.rotation.eulerAngles.z);
            }
        }

        if (midPoseNode.isValid)
        {
            midOverrideScript.constraintActive = true;
            if (calibrateOverride)
            {
                // TODO: Can this be done cleaner?
                midOverrideScript.rotationOffsets[0] = new Vector3(0 - midPoseNode.transform.rotation.eulerAngles.x, 0 - midPoseNode.transform.rotation.eulerAngles.y, 0 - midPoseNode.transform.rotation.eulerAngles.z);
            }
        }

        // if (tipPoseNode.isValid)
        // {
        //     tipOverrideScript.enabled = true;
        //     if (calibrateOverride)
        //     {
        //         // TODO: Can this be done cleaner?
        //         tipOverrideScript.rotateOffsetVector = new Vector3(0 - tipPoseNode.transform.rotation.eulerAngles.x, 0 - tipPoseNode.transform.rotation.eulerAngles.y, 0 - tipPoseNode.transform.rotation.eulerAngles.z);
        //     }
        // }

        rigBuilder.Build();
    }

    public void HandleNewController(SteamVR_Behaviour_Pose _, SteamVR_Input_Sources __, bool ___)
    {
        ConfigureIK();
    }

    public void TriggerCalibration()
    {
        ConfigureIK(true);
    }

    void Start()
    {
        localIK = GetComponent<TwoBoneIKConstraint>();

        rootPoseNode = rootPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        midPoseNode = midPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();

        rootOverrideScript = rootBone.GetComponent<ParentConstraint>();
        midOverrideScript = midBone.GetComponent<ParentConstraint>();
        tipOverrideScript = tipBone.GetComponent<ParentConstraint>();

        ConfigureIK();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Valve.VR;

public class IKManager : MonoBehaviour
{
    [SerializeField] GameObject rootPoseTracker;
    [SerializeField] GameObject midPoseTracker;
    [SerializeField] GameObject tipPoseTracker;
    [SerializeField] Transform rootBone;
    [SerializeField] Transform midBone;
    [SerializeField] Transform tipBone;

    [SerializeField] RigBuilder rigBuilder;

    // This is a hack to allow for a split IK on a limb
    private ChainIKConstraint defaultLimbIK;
    // private TwoBoneIKConstraint secondaryIK;
    private SteamVR_Behaviour_Pose rootPoseNode;
    private SteamVR_Behaviour_Pose midPoseNode;
    private SteamVR_Behaviour_Pose tipPoseNode;
    // private Transform rootPoseTransform;
    // private Transform midPoseTransform;
    private Transform tipPoseTransform;
    private Transform defaultTipTransform;

    private void ConfigureIK()
    {
        //FIXME: Restoring doesn't work
        defaultLimbIK.data.root = rootBone;
        defaultLimbIK.data.tip = tipBone;
        defaultLimbIK.data.target = defaultTipTransform;
        defaultLimbIK.enabled = true;
        // secondaryIK.enabled = false;

        if (rootPoseNode.isValid && midPoseNode.isValid && tipPoseNode.isValid)
        {
            defaultLimbIK.enabled = false;
            // secondaryIK.enabled = false;
            // Might need to rebuild
            return;
        }

        //FIXME: Freezes various IK elements
        if (rootPoseNode.isValid)
        {
            defaultLimbIK.data.root = midBone;
        }

        // if (midPoseNode.isValid)
        // {
        //     defaultLimbIK.data.root = tipBone;
        //     defaultLimbIK.data.tip = tipBone;
        //     secondaryIK.enabled = true;

        // }

        // if (tipPoseNode.isValid)
        // {
        //     defaultLimbIK.data.target = tipPoseTransform;
        // }

        rigBuilder.Build();
    }

    public void HandleNewController(SteamVR_Behaviour_Pose _, SteamVR_Input_Sources __, bool ___)
    {
        ConfigureIK();
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultLimbIK = GetComponent<ChainIKConstraint>();
        // secondaryIK = GetComponent<TwoBoneIKConstraint>();

        rootPoseNode = rootPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        midPoseNode = midPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        tipPoseNode = tipPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();

        // rootPoseTransform = rootPoseTracker.GetComponent<Transform>();
        // midPoseTransform = midPoseTracker.GetComponent<Transform>();
        tipPoseTransform = tipPoseTracker.GetComponent<Transform>();

        defaultTipTransform = defaultLimbIK.data.target;

        ConfigureIK();
    }
}

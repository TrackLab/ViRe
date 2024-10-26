using UnityEngine;
using UnityEngine.Animations.Rigging;
using Valve.VR;

public class IKOverrideManager : MonoBehaviour
{
    [SerializeField] GameObject rootPoseTracker, midPoseTracker, tipPoseTracker;

    [SerializeField] GameObject rootOverride, midOverride, tipOverride;

    [SerializeField] RigBuilder rigBuilder;

    private SteamVR_Behaviour_Pose rootPoseNode, midPoseNode, tipPoseNode;
    private CopyTrackerToOverride rootOverrideScript, midOverrideScript, tipOverrideScript;

    private void ConfigureIK(bool calibrateOverride = false)
    {
        if (rootOverride) rootOverride.SetActive(false);
        if (midOverride) midOverride.SetActive(false);
        if (tipOverride) tipOverride.SetActive(false);

        if (rootPoseNode.isValid && rootOverride)
        {
            rootOverride.SetActive(true);
            if (calibrateOverride)
            {
                // TODO: Can this be done cleaner?
                rootOverrideScript.offsetVector = new Vector3(0 - rootPoseNode.transform.rotation.eulerAngles.x, 0 - rootPoseNode.transform.rotation.eulerAngles.y, 0 - rootPoseNode.transform.rotation.eulerAngles.z);
            }
        }

        if (midPoseNode.isValid && midOverride)
        {
            midOverride.SetActive(true);
            if (calibrateOverride)
            {
                // TODO: Can this be done cleaner?
                midOverrideScript.offsetVector = new Vector3(0 - midPoseNode.transform.rotation.eulerAngles.x, 0 - midPoseNode.transform.rotation.eulerAngles.y, 0 - midPoseNode.transform.rotation.eulerAngles.z);
            }
        }

        if (tipPoseNode.isValid && tipOverride)
        {
            tipOverride.SetActive(true);
            if (calibrateOverride)
            {
                // TODO: Can this be done cleaner?
                tipOverrideScript.offsetVector = new Vector3(0 - tipPoseNode.transform.rotation.eulerAngles.x, 0 - tipPoseNode.transform.rotation.eulerAngles.y, 0 - tipPoseNode.transform.rotation.eulerAngles.z);
            }
        }

        rigBuilder.Build();
    }

    public void HandleNewController(SteamVR_Behaviour_Pose _, SteamVR_Input_Sources __, bool ___)
    {
        ConfigureIK();
    }

    void Start()
    {
        rootPoseNode = rootPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        midPoseNode = midPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();
        tipPoseNode = tipPoseTracker.GetComponent<SteamVR_Behaviour_Pose>();

        ConfigureIK();
    }
}

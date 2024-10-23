using UnityEngine;
using UnityEngine.Animations.Rigging;
using Valve.VR;

public class IKManager : MonoBehaviour
{
    [SerializeField] GameObject rootPoseTracker;
    [SerializeField] GameObject midPoseTracker;
    [SerializeField] GameObject tipPoseTracker;

    [SerializeField] GameObject rootOverride;
    [SerializeField] GameObject midOverride;
    [SerializeField] GameObject tipOverride;

    [SerializeField] RigBuilder rigBuilder;

    private SteamVR_Behaviour_Pose rootPoseNode;
    private SteamVR_Behaviour_Pose midPoseNode;
    private SteamVR_Behaviour_Pose tipPoseNode;

    private void ConfigureIK()
    {
        rootOverride.SetActive(false);
        // midOverride.SetActive(false);
        // tipOverride.SetActive(false);

        if (rootPoseNode.isValid)
        {
            rootOverride.SetActive(true);
        }

        if (midPoseNode.isValid)
        {
            midOverride.SetActive(true);
        }

        if (tipPoseNode.isValid)
        {
            tipOverride.SetActive(true);
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

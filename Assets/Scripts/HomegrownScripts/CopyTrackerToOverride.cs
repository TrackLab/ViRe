using UnityEngine;
using UnityEngine.Animations.Rigging;
using Valve.VR;

public class CopyTrackerToOverride : MonoBehaviour
{
    public Vector3 offsetVector;

    private OverrideTransform overrideTransform;

    public void UpdateTransform(SteamVR_Behaviour_Pose poseNode, SteamVR_Input_Sources _)
    {
        if (!gameObject.activeSelf) return;

        overrideTransform.data.position = poseNode.transform.position;
        overrideTransform.data.rotation = poseNode.transform.rotation.eulerAngles + offsetVector;
    }

    void Start()
    {
        overrideTransform = GetComponent<OverrideTransform>();
    }
}

using UnityEngine;
using UnityEngine.Animations.Rigging;
using Valve.VR;

public class CopyUpdatingTransform : MonoBehaviour
{
    public float offsetX, offsetY, offsetZ;

    private OverrideTransform overrideTransform;

    public void UpdateTransform(SteamVR_Behaviour_Pose behaviourPose, SteamVR_Input_Sources _)
    {
        if (!gameObject.activeSelf) return;
        Vector3 updatedRotation = behaviourPose.transform.rotation.eulerAngles;

        updatedRotation.x += offsetX;
        updatedRotation.y += offsetY;
        updatedRotation.z += offsetZ;

        overrideTransform.data.position = behaviourPose.transform.position;
        overrideTransform.data.rotation = updatedRotation;
    }

    void Start()
    {
        overrideTransform = GetComponent<OverrideTransform>();
    }
}

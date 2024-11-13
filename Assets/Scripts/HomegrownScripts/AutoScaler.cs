using UnityEngine;
using Valve.VR;

public class AutoScaler : MonoBehaviour
{
    public GameObject head, leftHand, rightHand, playerRoot;
    public SteamVR_Behaviour_Pose leftHandController, rightHandController;
    private float defaultHeadY, defaultLeftHandX, defaultRightHandX, defaultModelX, defaultModelY, defaultModelZ;
    void Start()
    {
        defaultModelX = playerRoot.transform.localScale.x;
        defaultModelY = playerRoot.transform.localScale.y;
        defaultModelZ = playerRoot.transform.localScale.z;

        defaultHeadY = head.transform.position.y;
        defaultLeftHandX = leftHand.transform.position.x;
        defaultRightHandX = rightHand.transform.position.x;

        SteamVR_Actions._default.Scale.onStateDown += vrOnScale;
    }

    private void vrOnScale(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        resize();
    }

    public void resize()
    {
        float newYScale = head.transform.position.y / defaultHeadY;

        float newXScale = 0;
        int activeControllers = 0;
        if (leftHandController.isValid)
        {
            newXScale += leftHand.transform.position.x / defaultLeftHandX;
            activeControllers++;
        }
        if (rightHandController.isValid)
        {
            newXScale += rightHand.transform.position.x / defaultRightHandX;
            activeControllers++;
        }
        if (activeControllers > 0) newXScale /= activeControllers;
        else newXScale = defaultModelX;

        playerRoot.transform.localScale = new Vector3(newXScale, newYScale, defaultModelZ);
    }
}

using UnityEngine;
using Valve.VR;

public class AutoScaler : MonoBehaviour
{
    public float defaultModelHeight = 1.8f;
    public GameObject head, playerRoot;
    public IKOverrideManager iKOverrideManager;

    void Start()
    {
        SteamVR_Actions._default.Scale[SteamVR_Input_Sources.Any].onStateDown += vrOnScale;
    }

    //TODO: Keyboard controls
    private void vrOnScale(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {
        resize();
        iKOverrideManager.TriggerCalibration();
    }

    public void resize()
    {
        float newScale = head.transform.position.y / defaultModelHeight;
        playerRoot.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}

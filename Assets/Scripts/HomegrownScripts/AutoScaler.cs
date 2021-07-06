using UnityEngine;
using Valve.VR;

public class AutoScaler : MonoBehaviour
{
    private ActionReader ctrlLister;
    public float defaultHeight = 1.8f;
    private GameObject head, playerRoot;
    

    void Awake()
    {
        ctrlLister = new ActionReader();
        subscribeControls();
        
        head = GameObject.Find("Main Camera");
        playerRoot = GameObject.Find("metarig");
        
    }

    private void subscribeControls(){
        SteamVR_Actions._default.Scale[SteamVR_Input_Sources.Any].onStateDown += vrOnScale;
        ctrlLister.getInputAction("Keyboard","Scale").performed += onScale;
    }

    private void onScale(UnityEngine.InputSystem.InputAction.CallbackContext ctx){Resize();}
    private void vrOnScale(SteamVR_Action_Boolean action, SteamVR_Input_Sources source){Resize();}

    public void Resize()
    {   
        float newScale = head.transform.position.y / defaultHeight;
        playerRoot.transform.localScale = new Vector3(newScale,newScale,newScale);
    }
}

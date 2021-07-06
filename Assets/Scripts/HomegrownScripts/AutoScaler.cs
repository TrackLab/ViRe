using UnityEngine;
using Valve.VR;

public class AutoScaler : MonoBehaviour
{
    public float defaultHeight = 1.8f;
    private GameObject head, playerRoot;
    

    void Awake()
    {
        subscribeControls();
        
        head = GameObject.Find("Main Camera");
        playerRoot = GameObject.Find("metarig");
        
    }

    private void subscribeControls(){
        SteamVR_Actions._default.Scale[SteamVR_Input_Sources.Any].onStateDown += vrOnScale;
        //TODO: Keyboard controls
    }

    //TODO: Keyboard controls
    private void vrOnScale(SteamVR_Action_Boolean action, SteamVR_Input_Sources source){Resize();}

    public void Resize()
    {   
        float newScale = head.transform.position.y / defaultHeight;
        playerRoot.transform.localScale = new Vector3(newScale,newScale,newScale);
    }
}

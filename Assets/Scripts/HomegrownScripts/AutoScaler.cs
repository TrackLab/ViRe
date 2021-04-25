using UnityEngine;

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
        ctrlLister.getInputAction("XRI BothHands","Scale").performed += onScale;
        ctrlLister.getInputAction("Keyboard","Scale").performed += onScale;
    }

    void OnDisable(){
        ctrlLister.getInputAction("XRI BothHands","Scale").performed -= onScale;
        ctrlLister.getInputAction("Keyboard","Scale").performed -= onScale;
    }

    //This might be avoidable if I ever understand event delegates all the wa
    private void onScale(UnityEngine.InputSystem.InputAction.CallbackContext ctx){Resize();}

    public void Resize()
    {   
        float newScale = head.transform.position.y / defaultHeight;
        playerRoot.transform.localScale = new Vector3(newScale,newScale,newScale);
    }
}

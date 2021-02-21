using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
THIS IS A DEBUG SCRIPT!
Said script lets you control the soon-to-be VR parts of the studio
It should be disabled in the future
*/

public class KeyboardMoveDebug : MonoBehaviour
{
    Transform RHtrans, LHtrans, CamTrans, SpecCamTrans;

    float mainSpeed = 0.01f; // Regular speed
    float camSens = 0.25f; // Mouse sensitivity 
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

    void mouseRotate(Transform target){
        lastMouse = Input.mousePosition - lastMouse ;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
        lastMouse = new Vector3(target.eulerAngles.x + lastMouse.x , target.eulerAngles.y + lastMouse.y, 0);
        target.eulerAngles = lastMouse;
        lastMouse =  Input.mousePosition;
    }

    /* 
    CONTROLS:
    WASD + Page up/down + Mouse rotation
    --------------------------------------------------
    To control left hand, hold 1
    To control head, hold 2
    To control right hand, hold 3
    To control spectator camera, hold 4
    */

    void keyMove(Transform target, bool invert){
        if (Input.GetKey("a")){
            if (invert){
                target.position += target.right * mainSpeed;
            }
            else {
                target.position -= target.right * mainSpeed;
            }
        }
        if (Input.GetKey("d")){
            if (invert){
                target.position -= target.right * mainSpeed;
            }
            else{
                target.position += target.right * mainSpeed;
            }
        }
        if (Input.GetKey("w")){
            target.position += target.forward * mainSpeed;
        }
        if (Input.GetKey("s")){
            target.position -= target.forward * mainSpeed;
        }
        if (Input.GetKey("page down")){
            target.position -= target.up * mainSpeed;
        }
        if (Input.GetKey("page up")){
            target.position += target.up * mainSpeed;
        }
    }
    
    void Start()
    {
        //Finds the controllable object's properties
        RHtrans = GameObject.Find("RightHandController").GetComponent<Transform>();
        LHtrans = GameObject.Find("LeftHandController").GetComponent<Transform>();
        CamTrans = GameObject.Find("VRcam").GetComponent<Transform>();
        SpecCamTrans = GameObject.Find("SpectatorCamPlaceholder").GetComponent<Transform>();
    }

    void Update()
    {
        // LEFT HAND
        if (Input.GetKey("1")){
            mouseRotate(LHtrans);
            keyMove(LHtrans, false);
        }

        // HEAD
        if (Input.GetKey("2")){
            mouseRotate(CamTrans);
            keyMove(CamTrans, false);
        }

        // RIGHT HAND
        if (Input.GetKey("3")){
            mouseRotate(RHtrans);
            keyMove(RHtrans, true);
        }

        // SPECTATOR
        if (Input.GetKey("4")){
            mouseRotate(SpecCamTrans);
            keyMove(SpecCamTrans, false);
        }
    }
}

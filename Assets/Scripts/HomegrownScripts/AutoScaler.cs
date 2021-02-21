using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class AutoScaler : MonoBehaviour
{
    public float defaultHeight = 1.8f;
    // GameObject[] ikList;
    public GameObject Head;
    public GameObject PlayerRoot;
    public GameObject LeftFoot;
    public GameObject LeftHand;
    public GameObject RightFoot;
    public GameObject RightHand;
    public SteamVR_Action_Boolean m_BooleanAction;



    // void Start()
    // {
    //     ikList[0] = GameObject.Find("Head");
    //     ikList[1] = GameObject.Find("LeftFoot");
    //     ikList[2] = GameObject.Find("LeftHand");
    //     ikList[3] = GameObject.Find("RightFoot");
    //     ikList[4] = GameObject.Find("RightHand");
    // }

    void Update()
    {
        if (Input.GetKeyDown("t")){
            Resize();
        }
        if (m_BooleanAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Resize();
        }
    }

    public void Resize()
    {   
        Vector3 ScaleChange = new Vector3(0.01f, 0.01f, 0.01f);
        // Debug.Log(Head.transform.position.y);
        // while  (Head.transform.position.y < 1.8f | Head.transform.position.y > 1.9f)
        // {
        //     Debug.Log("Not Right");

        // }

        if (Head.transform.position.y > 1.9f) {
            while (Head.transform.position.y > 1.9f)
            {
                PlayerRoot.transform.localScale -= ScaleChange;
            }
        }
        
        if (Head.transform.position.y < 1.8f) {
            while (Head.transform.position.y < 1.8f)
            {
                PlayerRoot.transform.localScale += ScaleChange;
            }
        }


        // for (int i = 0; i<=4; i++){ikList[i].GetComponent<MonoBehaviour>().enabled = false;}
        // Head.GetComponent<MonoBehaviour>().enabled = false;
        // LeftFoot.GetComponent<MonoBehaviour>().enabled = false;
        // LeftHand.GetComponent<MonoBehaviour>().enabled = false;
        // RightHand.GetComponent<MonoBehaviour>().enabled = false;
        // RightFoot.GetComponent<MonoBehaviour>().enabled = false;

        // float headHeight = GameObject.Find("VRcam").transform.localPosition.y;
        // float scale = defaultHeight / headHeight;
        // transform.localScale = Vector3.one * scale;

        //         Head.GetComponent<MonoBehaviour>().enabled = true;
        // LeftFoot.GetComponent<MonoBehaviour>().enabled = true;
        // LeftHand.GetComponent<MonoBehaviour>().enabled = true;
        // RightHand.GetComponent<MonoBehaviour>().enabled = true;
        // RightFoot.GetComponent<MonoBehaviour>().enabled = true;
        // for (int i = 0; i<=4; i++){ikList[i].GetComponent<MonoBehaviour>().enabled = true;}
    }
}

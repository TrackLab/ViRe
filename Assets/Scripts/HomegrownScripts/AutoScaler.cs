using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoScaler : MonoBehaviour
{
    //TODO: Add VR controller support
    public float defaultHeight = 1.8f;
    public GameObject head;
    public GameObject playerRoot;

    void Start()
    {
        head = GameObject.Find("Main Camera");
        playerRoot = GameObject.Find("metarig");
    }

    void Update()
    {
        if (Input.GetKeyDown("r")){
            Resize();
        }
    }

    public void Resize()
    {   
        float newScale = head.transform.position.y / defaultHeight;
        playerRoot.transform.localScale = new Vector3(newScale,newScale,newScale);
    }
}

using UnityEngine;
using UnityEngine.XR;

public class VRsimCTRL : MonoBehaviour
{
    /* 
        This is a debug script that creates a workaround a known bug in XR Interaction Toolkit
        It allows for VR headsets to work without removing the VR emulator
        If the package gets an update, test for bugfix
    */
    
    private GameObject emulate;
    void Awake()
    {
        InputDevices.deviceConnected+=c_deviceConnected;
        emulate = GameObject.Find("XR Device Simulator");
    }


    void c_deviceConnected(InputDevice value){
        emulate.SetActive(false);
    }
}

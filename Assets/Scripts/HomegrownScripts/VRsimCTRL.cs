using UnityEngine;
using UnityEngine.XR;

public class VRsimCTRL : MonoBehaviour
{
    /* 
        This is a debug script that creates a workaround a known bug in XR Interaction Toolkit
        It allows for VR headsets to track without removing the VR emulator
        If the package gets an update, test for bugfix

        P.S: This script also disables the "drone" spectator camera, so that the debug view comes from the VR camera
    */
    
    private GameObject emulate, drone;
    void Awake()
    {
        InputDevices.deviceConnected+=onDeviceConnected;
        emulate = GameObject.Find("XR Device Simulator");
        drone = GameObject.Find("SpectatorDrone");
    }


    void onDeviceConnected(InputDevice value){
        emulate.SetActive(true);
        drone.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class StudioController : MonoBehaviour
{
    private new GameObject light;
    FeedbackColor feedlight;
    BVHRecorder recorder;
    public GameObject pauseScreen;
    public SteamVR_Action_Boolean m_BooleanAction;
    
    private void Awake()
    {
        m_BooleanAction = SteamVR_Actions._default.GrabGrip;
    }

    void Start()
    {
        // Initialize the variables used by this script and the path to Documents
        light = GameObject.Find("AirText");
        feedlight = light.GetComponent<FeedbackColor>();
        recorder = GetComponent<BVHRecorder>();
        string recpath = PlayerPrefs.GetString("RecPath", System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents");
        recorder.directory = recpath;
    }

    void ToggleRecording()
    {
        if (recorder.capturing){
            recorder.capturing=false;
            recorder.saveBVH();
            recorder.clearCapture();
            feedlight.OffAir();
            Debug.Log("Recorder OFF & data saved");
        }
        else {
            recorder.capturing=true;
            feedlight.OnAir();
            Debug.Log("Recorder ON");
        }
    }

    void Update()
    {
        // Toggle the state of the recorder
        if (Input.GetKeyDown("e")){
            ToggleRecording();
        }

        if (m_BooleanAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ToggleRecording();
        }

        // Open the pause menu
        if (Input.GetKeyDown("escape")){
            pauseScreen.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class StudioController : MonoBehaviour
{
    private new GameObject light;
    private FeedbackColor feedlight;
    private BVHRecorder recorder;
    public GameObject pauseScreen;
    

    void Awake()
    {
        // Initialize the variables used by this script and the path to Documents
        light = GameObject.Find("AirText");
        feedlight = light.GetComponent<FeedbackColor>();
        recorder = GetComponent<BVHRecorder>();
        string recpath = PlayerPrefs.GetString("RecPath", System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents");
        recorder.directory = recpath;
    }

    public void toggleRecording()
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
            toggleRecording();
        }
        // Open the pause menu
        if (Input.GetKeyDown("escape")){
            pauseScreen.SetActive(true);
        }
    }
}

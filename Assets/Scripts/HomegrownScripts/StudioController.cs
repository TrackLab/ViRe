using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Valve.VR;

public class StudioController : MonoBehaviour
{
    private FeedbackColor feedlight;
    private BVHRecorder recorder;
    private bool isPaused;
    public GameObject pauseScreen;
    

    void Start()
    {
        // Initialize the variables used by this script and the path to Documents
        subscribeControls();

        GameObject light = GameObject.Find("AirText");
        feedlight = light.GetComponent<FeedbackColor>();
        recorder = GetComponent<BVHRecorder>();

        string docPath = System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents";
        string recPath = PlayerPrefs.GetString("RecPath", docPath);
        if (!Directory.Exists(recPath)){recPath = docPath;}
        recorder.directory = recPath;

        UpdateCheck updateCheck = GetComponent<UpdateCheck>();
        updateCheck.check();
    }

    private void subscribeControls(){
        SteamVR_Actions._default.Record[SteamVR_Input_Sources.Any].onStateDown += vrOnRecord;
        SteamVR_Actions._default.Pause[SteamVR_Input_Sources.Any].onStateDown += vrOnPause;
        //TODO: Keyboard controls
    }

    private void vrOnPause(SteamVR_Action_Boolean action, SteamVR_Input_Sources source){pause();}
    private void vrOnRecord(SteamVR_Action_Boolean action, SteamVR_Input_Sources source){toggleRecording();}
    //TODO: Keyboard controls

    public void pause(){
        if (!isPaused){
            pauseScreen.SetActive(true);
            isPaused = true;
        } else {
            pauseScreen.SetActive(false);
            isPaused = false;
        }
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
}

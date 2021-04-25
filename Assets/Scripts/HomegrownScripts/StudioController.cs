using UnityEngine;

public class StudioController : MonoBehaviour
{
    private ActionReader ctrlLister;
    private ControlManager ctrlManager;
    private new GameObject light;
    private FeedbackColor feedlight;
    private BVHRecorder recorder;
    private bool isPaused = false;
    public GameObject pauseScreen;
    

    void Start()
    {
        // Initialize the variables used by this script and the path to Documents
        ctrlLister = new ActionReader();
        subscribeControls();

        ctrlManager = ctrlLister.getControlManager();
        light = GameObject.Find("AirText");
        feedlight = light.GetComponent<FeedbackColor>();
        recorder = GetComponent<BVHRecorder>();
        string recpath = PlayerPrefs.GetString("RecPath", System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents");
        recorder.directory = recpath;
    }

    private void subscribeControls(){
        ctrlLister.getInputAction("XRI BothHands","Record").performed += onRecord;
        ctrlLister.getInputAction("XRI BothHands","Pause").performed += onPause;
        ctrlLister.getInputAction("Keyboard","Record").performed += onRecord;
        ctrlLister.getInputAction("Keyboard","Pause").performed += onPause;
    }

    private void onPause(UnityEngine.InputSystem.InputAction.CallbackContext ctx){
        if (!isPaused){
            ctrlManager.disableControls();
            pauseScreen.SetActive(true);
            isPaused = true;
        } else {
            ctrlManager.enableControls();
            pauseScreen.SetActive(false);
            isPaused = false;
        }
    }
    private void onRecord(UnityEngine.InputSystem.InputAction.CallbackContext ctx){toggleRecording();}

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

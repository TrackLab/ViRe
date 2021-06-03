using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StudioController : MonoBehaviour
{
    private ActionReader ctrlLister;
    private FeedbackColor feedlight;
    private BVHRecorder recorder;
    private bool isPaused;
    public GameObject pauseScreen;
    

    void Start()
    {
        // Initialize the variables used by this script and the path to Documents
        ctrlLister = new ActionReader();
        subscribeControls();

        GameObject light = GameObject.Find("AirText");
        feedlight = light.GetComponent<FeedbackColor>();
        recorder = GetComponent<BVHRecorder>();

        string docPath = System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents";
        string recPath = PlayerPrefs.GetString("RecPath", docPath);
        if (!Directory.Exists(recPath)){recPath = docPath;}
        recorder.directory = recPath;
    }

    private void subscribeControls(){
        ctrlLister.getInputAction("XRI BothHands","Record").performed += onRecord;
        ctrlLister.getInputAction("XRI BothHands","Pause").performed += onPause;
        ctrlLister.getInputAction("Keyboard","Record").performed += onRecord;
        ctrlLister.getInputAction("Keyboard","Pause").performed += onPause;
    }
    
    void OnDisable(){
        ctrlLister.getInputAction("XRI BothHands","Record").performed -= onRecord;
        ctrlLister.getInputAction("XRI BothHands","Pause").performed -= onPause;
        ctrlLister.getInputAction("Keyboard","Record").performed -= onRecord;
        ctrlLister.getInputAction("Keyboard","Pause").performed -= onPause;
    }

    private void onPause(UnityEngine.InputSystem.InputAction.CallbackContext ctx){pause();}
    private void onRecord(UnityEngine.InputSystem.InputAction.CallbackContext ctx){toggleRecording();}
    
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

    public void returnMenu(){SceneManager.LoadScene("Menu");}
}

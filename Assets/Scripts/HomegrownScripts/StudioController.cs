using UnityEngine;
using System.IO;
using Valve.VR;

public class StudioController : MonoBehaviour
{
    public FeedbackColor feedlight;
    public BVHRecorder recorder;
    public IKFootSolver leftLeg, rightLeg;
    public UpdateCheck updateCheck;
    public GameObject pauseScreen;


    void Start()
    {
        // Initialize the variables used by this script and sets the path to Documents in the Recorder
        SteamVR_Actions._default.Record[SteamVR_Input_Sources.Any].onStateDown += vrOnRecord;
        SteamVR_Actions._default.Pause[SteamVR_Input_Sources.Any].onStateDown += vrOnPause;

        string docPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "/Documents";
        string recPath = PlayerPrefs.GetString("RecPath", docPath);
        if (!Directory.Exists(recPath)) { recPath = docPath; }
        recorder.directory = recPath;
        recorder.frameRate = PlayerPrefs.GetInt("RecFPS", 60);


        leftLeg.stepDistance = rightLeg.stepDistance = PlayerPrefs.GetFloat("IKthreshold", leftLeg.stepDistance);
        leftLeg.stepLength = rightLeg.stepLength = PlayerPrefs.GetFloat("IKdistance", leftLeg.stepLength);

        updateCheck.check();
    }

    //These intermediary methods need to exist for proper delegate matching
    private void vrOnPause(SteamVR_Action_Boolean action, SteamVR_Input_Sources source) { pause(); }
    private void vrOnRecord(SteamVR_Action_Boolean action, SteamVR_Input_Sources source) { toggleRecording(); }

    public void pause() { pauseScreen.SetActive(!pauseScreen.activeInHierarchy); }

    public void toggleRecording()
    {
        if (recorder.capturing)
        {
            recorder.capturing = false;
            recorder.saveBVH();
            recorder.clearCapture();
            feedlight.OffAir();
            Debug.Log("Recorder OFF & data saved");
        }
        else
        {
            recorder.capturing = true;
            feedlight.OnAir();
            Debug.Log("Recorder ON");
        }
    }

    public void quitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}

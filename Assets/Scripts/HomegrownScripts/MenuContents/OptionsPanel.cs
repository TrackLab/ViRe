using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using SimpleFileBrowser;

public class OptionsPanel : MonoBehaviour
{
    public Button[] buttons;
    private TMPro.TextMeshProUGUI pathtext;
    private string pathC;
    private int framerate;
    private BVHRecorder recorder;
    private Regex buttonMatch = new Regex("[0-9]+");

    //Loads the recorder parameters saved in Player Preferences, or sets the defaults
    void Start(){
        recorder = GameObject.Find("ViRe_Character").GetComponent<BVHRecorder>();
        pathtext = GameObject.Find("PathField").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        pathC = PlayerPrefs.GetString("RecPath", System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        string fpsL = PlayerPrefs.GetString("RecFPS", "60");
        try{framerate = Int32.Parse(fpsL);}
        catch(FormatException){framerate = 60;}

        writeSettings();
    }

    public void changeFramerate(GameObject button){
        framerate = Int32.Parse(buttonMatch.Match(button.name).Captures[0].ToString());
        
        writeSettings();
    }
    
    public void fileBrowser(){
        pathtext.text = "Check PC";
        StartCoroutine(runFileBrowser());
    }
    
    IEnumerator runFileBrowser(){
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders,false,pathC,null,"Select recording folder","Select");
        if (FileBrowser.Success){pathC = FileBrowser.Result[0];}
        writeSettings();
    }

    //Writes the settings to both Player Preferences and the Recorder, as well as output them to their fields
    private void writeSettings(){
        if (!Directory.Exists(pathC)){
            pathC = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
        pathtext.text = pathC;

        foreach (Button button in buttons){
            if (button.name == framerate+"FPS"){button.interactable = false;}
            else{button.interactable = true;}
        }

        recorder.directory = pathC;
        recorder.frameRate = framerate;

        PlayerPrefs.SetString("RecPath", pathC);
        PlayerPrefs.SetString("RecFPS", framerate.ToString());
        PlayerPrefs.Save();
    }
}

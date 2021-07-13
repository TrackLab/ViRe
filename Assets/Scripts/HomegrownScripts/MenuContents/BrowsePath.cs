using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using SimpleFileBrowser;

public class BrowsePath : MonoBehaviour
{
    private Text pathtext;
    private string pathC;
    private BVHRecorder recorder;

    //Loads the recordings path saved in Player Preferences into the Path field, if it exists
    void Start(){
        recorder = GameObject.Find("ViRe_Character").GetComponent<BVHRecorder>();
        pathtext = GameObject.Find("PathField").GetComponentInChildren<Text>();
        pathC = PlayerPrefs.GetString("RecPath", "");

        if (pathC.Length>0){
            if (Directory.Exists(pathC)){pathtext.text = pathC;}
            else {PlayerPrefs.DeleteKey("RecPath");}
        }
    }
    
    public void fileBrowser(){StartCoroutine(runFileBrowser());}
    
    //Browse for a directory, commit it to Player Preferences, and set it in the Recorder
    IEnumerator runFileBrowser(){
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders,false,pathC,null,"Select recording folder","Select");
        if (FileBrowser.Success){
            pathC = FileBrowser.Result[0];
            PlayerPrefs.SetString("RecPath", pathC);
            PlayerPrefs.Save();
            pathtext.text = pathC;
            recorder.directory = pathC;
        }
    }
}

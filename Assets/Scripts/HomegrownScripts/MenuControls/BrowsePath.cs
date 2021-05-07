using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleFileBrowser;

public class BrowsePath : MonoBehaviour
{
    private Text pathtext;
    private string pathD, pathC;
    void Start(){
        pathtext = GameObject.Find("PathField").GetComponentInChildren<Text>();
        pathD = System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents";
        string pathC = PlayerPrefs.GetString("RecPath", "");
        if (pathC.Length>0){pathtext.text = pathC;}
    }
    
    public void fileBrowser(){StartCoroutine(runFileBrowser());}
    
    IEnumerator runFileBrowser(){
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders,false,pathC,null,"Select recording folder","Select");
        if (FileBrowser.Success){
            pathC = FileBrowser.Result[0];
            PlayerPrefs.SetString("RecPath", pathC);
            pathtext.text = pathC;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ookii.Dialogs;

public class BrowsePath : MonoBehaviour
{
    private Text pathtext;
    private string pathf = System.Environment.GetEnvironmentVariable("USERPROFILE")+"/Documents";
    void Start(){
        pathtext = GameObject.Find("PathField").GetComponentInChildren<Text>();
        string pathc = PlayerPrefs.GetString("RecPath", "");
        if (pathc.Length>0){pathtext.text = pathc;}
    }
    
    public void FileOpener()
    {
       var folbrowse = new Ookii.Dialogs.VistaFolderBrowserDialog();
       folbrowse.ShowDialog();
       if (!string.IsNullOrWhiteSpace(folbrowse.SelectedPath)){pathf = folbrowse.SelectedPath;}
       Debug.Log(pathf);
       pathtext.text = pathf;
       PlayerPrefs.SetString("RecPath", pathf);
       PlayerPrefs.Save();
    }
}

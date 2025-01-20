using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using SimpleFileBrowser;

public class RecOptPanel : MonoBehaviour
{
    public Button[] buttons;
    public TMPro.TextMeshProUGUI pathtext;
    public BVHRecorder recorder;
    private string pathC;
    private int framerate;

    //Loads the recorder parameters saved in Player Preferences, or sets the defaults
    void Start()
    {
        pathC = PlayerPrefs.GetString("RecPath", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        framerate = PlayerPrefs.GetInt("RecFPS", 60);
        updateUI();
    }

    public void changeFramerate(GameObject button)
    {
        framerate = int.Parse(button.name);

        writeSettings();
    }

    public void fileBrowser()
    {
        pathtext.text = "Check PC";
        StartCoroutine(runFileBrowser());
    }

    IEnumerator runFileBrowser()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, pathC, null, "Select recording folder", "Select");
        if (FileBrowser.Success) { pathC = FileBrowser.Result[0]; }
        writeSettings();
    }

    private void updateUI()
    {
        pathtext.text = pathC;

        foreach (Button button in buttons)
        {
            if (button.name == framerate + "FPS") { button.interactable = false; }
            else { button.interactable = true; }
        }
    }

    //Writes the settings to both Player Preferences and the Recorder, as well as output them to their fields
    private void writeSettings()
    {
        if (!Directory.Exists(pathC))
        {
            pathC = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        updateUI();

        recorder.directory = pathC;
        recorder.frameRate = framerate;

        PlayerPrefs.SetString("RecPath", pathC);
        PlayerPrefs.SetInt("RecFPS", framerate);
        PlayerPrefs.Save();
    }
}

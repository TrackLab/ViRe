using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;

public class AudioPlayer : MonoBehaviour
{
    public GameObject playlistGUI;
    public Transform playlistGUIcontent;
    public AudioSource audioSource;
    public TMPro.TextMeshProUGUI timestamp, filename;
    public Image playToggleImage;
    public Sprite playButton, pauseButton;
    private AudioClip currentFile;
    private readonly List<string> filePaths = new();
    private readonly List<GameObject> playlistGUIelements = new();
    private int currentTrackIdx;
    private bool userPause = false;
    private bool switchingTrack = false;
    private bool clearedList = false;

    private int GetNullableListCount<T>(List<T> list)
    {
        int validCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null) validCount++;
        }
        return validCount;
    }

    //Runs every 0.5 seconds to update time and check if it's time to play next track. Runs slower than 1/frame to avoid race conditions when calling functions
    private void UpdateData()
    {
        if (!userPause && !audioSource.isPlaying)
        {
            StartCoroutine(QueueNextTrack());
            return;
        }
        if (audioSource.clip == null) return;
        int currentTime = (int)Math.Round(audioSource.time);
        int currentMinutes = currentTime / 60;
        int currentSeconds = currentTime % 60;
        timestamp.text = currentMinutes + ":" + currentSeconds;
    }

    private void PlayNextTrack()
    {
        filename.text = Path.GetFileNameWithoutExtension(filePaths[currentTrackIdx]);
        audioSource.clip = currentFile;
        audioSource.time = 0;

        togglePlayback();
        switchingTrack = false;
    }

    private void ResetPlayback()
    {
        switchingTrack = false;
        currentTrackIdx = -1;
        userPause = false;
        audioSource.Stop();
        playToggleImage.sprite = playButton;
    }

    public void togglePlayback()
    {
        if (GetNullableListCount(filePaths) == 0) return;
        if (audioSource.isPlaying)
        {
            userPause = true;
            audioSource.Pause();
            playToggleImage.sprite = playButton;
        }
        else
        {
            userPause = false;
            audioSource.Play();
            playToggleImage.sprite = pauseButton;
        }
    }

    public void scrubPlayhead(bool rewind)
    {
        if (GetNullableListCount(filePaths) == 0) return;
        if (rewind)
        {
            if (audioSource.time - 5 < 0) return;
            audioSource.time -= 5;
        }
        else
        {
            if (audioSource.time + 5 > currentFile.length) return;
            audioSource.time += 5;
        }
    }

    public void browseAudio()
    {
        filename.text = "Check PC";
        StartCoroutine(RunAudioBrowser());
    }

    //Runs the file browser and calls to populate arrays and playlist GUI
    private IEnumerator RunAudioBrowser()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Audio", ".mp3", ".wav"));
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select audio for playlist", "Select");

        if (FileBrowser.Success)
        {
            playlistGUI.SetActive(false);
            CancelInvoke();
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                filePaths.Add(FileBrowser.Result[i]);
            }

            InvokeRepeating(nameof(UpdateData), 0, 0.5f);
            StartCoroutine(GeneratePlaylistGUI());
            currentTrackIdx = -1;
        }
        else filename.text = "No audio selected";

    }

    private IEnumerator GeneratePlaylistGUI()
    {
        GameObject track0 = playlistGUIcontent.GetChild(0).gameObject;
        playlistGUIelements.Add(track0);
        track0.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(filePaths[0]);
        for (int i = 1; i < GetNullableListCount(filePaths); i++)
        {
            GameObject nextTrack = Instantiate(track0, playlistGUIcontent);
            nextTrack.name = $"Track_{i}";
            nextTrack.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(filePaths[i]);
            nextTrack.transform.GetChild(0).name = $"Title_{i}";
            nextTrack.transform.GetChild(1).name = $"Delete_{i}";
            playlistGUIelements.Add(nextTrack);
            yield return null;
        }
    }

    // Creates an AudioClip instance of the selected file
    private IEnumerator QueueNextTrack()
    {
        currentFile = null;
        if (GetNullableListCount(filePaths) == 0 || switchingTrack) yield break;
        switchingTrack = true;
        do
        {
            currentTrackIdx++;
            if (currentTrackIdx >= filePaths.Count) currentTrackIdx = 0;
        } while (filePaths[currentTrackIdx] == null);

        AudioType audioType = AudioType.UNKNOWN;
        switch (Path.GetExtension(filePaths[currentTrackIdx]))
        {
            case ".wav":
                audioType = AudioType.WAV;
                break;
            case ".mp3":
                audioType = AudioType.MPEG;
                break;
        }
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePaths[currentTrackIdx], audioType);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
            filename.text = "File error";
            switchingTrack = false;
        }
        else
        {
            currentFile = DownloadHandlerAudioClip.GetContent(www);
            PlayNextTrack();
        }
    }

    public void togglePlaylistGUI()
    {
        if (filePaths.Count == 0 && !clearedList) return;
        if (clearedList) clearedList = false;
        playlistGUI.SetActive(!playlistGUI.activeInHierarchy);
    }

    public void skipToTrack(GameObject button)
    {
        //Guard against off-screen button clicks
        if (button.transform.parent.localPosition.y <= -250 || button.transform.parent.localPosition.y >= -9) return;
        int selectID = int.Parse(button.name.Split('_')[1]);
        currentTrackIdx = selectID - 1;
        StartCoroutine(QueueNextTrack());
    }

    public void deleteItem(GameObject button)
    {
        //Guard against off-screen button clicks
        if (button.transform.parent.localPosition.y <= -250 || button.transform.parent.localPosition.y >= -9) return;
        int selectID = int.Parse(button.name.Split('_')[1]);
        switch (GetNullableListCount(filePaths))
        {
            case 0:
                return;
            case 1:
                clearPlaylist();
                return;
        }
        if (selectID == currentTrackIdx) StartCoroutine(QueueNextTrack());
        filePaths[selectID] = null;
        Destroy(playlistGUIelements[selectID]);
        playlistGUIelements[selectID] = null;
    }

    public void clearPlaylist()
    {

        GameObject newElement0 = null;

        foreach (GameObject playlistGUIelement in playlistGUIelements)
        {
            if (newElement0 == null) newElement0 = playlistGUIelement;
            else Destroy(playlistGUIelement);
        }

        newElement0.name = "Track_0";
        newElement0.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Playlist Empty";
        newElement0.transform.GetChild(0).name = $"Title_0";
        newElement0.transform.GetChild(1).name = $"Delete_0";

        clearedList = true;
        filename.text = "No audio selected";

        filePaths.Clear();
        playlistGUIelements.Clear();

        CancelInvoke();
        ResetPlayback();
    }

}

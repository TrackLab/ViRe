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
    private AudioSource audioSource;
    private TMPro.TextMeshProUGUI timestamp, filename;
    private Image playToggleImage;
    public Sprite playButton, pauseButton;
    private List<AudioClip> playlist;
    private List<string> filenames;
    private int currentTrack;
    private bool userPause = false;
    private bool switchingTrack = false;

    void Start()
    {
       audioSource = GameObject.Find("AudioPlayerSource").GetComponent<AudioSource>();
       timestamp = GameObject.Find("Timestamp").GetComponent<TMPro.TextMeshProUGUI>();
       filename = GameObject.Find("Filename").GetComponent<TMPro.TextMeshProUGUI>();
       playToggleImage = GameObject.Find("Play").GetComponent<Image>();

       playlist = new List<AudioClip>();
       filenames = new List<string>();
       
       InvokeRepeating("updateData", 0, 0.5f);
    }

    //Runs every 0.5 seconds to update time and check if it's time to play next track. Runs slower than 1/frame to avoid race conditions when calling functions
    private void updateData(){
        int currentTime = (int) Math.Round(audioSource.time);
        int currentMinutes = currentTime / 60;
        int currentSeconds = (currentTime % 60);
        timestamp.text = currentMinutes+":"+currentSeconds;

        if (!userPause && !audioSource.isPlaying){playNextTrack();}
    }

    private void playNextTrack(){
        if (playlist.Count == 0){return;}
        if (switchingTrack){return;}
        if (currentTrack+1 >= playlist.Count){
            endPlaylist();
            return;
        }

        switchingTrack = true;
        currentTrack++;
        if (playlist[currentTrack]==null){currentTrack++;}

        filename.text = filenames[currentTrack];
        audioSource.clip = playlist[currentTrack];
        audioSource.time = 0;

        togglePlayback();
        switchingTrack = false;
    }

    //Loop at the end of the playlist and pauses
    private void endPlaylist(){
        currentTrack = -1;
        playNextTrack();
        userPause = true;
        audioSource.Stop();
        playToggleImage.sprite = playButton;
    }

    public void togglePlayback(){
        if (playlist.Count == 0){return;}
        if (audioSource.isPlaying){
            userPause = true;
            audioSource.Pause();
            playToggleImage.sprite = playButton;
        }
        else {
            userPause = false;
            audioSource.Play();
            playToggleImage.sprite = pauseButton;
        }
    }

    //Scrubs the playback forwards or backwards by X seconds
    public void scrubPlayhead(bool rewind){
        if (playlist.Count == 0){return;}
        if (rewind){
            if (audioSource.time-5 < 0){return;}
            audioSource.time -= 5;
        }
        else {
            if (audioSource.time+5 > playlist[currentTrack].length){return;}
            audioSource.time += 5;
        }
    }

    //This bunch of coroutines cause lag, plain and simple.
    //Since all of these methods manipulate stuff with the Unity API, I couldn't send them into other threads
    //Sorry about the solid 2 or more seconds of a freeze that you'll experience, but you can blame Unity's lack of good multithreading for it (definitely not my skills)

    public void browseAudio(){
        filename.text = "Check PC";
        StartCoroutine(runAudioBrowser());
    }

    //Runs the file browser and calls to populate arrays and playlist GUI
    IEnumerator runAudioBrowser(){
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Audio", ".mp3", ".wav"));
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select audio for playlist", "Select");

        if (FileBrowser.Success){
            for (int i = 0; i<FileBrowser.Result.Length; i++){
                yield return addAudioClipInstance(FileBrowser.Result[i],i);
                filenames.Add(Path.GetFileNameWithoutExtension(FileBrowser.Result[i]));
            }

            generatePlaylistGUI();
            currentTrack = -1;
            playNextTrack();
        }
    }

    //Populates the playlist management GUI
    private void generatePlaylistGUI(){
        Transform track0 = playlistGUIcontent.GetChild(0);
        track0.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = filenames[0];
        for (int i = 0; i < filenames.Count-1; i++){
            GameObject nextTrack = GameObject.Instantiate(playlistGUIcontent.GetChild(0).gameObject,playlistGUIcontent);
            nextTrack.name = "Track"+(i+1);
            nextTrack.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = filenames[i+1];
            nextTrack.transform.GetChild(0).name = "Title"+(i+1);
            nextTrack.transform.GetChild(1).name = "Delete"+(i+1);
        }
    }

    //Gets AudioClip instances of the selected files
    IEnumerator addAudioClipInstance(string file, int index){
        AudioType audioType = AudioType.UNKNOWN;
        switch (Path.GetExtension(file)){
            case ".wav":
                    audioType = AudioType.WAV;
                    break;
            case ".mp3":
                    audioType = AudioType.MPEG;
                    break;
        }
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://"+file, audioType)){
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                playlist.Add(DownloadHandlerAudioClip.GetContent(www));
            }
        }
    }

    public void togglePlaylistGUI(){
        if (playlist.Count == 0){return;}
        playlistGUI.SetActive(!playlistGUI.activeInHierarchy);
    }

    public void skipToTrack(GameObject button){
        int selectID = (int) Char.GetNumericValue(button.name[button.name.Length-1]);
        currentTrack = selectID-1;
        playNextTrack();
    }
    
    //Deletes a track from the playlist and skips to the next one
    public void deleteItem(GameObject button){
        Debug.Log("Before: "+playlist.Count);
        int selectID = (int) Char.GetNumericValue(button.name[button.name.Length-1]);
        if (playlist.Count == 1){
            clearPlaylist();
            return;
        }
        playNextTrack();
        try{
            playlist.RemoveAt(selectID);
            filenames.RemoveAt(selectID);
        } catch (ArgumentOutOfRangeException){}
        Debug.Log("After: "+playlist.Count);
        Destroy(playlistGUIcontent.GetChild(selectID).gameObject);
    }

    public void clearPlaylist(){
       for (int i = 1; i < playlist.Count; i++){
           Destroy(playlistGUIcontent.GetChild(i).gameObject);
       }
       playlistGUIcontent.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Playlist Empty";

       playlist.Clear();
       filenames.Clear();

       endPlaylist();
    }
    
}

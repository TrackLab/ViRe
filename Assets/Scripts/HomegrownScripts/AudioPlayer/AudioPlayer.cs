using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.IO;
using SimpleFileBrowser;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private TMPro.TextMeshProUGUI timestamp, filename;
    private Image playToggleImage;
    public Sprite playButton, pauseButton;
    private AudioClip[] playlist;
    private string[] filenames;
    private int currentTrack;
    private bool userPause = false;
    private bool switchingTrack = false;

    void Start()
    {
       audioSource = GameObject.Find("AudioPlayerSource").GetComponent<AudioSource>();
       timestamp = GameObject.Find("Timestamp").GetComponent<TMPro.TextMeshProUGUI>();
       filename = GameObject.Find("Filename").GetComponent<TMPro.TextMeshProUGUI>();
       playToggleImage = GameObject.Find("Play").GetComponent<Image>();
       InvokeRepeating("updateData", 0, 0.5f);
    }

    private void updateData(){
        int currentTime = (int) Math.Round(audioSource.time);
        int currentMinutes = currentTime / 60;
        int currentSeconds = (currentTime % 60);
        timestamp.text = currentMinutes+":"+currentSeconds;
        if (!userPause && !audioSource.isPlaying){playNextTrack();}
    }

    private void playNextTrack(){
        if (playlist == null){return;}
        if (switchingTrack){return;}
        if (currentTrack+1 == playlist.Length){return;}
        switchingTrack = true;
        currentTrack++;
        filename.text = filenames[currentTrack];
        audioSource.clip = playlist[currentTrack];
        audioSource.time = 0;
        togglePlayback();
        switchingTrack = false;
    }

    public void togglePlayback(){
        if (playlist == null){return;}
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

    public void scrubPlayhead(bool rewind){
        if (playlist == null){return;}
        if (rewind){
            if (audioSource.time-5 < 0){return;}
            audioSource.time -= 5;
        }
        else {
            if (audioSource.time+5 > playlist[currentTrack].length){return;}
            audioSource.time += 5;
        }
    }

    public void browseAudio(){
        StartCoroutine(runAudioBrowser());
    }

    IEnumerator runAudioBrowser(){
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Audio", ".mp3", ".wav"));
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select audio for playlist", "Select");
        if (FileBrowser.Success){
            playlist = new AudioClip[FileBrowser.Result.Length];
            filenames = new string[FileBrowser.Result.Length];
            for (int i = 0; i<FileBrowser.Result.Length; i++){
                yield return addAudioClipInstance(FileBrowser.Result[i],i);
                filenames[i] = Path.GetFileNameWithoutExtension(FileBrowser.Result[i]);
            }
            currentTrack = -1;
            playNextTrack();
        }
    }

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
                playlist[index] = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
    
}

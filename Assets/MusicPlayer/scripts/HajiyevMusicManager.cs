using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(AudioSource))]
public class HajiyevMusicManager : MonoBehaviour {
    #region Public Inspector Attributes
    [Tooltip("Clips to play")]
    public List<AudioClip> playList;

    [Tooltip("Keep playing this clip till clip is changed")]
    public bool repeat = false;

    [Tooltip("When clip is done, go to next clip")]
    public bool autoAdvance = true;

    [Tooltip("When end of playlist is reached, go back to start on advance")]
    public bool recycle = true;

    [Tooltip("Begin playing first clip as soon as the MusicManager instantiates")]
    public bool playOnAwake = false;

    [Tooltip("If nonzero, fade in on resume from pause or change track from middle of song for that many seconds")]
    [Range(0f, 10f)]
    public float fadeInTime = 0.5f;

    [Tooltip("If nonzero, fade out on pause or change track from middle of song for that many seconds")]
    [Range(0f, 10f)]
    public float fadeOutTime = 1f;

    [Tooltip("Volume control")]
    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Tooltip("If true, on awake, load audioclips from resources directories into playlist")]
    public bool loadFromResources = true;

    [Tooltip("If true, use as a Singleton class and don't destroy when scene is changed")]
    public bool dontDestroyOnLoad = true;
    #endregion

    #region Public Scripted Setup
    /// <summary>
    /// Remove all clips from playlist
    /// </summary>
    public void ClearPlayList() {
        playList.Clear();
    }

    /// <summary>
    /// Add a clip to the end of the playlist
    /// </summary>
    /// <param name="clip"></param>
    public void AddToPlayList(AudioClip clip) {
        playList.Add(clip);
    }

    /// <summary>
    /// Remove selected track from playlist (0 is first track)
    /// </summary>
    /// <param name="index"></param>
    public void RemoveFromPlayList(int index) {
        playList.RemoveAt(index);
    }

    /// <summary>
    /// Load given clip from resources folder(s) into playlist
    /// </summary>
    /// <param name="name"></param>
    /// <returns>False if clip could not be loaded</returns>
    public bool LoadClipFromResources(string name) {
        AudioClip clip = Resources.Load<AudioClip>(name);
        if (clip) {
            playList.Add(clip);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Load all clips in resources folders into playlist, but without duplication
    /// </summary>
    /// <param name="path">optional subfolder of resources folder</param>
    /// <returns>Number of unique clips added to playlist</returns>
    public int LoadClipsFromResources(string path="") {   
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);
        int count = 0;
        foreach(AudioClip clip in clips) {
            if (!playList.Contains(clip)) {
                count++;
                playList.Add(clip);
            }
        }
        return count;
    }

    /// <summary>
    /// Get the audioclip at specified track in playlist (0 is first track)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AudioClip GetPlayListClip(int index) {
        if (index >= 0 && index < GetPlayListLength()) {
            return playList[index];
        } else {
            return null;
        }
    }

    /// <summary>
    /// Get the number of tracks in the playlist
    /// </summary>
    /// <returns></returns>
    public int GetPlayListLength() {
        return playList.Count;
    }

    /// <summary>
    /// Set the fade-in time when resuming from pause
    /// </summary>
    /// <param name="time"></param>
    public void SetFadeIn(float time) {
        if (time < 0 || time > 10) {
            throw new UnityException("Fade in time out of range: " + time);
        }
        fadeInTime = time;
    }

    /// <summary>
    /// Set the fade-out time when pausing or stopping in the middle of a clip
    /// </summary>
    /// <param name="time"></param>
    public void SetFadeOut(float time) {
        if (time < 0 || time > 10) {
            throw new UnityException("Fade out time out of range: " + time);
        }
        fadeOutTime = time;
    }

    /// <summary>
    /// Adjust the volume control, from 0 to 1  (use 1 for "turn it up to eleven")
    /// </summary>
    /// <param name="amount"></param>
    public void SetVolume(float amount) {
        volume = Mathf.Clamp(amount, 0, 1);
    }

    /// <summary>
    /// Set autoadvance (like a CD player, next track when this track finishes),
    /// only works if not repeating current track.
    /// </summary>
    public void AutoAdvance() {
        autoAdvance = true;
    }

    /// <summary>
    /// Unset autoadvance, stop playing when track ends
    /// </summary>
    public void NoAutoAdvance() {
        autoAdvance = false;
    }

    /// <summary>
    /// Repeat current track until manually changed
    /// </summary>
    public void Repeat() {
        repeat = true;
    }

    /// <summary>
    /// Don't repeat track--advance or stop when track is done
    /// </summary>
    public void NoRepeat() {
        repeat = false;
    }

    /// <summary>
    /// Repeat playlist from beginning when end is reached
    /// </summary>
    public void Recycle() {
        recycle = true;
    }

    /// <summary>
    /// Stop playing when end of playlist is reached
    /// </summary>
    public void NoRecycle() {
        recycle = false;
    }
    #endregion

    #region Public Controls
    /// <summary>
    /// Toggle between playing and pausing of current track
    /// </summary>
    public void PlayPauseToggle() {
        if (!IsPlaying()) {
            Play();
        } else {
            Pause();
        }
    }

    /// <summary>
    /// Toggle the Repeat (loop this track) flag
    /// </summary>
    public void ToggleRepeat() {
        if (repeat) {
            NoRepeat();
        } else {
            Repeat();
        }
    }

    /// <summary>
    /// Toggle the Recycle (loop the whole playlist) flag
    /// </summary>
    public void ToggleRecycle() {
        if (recycle) {
            NoRecycle();
        } else {
            Recycle();
        }
    }

    /// <summary>
    /// Increment the volume
    /// </summary>
    public void VolumeUp(float amount = 0.05f) {
        SetVolume(volume + amount);
    }

    /// <summary>
    /// Decrement the volume
    /// </summary>
    public void VolumeDown(float amount = 0.05f) {
        SetVolume(volume - amount);
    }

    /// <summary>
    /// Toggle between muting and playing at last-set volume
    /// </summary>
    public void ToggleMute() {
        if (saveVolume < 0) {
            SendControlMessage(Message.MusicManager_Mute);
            saveVolume = volume;
            SetVolume(0);
        } else {
            SendControlMessage(Message.MusicManager_UnMute);
            SetVolume(saveVolume);
            saveVolume = -1;
        }
    }

    /// <summary>
    ///   Start playing, or resume from pause
    /// </summary>
    public void Play() {
        SendControlMessage(Message.MusicManager_Play);
        if (!IsPlaying()) {
            if (IsPaused()) {
                ResumeFromPause();
                audioSource.time = saveTime;
            } else {
                saveTime = 0;
                Rewind();
                ResumeFromPause();
                audioSource.time = 0;
            }
        }
    }

    /// <summary>
    /// Stop playing, but don't lose place in clip
    /// </summary>
    public void Pause() {
        SendControlMessage(Message.MusicManager_Pause);
        saveTime = 0;
        if (IsPlaying()) {
            isPlaying = false;
            Fade(volume, 0, fadeOutTime);
            saveTime = audioSource.time;
        }
    }

    /// <summary>
    /// Go back to beginning of playlist
    /// </summary>
    public void Rewind() {
        if (IsPlaying()) {
            Pause();
            SetTrack(0);
            Play();
        } else {
            SetTrack(0);
            Pause();
        }
    }

    /// <summary>
    /// Go back to beginning of clip
    /// </summary>
    public void RewindClip() {
        if (IsPlaying()) {
            Pause();
            audioSource.time = 0;
            Play();
        } else {
            audioSource.time = 0;
            Pause();
        }
    }

    /// <summary>
    /// Stop playing and rewind to beginning of playlist
    /// </summary>
    public void Stop() {
        Pause();
        Rewind();
    }

    /// <summary>
    /// Stop playing and rewind to beginning of clip
    /// </summary>
    public void StopClip() {
        Pause();
        Invoke("RewindClip", fadeOutTime);
    }

    /// <summary>
    /// Advance to next song on playlist.
    /// </summary>
    public void Next() {
        if (IsPlaying()) {
            Pause();
            SetTrack(NextTrack());
            Play();
        } else {
            SetTrack(NextTrack());
            Pause();
        }
    }

    /// <summary>
    /// Go back to previous song on playlist
    /// </summary>
    public void Previous() {
        if (IsPlaying()) {
            Pause();
            SetTrack(PreviousTrack());
            Play();
        } else {
            SetTrack(PreviousTrack());
            Pause();
        }
    }

    /// <summary>
    /// Advance a bit in the song
    /// </summary>
    /// <param name="seconds"></param>
    public void MoveForward(float seconds = 10f) {
        if (IsPlaying()) {
            Pause();
            if (audioSource.clip) {
                audioSource.time = Mathf.Clamp(audioSource.time + seconds, 0, audioSource.clip.length);
            }
            Play();
        } else {
            if (audioSource.clip) {
                audioSource.time = Mathf.Clamp(audioSource.time + seconds, 0, audioSource.clip.length);
            }
        }

    }

    /// <summary>
    /// Go back a bit in the song
    /// </summary>
    /// <param name="seconds"></param>
    public void MoveBackward(float seconds = 10f) {
        if (IsPlaying()) {
            Pause();
            if (audioSource.clip) {
                audioSource.time = Mathf.Clamp(audioSource.time - seconds, 0, audioSource.clip.length);
            }
            Play();
        } else {
            if (audioSource.clip) {
                audioSource.time = Mathf.Clamp(audioSource.time - seconds, 0, audioSource.clip.length);
            }
        }

    }

    /// <summary>
    /// Reorder the playlist randomly
    /// </summary>
    public void Shuffle() {
        if (IsPlaying()) {
            Stop();
            //reorder playlist
            Randomize(playList);
            Play();
        } else if (IsPaused()) {
            Stop();
            //reorder playlist
            Randomize(playList);
        } else {
            Randomize(playList);
        }
    }


    #endregion

    #region Public Queries
    /// <summary>
    /// Track number playing (from 0 to number of tracks - 1)
    /// </summary>
    /// <returns></returns>
    public int CurrentTrackNumber() {
        return trackNumber;
    }

    /// <summary>
    /// Audio clip now playing (even if paused)
    /// </summary>
    /// <returns></returns>
    public AudioClip NowPlaying() {
        return playList[trackNumber];
    }

    /// <summary>
    /// True if there is a current clip and it is not paused
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying() {
        return isPlaying;
    }

    /// <summary>
    /// True if there is a current clip and it is paused
    /// </summary>
    /// <returns></returns>
    public bool IsPaused() {
        return !IsPlaying() && CurrentTrackNumber() >= 0;
    }

    /// <summary>
    /// Time into the current clip
    /// </summary>
    /// <returns></returns>
    public float TimeInSeconds() {
        return audioSource.time;
    }

    /// <summary>
    /// Length of the current clip, or 0 if no clip
    /// </summary>
    /// <returns></returns>
    public float LengthInSeconds() {
        if (!audioSource.clip) {
            return 0;
        }
        return audioSource.clip.length;
    }

    #endregion


    #region Internal Manager State
    private AudioSource audioSource;

    int trackNumber = -1; //negative means no active track

    bool isPlaying = false;

    float fadeStartVolume;
    float fadeEndVolume;
    float fadeTime = 0;
    float fadeStartTime;
    float saveVolume = -1;
    float saveTime = 0;


    #endregion

    #region Internal Singleton
    private static HajiyevMusicManager _instance;

    public static HajiyevMusicManager instance {
        get {
            if (_instance == null) {//in case not awake yet
                _instance = FindObjectOfType<HajiyevMusicManager>();
            }
            return _instance;
        }
    }

    void Awake() {
        // if the singleton hasn't been initialized yet
        if (dontDestroyOnLoad) {
            if (_instance != null && _instance != this) {
                Debug.LogError("Duplicate singleton " + this.gameObject + " created; destroying it now");
                Destroy(this.gameObject);
            }

            if (_instance != this) {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        } else {
            _instance = this;
        }
    }
    #endregion

    #region Internal Methods
    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = repeat && !autoAdvance;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
        if (loadFromResources) {
            LoadClipsFromResources();
        }
        Rewind();
        if (playOnAwake) {
            Play();
        }
    }

    // Update is called once per frame
    void Update() {
        if (fadeTime > 0) {
            SetFadeVolume();
        } else {
            audioSource.volume = volume;
        }
        if (isPlaying && !audioSource.isPlaying && audioSource.time == 0) {
            ClipFinished();
        }
        audioSource.loop = repeat;
    }

    void SetFadeVolume() {
        fadeEndVolume = Mathf.Clamp(fadeEndVolume, 0, volume);//in case volume control adjusted
                                                              //during fade
        float t = (Time.time - fadeStartTime) / fadeTime;
        if (t >= 1) {
            fadeTime = 0;
            if (fadeEndVolume == 0) {
                audioSource.Stop();
                audioSource.time = saveTime;
            }
            audioSource.volume = volume;
        } else {
            audioSource.volume = (1 - t) * fadeStartVolume + t * fadeEndVolume;
        }
    }

    void ClipFinished() {
        if (autoAdvance) {
            Next();
        } else {
            Pause();
        }
    }

    void ResumeFromPause() {
        if (CurrentTrackNumber() >= 0) {
            isPlaying = true;
            Fade(0, volume, fadeInTime);
        }
    }

    void SetTrack(int trackNum) {
        if (CurrentTrackNumber() != trackNum) {
            SendControlMessage(Message.MusicManager_ChangeTrack);
        }
        saveTime = 0;
        if (GetPlayListLength() == 0) {
            trackNumber = -1;
        } else if (trackNum >= 0 && trackNum < GetPlayListLength()) {
            trackNumber = trackNum;
        } else if (recycle) {
            trackNumber = (trackNum % GetPlayListLength() + GetPlayListLength()) % GetPlayListLength();
        } else {
            trackNumber = -1;
        }
    }

    int NextTrack() {
        int trackNum = trackNumber + 1;
        if (trackNum >= 0 && trackNum < GetPlayListLength()) {
            return trackNum;
        } else if (recycle) {
            return (trackNum % GetPlayListLength() + GetPlayListLength()) % GetPlayListLength();
        } else {
            Stop();
            return -1;
        }
    }

    int PreviousTrack() {
        int trackNum = trackNumber - 1;
        if (trackNum >= 0 && trackNum < GetPlayListLength()) {
            return trackNum;
        } else if (recycle) {
            return (trackNum % GetPlayListLength() + GetPlayListLength()) % GetPlayListLength();
        } else {
            return -1;
        }
    }

    void Fade(float startVolume, float endVolume, float time) {
        if (audioSource.isPlaying) {
            fadeStartTime = Time.time;
            fadeStartVolume = startVolume;
            fadeEndVolume = endVolume;
            fadeTime = time;
            audioSource.volume = startVolume;

            if (startVolume == 0) {
                audioSource.Stop();
                audioSource.clip = NowPlaying();
                audioSource.Play();
            }
        } else {
            if (startVolume == 0) {
                audioSource.clip = NowPlaying();
                audioSource.volume = endVolume;
                audioSource.Play();
            }
        }
    }

    void Randomize(List<AudioClip> list) {
        for (int i = 0; i < list.Count - 1 /*because random(a,a) is inclusive*/; i++) {
            int j = Random.Range(i + 1, list.Count);
            AudioClip tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
    #endregion

    #region Messages
    [Tooltip("Send control messages like MusicManager_Play, etc., to scripts in user-supplied parent game objects (advanced)")]
    public bool sendControlMessagesUpward = false;

    [Tooltip("Send control messages like MusicManager_Play, etc., to scripts in user-supplied child game objects (advanced)")]
    public bool sendControlMessagesDownward = false;

    [Tooltip("Send control messages like MusicManager_Play, etc., to user-supplied scripts in this game object (advanced)")]
    public bool sendControlMessagesToThisGameObject = true;

    [Tooltip("Log control messages like MusicManager_Play, etc., to the Unity console or the executable's log file")]
    public bool logControlMessages = false;

    void SendControlMessage(Message message) {
        string message_string = message.ToString();
        if (sendControlMessagesUpward) {
            SendMessageUpwards(message_string, SendMessageOptions.DontRequireReceiver);
        }
        if (sendControlMessagesDownward) {
            BroadcastMessage(message_string, SendMessageOptions.DontRequireReceiver);
        }
        if (sendControlMessagesToThisGameObject) {
            SendMessage(message_string, SendMessageOptions.DontRequireReceiver);
        }
        if (logControlMessages) {
            Debug.Log("Control Message: \"" + message_string + "\"");
        }
    }

    private enum Message {
        MusicManager_Play,
        MusicManager_Pause,
        MusicManager_Mute,
        MusicManager_UnMute,
        MusicManager_ChangeTrack
    }

    #endregion

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NowPlaying : MonoBehaviour {

    public Text nowPlayingText;
    public UnityEngine.UI.Slider musicLength;


    void Update () {
      
        if (HajiyevMusicManager.instance.CurrentTrackNumber() >= 0) {
            string timeText = SecondsToMS(HajiyevMusicManager.instance.TimeInSeconds());
            string lengthText = SecondsToMS(HajiyevMusicManager.instance.LengthInSeconds());

            nowPlayingText.text = "" + (HajiyevMusicManager.instance.CurrentTrackNumber() + 1) + ".  " +
                HajiyevMusicManager.instance.NowPlaying().name
                + " (" + timeText + "/" + lengthText + ")" ;

            musicLength.value = HajiyevMusicManager.instance.TimeInSeconds();
            musicLength.maxValue = HajiyevMusicManager.instance.LengthInSeconds();
            
           
        }
        else {
            nowPlayingText.text = "-----------------";
        }
	}

    string SecondsToMS(float seconds) {
        return string.Format("{0:D2}:{1:D2}", ((int)seconds)/60, ((int)seconds)%60);
    }
}

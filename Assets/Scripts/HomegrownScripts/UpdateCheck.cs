using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class UpdateCheck : MonoBehaviour
{

    public TMPro.TextMeshProUGUI versionText;
    public GameObject versionNotifWindow;

    public void check()
    {
        StartCoroutine(getUpdate());
    }

    IEnumerator getUpdate(){
        UnityWebRequest net = UnityWebRequest.Get("https://api.github.com/repos/TrackLab/ViRe/releases/latest");
        yield return net.SendWebRequest();
        if (net.result != UnityWebRequest.Result.Success){yield break;}

        string currentVersion = Application.version.TrimEnd('A');
        string webVersion = findRelease(net.downloadHandler.text);
        string webVtrimmed = webVersion.TrimEnd('A');

        if (!webVtrimmed.Equals(currentVersion) && !currentVersion.Equals("")){
            setNotifier(webVersion);
        }
    }

    private string findRelease(string src){
        if (src.Contains("v") && src.Contains("\"")){
            int Start = src.IndexOf("v", 0) + 1;
            int End = src.IndexOf("\"", Start);
            return src.Substring(Start, End - Start);
        }
        return "";
    }

    private void setNotifier(string webVersion){
        versionNotifWindow.SetActive(true);
        versionText.text = webVersion;
    }
}

using System.Collections;
using System.Text.RegularExpressions;
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
        if (!webVersion.Equals(currentVersion) && !currentVersion.Equals("")){
            setNotifier(webVersion+'A');
        }
    }

    private string findRelease(string input)
    {
        Regex regex1 = new Regex("\"tag_name\": \"(?:[^\"]|\"\")*\",", RegexOptions.IgnoreCase);
        Regex regex2 = new Regex("([0-9]+(\\.[0-9]+)+)", RegexOptions.IgnoreCase);
        Match matchPre = regex1.Match(input);
        Match matchFin = regex2.Match(matchPre.ToString());
        return matchFin.Captures[0].ToString();
    }

    private void setNotifier(string webVersion){
        versionNotifWindow.SetActive(true);
        versionText.text = webVersion;
    }
}

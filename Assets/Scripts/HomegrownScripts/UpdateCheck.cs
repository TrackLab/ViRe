using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

[System.Serializable]
public class GitData
{
    public string tag_name;

    public static GitData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GitData>(jsonString);
    }
}


public class UpdateCheck : MonoBehaviour
{

    public TMPro.TextMeshProUGUI newVersionText;
    public TMPro.TextMeshProUGUI currentVersionText;
    public GameObject versionNotificationWindow;

    public void check()
    {
        StartCoroutine(GetUpdate());
    }

    //Get the latest release version from GitHub's API and comapre it to the current app version
    IEnumerator GetUpdate()
    {
        UnityWebRequest net = UnityWebRequest.Get("https://api.github.com/repos/TrackLab/ViRe/releases/latest");
        yield return net.SendWebRequest();
        if (net.result != UnityWebRequest.Result.Success) { yield break; }
        string incomingVersion = JsonUtility.FromJson<GitData>(net.downloadHandler.text).tag_name;
        incomingVersion = incomingVersion.TrimStart('V');
        if (int.TryParse(incomingVersion[^1..], out _)) incomingVersion += 'A';

        if (!incomingVersion.Equals(Application.version) && !Application.version.Equals(""))
        {
            SetNotifier(incomingVersion, Application.version);
        }
    }

    private void SetNotifier(string webVersion, string currentVersion)
    {
        versionNotificationWindow.SetActive(true);

        newVersionText.text = webVersion;
        currentVersionText.text = currentVersion;
    }

    public void OpenGitHubPage()
    {
        Application.OpenURL("https://github.com/TrackLab/ViRe/releases");
    }
}

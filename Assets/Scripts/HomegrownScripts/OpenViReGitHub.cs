using System.Collections;
using UnityEngine;

public class OpenViReGitHub : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenGitHubPage()
    {
        Application.OpenURL("https://github.com/TrackLab/ViRe/releases");
    }

}

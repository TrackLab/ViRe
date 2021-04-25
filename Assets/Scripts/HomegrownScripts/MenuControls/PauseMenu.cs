using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void returnMenu(){
        SceneManager.LoadScene(0);
    }
    public void returnStudio(){
        GameObject.Find("PauseScreen").SetActive(false);
    }
}

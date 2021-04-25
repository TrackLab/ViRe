using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animator trns;

    public void PlayGame(){trns.SetTrigger("Run");}

    void Awake(){
        trns = GetComponentInParent<Animator>();
    }

    void Update(){if (trns.GetCurrentAnimatorStateInfo(0).IsName("MenuFadeDone")){SceneManager.LoadScene("RecPlane");}}

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}

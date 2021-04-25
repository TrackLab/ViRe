using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animator trns;

    public void PlayGame()
    {
        trns = GameObject.Find("Canvas").GetComponent<Animator>();
        trns.SetTrigger("Run");
    }

    void Update(){
        try{
        if (trns.GetCurrentAnimatorStateInfo(0).IsName("MenuFadeDone")){SceneManager.LoadScene(1);}
        }
        catch{}
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}

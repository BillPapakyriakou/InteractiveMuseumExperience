using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // load next scene in queue
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();   // works only on final build
    }
}
